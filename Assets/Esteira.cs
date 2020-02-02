using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esteira : MonoBehaviour
{

    public Vector2 direction;
    public bool activated;
    Animator animator;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Activated", activated);
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (activated) {
            if (other.gameObject.GetComponent<Rigidbody2D>() != null) {
                Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D>();
                body.AddForce(direction);
            }
        }

        if(activated && !audio.isPlaying) {
            audio.Play();
        }
        else if(!activated && audio.isPlaying) {
            audio.Stop();
        }
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public void Revert()
    {
        direction *= -1;
    }
}
