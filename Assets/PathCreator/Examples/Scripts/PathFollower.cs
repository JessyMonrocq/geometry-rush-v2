using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float rotateSpeed = 20.0f;
        float distanceTravelled;
        public bool isGrounded;

        public Rigidbody rb;

        public float gravityScale = 5;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.position = new Vector3( pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction).x, this.transform.position.y,pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction).z);
                
                //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
            
            if (Input.GetButton("Jump") && isGrounded) {
                isGrounded = false;
                rb.AddForce(Vector3.up, ForceMode.Impulse);
            }
        }

        void FixedUpdate() {
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

            if (!isGrounded)
            {
                transform.Rotate(0,0,rotateSpeed*Time.deltaTime);
            } else {
                transform.Rotate(0,0,0);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        void OnCollisionEnter(Collision other) {
            if (other.gameObject.name == "Spike")
            {
                Debug.Log ("Touché");
                var rend = rb.GetComponent<Renderer>();
                rend.enabled = false;
                //rb.GetComponent<PathFollower>().enabled = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } else {
                isGrounded = true;
            }
        }
        
        void OnCollisionExit(Collision other) {
            isGrounded = false;    
        }
    }
}