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
        public float speed;
        float distanceTravelled;

        public bool isGrounded;

        public float rot;
        public float rotOffset;

        public Rigidbody rb;

        public float jumpAmount;
        public float gravityScale = 5;
        public float jumpForce;
        public GameObject particles;
        public GameObject Death;
        public GameObject SpaceShip;

        public TextMeshPro nb;
        private int nbEssais = 1;

        public GameObject rightController;
        public GameObject leftController;
        public GameObject pauseMenu;
        public GameObject essaisUI;
        public GameObject audioSync;

        void Start() { 
            SpaceShip.SetActive(false);
            rb.mass = 0.075f;
            Physics.gravity = new Vector3(0, -9.81F, 0);

            pathCreator = PathCreator.FindObjectOfType<PathCreator>();
            if (pathCreator != null)
            {
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
            audioSync.GetComponent<AudioSource>().Play(0);
        }

        public void PauseMenuBack()
        {
            SceneManager.LoadScene("MainMenu");
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
                if (transform.position.x < -160 && transform.position.x > -440)
                {
                    speed = 10;
                    rb.mass = 0.07f;
                    jumpAmount = 0.0001f;
                    isGrounded = true;
                } else
                {
                    speed = 7;
                    rb.mass = 0.075f;
                    jumpAmount = 0.025f;
                    isGrounded = false;
                }
                rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
            }

            if (Input.GetButton("Pause") && Time.timeScale == 1)
            {
                audioSync.GetComponent<AudioSource>().Pause();
                essaisUI.SetActive(false);
                pauseMenu.SetActive(true);
                rightController.SetActive(true);
                leftController.SetActive(true);
                Time.timeScale = 0;             
            }

            if (!isGrounded && transform.position.x >= -160 || !isGrounded && transform.position.x < -440)
            {
                transform.rotation = Quaternion.Euler(0, 0, rot);
                rot += rotOffset;
            }

            if (transform.position.x >= -160 || transform.position.x < -440)
            {
                SpaceShip.SetActive(false);
                if (isGrounded)
                {
                    particles.SetActive(true);
                } else
                {
                    particles.SetActive(false);
                }
            } else
            {
                SpaceShip.SetActive(true);
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

        private IEnumerator OnTriggerEnter(Collider other)
        {
            Death.SetActive(true);
            Death.GetComponent<ParticleSystem>().Play();
            pathCreator = null;
            var rend = this.gameObject.GetComponent<Renderer>();
            var collider = this.gameObject.GetComponent<BoxCollider>();
            var rigidbody = this.gameObject.GetComponent<Rigidbody>();
            rend.enabled = false;
            collider.enabled = false;
            rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            audioSync.SetActive(false);
            this.GetComponentInChildren<AudioSource>().Play(0);
            nbEssais++;
            SpaceShip.SetActive(false);
            yield return new WaitForSeconds(2);
            rend.enabled = true;
            collider.enabled = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            pathCreator = PathCreator.FindObjectOfType<PathCreator>();
            nb.text = nbEssais.ToString();
            distanceTravelled = 0.1f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(100, 0.75f, 0);
            audioSync.SetActive(true);
            //rb.GetComponent<PathFollower>().enabled = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void OnCollisionExit(Collision other) {
            if (transform.position.x < -160 && transform.position.x > -440)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}