using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D body;
    RaycastHit2D feetRay;
    public Animator equipmentUI;
    public InventoryManager inventory;
    public Animator playerAnimator;
    public GameObject interactionUI;
    public GameObject animationRoot;
    bool equipOpen = false;

    public GameObject rightArm;

    Vector2 targetVelocity;
    public float dragSpeed;
    public float walkSpeed;
    public float jumpHeight;
    public float hiJumpHeight;
    public float wheelSpeed;
    int direction = 1;
    bool jumping;
    bool hiJumping;
    bool dead;

    List<Interactable> interactions;
    Vector3 originalAnimationScale;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        targetVelocity = new Vector2(0, 0);
        interactions = new List<Interactable>();
        originalAnimationScale = playerAnimator.transform.localScale;
    }

    public bool hasLegs(){
        return inventory.OnLegs() != -1;
    }

    public bool hasArms(){
        return inventory.OnArm() != -1;
    }

    // Update is called once per frame
    void Update()
    {

        /*
            0 - Roda
            1 - Propulsor
            2 - Maçarico
        */

        if (!dead) {
            playerAnimator.SetBool("HasLegs", hasLegs());
            interactionUI.SetActive(interactions.Count > 0);
            rightArm.SetActive(hasArms());
            

        
            float speed = walkSpeed;
            float jump = jumpHeight;
            if (!hasLegs()) speed = dragSpeed;
            if (inventory.OnLegs() == 0) {
                speed = wheelSpeed;
            }


            targetVelocity.y = body.velocity.y;
            if (Input.GetKey(KeyCode.D) && !equipOpen) {
                targetVelocity.x = speed;
                playerAnimator.SetBool("Moving", true);
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.A) && !equipOpen) {
                targetVelocity.x = -speed;
                playerAnimator.SetBool("Moving", true);
                direction = -1;
            }
            else {
                targetVelocity.x = 0;
                playerAnimator.SetBool("Moving", false);
            }

            if (inventory.OnLegs() == 1 || inventory.OnLegs() == 2) {
                if (Input.GetKeyDown(KeyCode.W) && !equipOpen && !jumping) {
                    targetVelocity.y = jump;
                    jumping = true;
                }
                if (inventory.OnLegs() == 1 && Input.GetKeyDown(KeyCode.W) && !equipOpen && jumping && !hiJumping && targetVelocity.y < 0) {
                    targetVelocity.y = hiJumpHeight;
                    hiJumping = true;
                }
                if(jumping && targetVelocity.y == 0){
                    jumping = false;
                    hiJumping = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.I)) {
                if (equipOpen) {
                    equipOpen = false;
                    Time.timeScale = 1f;
                    equipmentUI.SetBool("Open", false);
                }
                else {
                    equipOpen = true;
                    Time.timeScale = .1f;
                    equipmentUI.SetBool("Open", true);
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                if (interactions.Count > 0) {
                    interactions[0].Interact();
                }
            }

            Vector3 lcl = originalAnimationScale;
            lcl.x *= direction;
            playerAnimator.transform.localScale = lcl;

            Vector2 vel = body.velocity;
            vel += (targetVelocity - vel) / 5f;
            if (!equipOpen)
                body.AddForce((vel - body.velocity) * 100);
            else
                body.velocity = new Vector2(0, 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Interactable>() != null){
            interactions.Add(other.gameObject.GetComponent<Interactable>());
        }

        if (other.GetComponentInParent<PistonController>() != null) {
            PistonController piston = other.GetComponentInParent<PistonController>();
            if (piston.activated) {
                Kill();
            }
        }
    }



    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Interactable>() != null){
            interactions.Remove(other.gameObject.GetComponent<Interactable>());
        }
    }

    public void Kill()
    {
        if (!dead) {
            dead = true;
            Destroy(animationRoot.GetComponent<Animator>());
            Iterate(animationRoot.transform, t => {

                CircleCollider2D collider = t.gameObject.AddComponent<CircleCollider2D>();
                collider.radius = 1f;
                Rigidbody2D pieceBody = t.gameObject.AddComponent<Rigidbody2D>();
                pieceBody.AddForce(UnityEngine.Random.insideUnitCircle * 100);

            });
        }
    }

    public void Iterate(Transform root, Action<Transform> action)
    {
        action(root);
        foreach (Transform t in root) {
            Iterate(t, action);
        }
    }

    

}
