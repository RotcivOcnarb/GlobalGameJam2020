using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D body;
    public Animator equipmentUI;
    public InventoryManager inventory;
    public Animator playerAnimator;
    public GameObject interactionUI;
    public GameObject animationRoot;
    bool equipOpen = false;
    AudioSource audio;

    [Header("Audios")]
    public SFXManager sfx;
    public AudioClip arrastando;
    public AudioClip wheel_walk;
    public AudioClip[] legWalks;
    public AudioClip[] jumpsAudio;
    public AudioClip propJump;
    public AudioClip propFront;
    public AudioClip fire;

    [Header("VFX")]

    public ParticleSystem feetBurst;
    public ParticleSystem handBurst;
    public ParticleSystem pickupvfx;
    public ParticleSystem flameFeet;
    public ParticleSystem flameHand;

    [Header("Resto")]
    public GameObject rightArm;

    public float feetPosition;
    RaycastHit2D feetRay;
    public float jumpForce;
    bool justJumped = false;
    bool doubleJump = false;
    public float propulsionPushForce;

    Vector2 targetVelocity;
    public float dragSpeed;
    public float walkSpeed;
    public float jumpHeight;
    public float hiJumpHeight;
    public float wheelSpeed;
    int direction = 1;
    bool falling;
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
        audio = GetComponent<AudioSource>();
    }

    public bool hasLegs(){
        return inventory.OnLegs() != -1;
    }

    public bool hasArms(){
        return inventory.OnArm() != -1;
    }

    public bool OnGround()
    {
        return feetRay.distance < 0.01f;
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
            playerAnimator.SetInteger("Arm", inventory.OnArm());
            playerAnimator.SetInteger("Leg", inventory.OnLegs());

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


                if (OnGround()) {
                    audio.loop = true;
                    if (!hasLegs())
                        audio.clip = arrastando;
                    else if (inventory.OnLegs() == 0)
                        audio.clip = wheel_walk;
                    else {
                        audio.loop = false;
                        if (!audio.isPlaying) {
                            audio.clip = legWalks[Random.Range(0, legWalks.Length)];
                        }
                    }

                    if (!audio.isPlaying) {
                        audio.Play();
                    }
                }
            }
            else if (Input.GetKey(KeyCode.A) && !equipOpen) {
                targetVelocity.x = -speed;
                playerAnimator.SetBool("Moving", true);
                direction = -1;

                if (OnGround()) {
                    audio.loop = true;
                    if (!hasLegs())
                        audio.clip = arrastando;
                    else if (inventory.OnLegs() == 0)
                        audio.clip = wheel_walk;
                    else {
                        audio.loop = false;
                        if (!audio.isPlaying) {
                            audio.clip = legWalks[Random.Range(0, legWalks.Length)];
                        }
                    }

                    if (!audio.isPlaying) {
                        audio.Play();
                    }
                }
            }
            else {
                audio.Stop();
                targetVelocity.x = 0;
                playerAnimator.SetBool("Moving", false);
            }

            if (Input.GetKeyDown(KeyCode.I)) {
                if (equipOpen) {
                    equipOpen = false;
                    Time.timeScale = 1f;
                    equipmentUI.SetBool("Open", false);
                    inventory.OpenSFX();
                }
                else {
                    equipOpen = true;
                    Time.timeScale = .1f;
                    equipmentUI.SetBool("Open", true);
                    inventory.CloseSFX();
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                if (interactions.Count > 0) {
                    interactions[0].Interact();
                    playerAnimator.SetTrigger("HandSkill");
                }
                else if(inventory.OnArm() == 1) {
                    //Propulsiona o item a frente
                    RaycastHit2D rc = Physics2D.Raycast(transform.position + new Vector3(3.9f * 0.1192f * direction, 0, 0), new Vector2(direction, 0));
                    Debug.Log(rc.collider.name + " - " + rc.distance);
                    
                    if (rc.collider.GetComponent<Box>() != null) {
                        if(rc.distance < 0.01f) {
                            rc.collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * propulsionPushForce * 20, 0), ForceMode2D.Impulse);
                        }
                    }
                    sfx.PlayAudio(propFront);
                }
                else if(inventory.OnArm() == 2) {
                    RaycastHit2D rc = Physics2D.Raycast(transform.position + new Vector3(3.9f * 0.1192f * direction, 0, 0), new Vector2(direction, 0));
                    Debug.Log(rc.collider.name + " - " + rc.distance);

                    if (rc.collider.GetComponent<Box>() != null) {
                        if (rc.distance < 0.01f) {
                            rc.collider.GetComponent<Box>().DestroyBox();
                            rc.collider.GetComponent<Animator>().SetTrigger("Destroy");
                        }
                    }
                    sfx.PlayAudio(fire);
                }
            }

            feetRay = Physics2D.Raycast(transform.position + new Vector3(0, feetPosition), new Vector2(0, -1));
            if (Input.GetKeyDown(KeyCode.W)) {
                //pulo normal
                if(inventory.OnLegs() == 1 || inventory.OnLegs() == 2) {
                    if (OnGround()) {
                        body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        justJumped = true;

                        audio.loop = false;
                        audio.clip = jumpsAudio[Random.Range(0, jumpsAudio.Length)];
                        audio.time = 0;
                        sfx.PlayAudio(audio.clip);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.W)) {
                justJumped = false;
                audio.Stop();
            }

            if (OnGround()) {
                doubleJump = false;
            }

            flameFeet.Stop();
            if (Input.GetKey(KeyCode.W)) { //plana
                if(!justJumped && !OnGround() && inventory.OnLegs() == 2) {
                    body.AddForce((new Vector2(body.velocity.x, -.1f) - body.velocity ) * 10);
                    //if (!flameFeet.isPlaying) {
                    //    flameFeet.Play();
                   // }
                    audio.loop = true;
                    audio.clip = fire;
                    if (!audio.isPlaying) {
                        audio.Play();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                if(!justJumped && !OnGround() && inventory.OnLegs() == 1 && !doubleJump) {
                    //Pulo duplo
                    body.AddForce(new Vector2(0, propulsionPushForce * 5), ForceMode2D.Impulse);
                    doubleJump = true;
                    sfx.PlayAudio(propJump);
                    feetBurst.Play();
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

            StartCoroutine(ResetScene());
        }
    }

    IEnumerator ResetScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }

    public void Iterate(Transform root, Action<Transform> action)
    {
        action(root);
        foreach (Transform t in root) {
            Iterate(t, action);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(-1, feetPosition), transform.position + new Vector3(1, feetPosition));
    }



}
