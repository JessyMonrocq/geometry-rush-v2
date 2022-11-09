using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        public bool isGrounded;

        public float rot;
        public float rotOffset;

        public Rigidbody rb;

        public float jumpAmount = 7;
        public float gravityScale = 5;
        float jumpForce;
        public GameObject particles;
        public GameObject Death;

        public TextMeshPro nb;
        private int nbEssais = 1;

        public GameObject rightController;
        public GameObject leftController;
        public GameObject pauseMenu;
        public GameObject essaisUI;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            rb.maxDepenetrationVelocity = 1;
            
        }

        public void PauseMenuDown()
        {
            essaisUI.SetActive(true);
            pauseMenu.SetActive(false);
            rightController.SetActive(false);
            leftController.SetActive(false);
            Time.timeScale = 1;
        }

        void Update()
        {
            jumpForce = Mathf.Sqrt(jumpAmount * -2 * (Physics.gravity.y * gravityScale));

            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.position = new Vector3( pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction).x, this.transform.position.y,pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction).z);
            
            }
            
            if (Input.GetButton("Jump") && isGrounded && Time.timeScale == 1) {
                isGrounded = false;
                //rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
            }

            if (Input.GetButton("Pause") && Time.timeScale == 1)
            {
                essaisUI.SetActive(false);
                pauseMenu.SetActive(true);
                rightController.SetActive(true);
                leftController.SetActive(true);
                Time.timeScale = 0;             
            }

            if (!isGrounded)
            {
                transform.rotation = Quaternion.Euler(0, 0, rot);
                rot += rotOffset;
                particles.SetActive(false);
            }
            else
            {
                particles.SetActive(true);


            }
        }

        void FixedUpdate() {
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        void OnCollisionEnter(Collision other) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rot = 0;
            isGrounded = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Death.SetActive(true);
            Death.GetComponent<ParticleSystem>().Play();
            var rend = rb.GetComponent<Renderer>();
            rend.enabled = false;
            nbEssais++;
            nb.text = nbEssais.ToString();
            distanceTravelled = 0.1f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(0, 0.75f, 0);
            //rb.GetComponent<PathFollower>().enabled = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void OnCollisionExit(Collision other) {
            isGrounded = false;    
        }
    }
}