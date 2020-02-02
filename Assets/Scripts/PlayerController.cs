using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D body;
    RaycastHit2D feetRay;
    public Animator equipmentUI;
    public InventoryManager inventory;
    public Animator playerAnimator;
    public GameObject interactionUI;
    bool equipOpen = false;

    public GameObject rightArm;

    Vector2 targetVelocity;
    public float dragSpeed;
    public float walkSpeed;
    public float wheelSpeed;
    int direction = 1;

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

        playerAnimator.SetBool("HasLegs", hasLegs());

        interactionUI.SetActive(interactions.Count > 0);

        rightArm.SetActive(hasArms());

        float speed = walkSpeed;
        if(!hasLegs()) speed = dragSpeed;
        if(inventory.OnLegs() == 0){
            speed = wheelSpeed;
        }

        targetVelocity.y = body.velocity.y;
        if (Input.GetKey(KeyCode.RightArrow) && !equipOpen) {
            targetVelocity.x = speed;
            playerAnimator.SetBool("Moving", true);
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !equipOpen) {
            targetVelocity.x = -speed;
            playerAnimator.SetBool("Moving", true);
            direction = -1;
        }
        else {
            targetVelocity.x = 0;
            playerAnimator.SetBool("Moving", false);
        }

        if(Input.GetKeyDown(KeyCode.I)){
            if(equipOpen){
                equipOpen = false;
                Time.timeScale = 1f;
                equipmentUI.SetBool("Open", false);
            }
            else{
                equipOpen = true;
                Time.timeScale = .1f;
                equipmentUI.SetBool("Open", true);
            }
        }

        if(Input.GetKeyDown(KeyCode.E)){
            if(interactions.Count > 0){
                interactions[0].Interact();
            }
        }

        Vector3 lcl = originalAnimationScale;
        lcl.x *= direction;
        playerAnimator.transform.localScale = lcl;

        Vector2 vel = body.velocity;
        vel += (targetVelocity - vel) / 5f;
        if(!equipOpen)
            body.AddForce((vel - body.velocity) * 100);
        else
            body.velocity = new Vector2(0, 0);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Interactable>() != null){
            interactions.Add(other.gameObject.GetComponent<Interactable>());
        }

        if(other.GetComponent<Gear>() != null){
            Gear gear = other.GetComponent<Gear>();
            if(inventory.OnArm() == 0){
                gear.Activate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Interactable>() != null){
            interactions.Remove(other.gameObject.GetComponent<Interactable>());
        }
    }

}
