using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonController : MonoBehaviour
{

    public bool activated;
    Animator animator;
    float timer = -1;
    bool timed = false;
    AudioSource audio;
    public ParticleSystem smokevfx;

    public AudioClip audioSobe, audioDesce, audioHit;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && timed) {
            Activate();
        }

        animator.SetBool("Activated", activated);
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public void DeactivateWithTimer(float time)
    {
        timed = true;
        Deactivate();
        timer = time;
    }

    public void PlayDesce()
    {
        audio.PlayOneShot(audioDesce);
    }

    public void PlaySobe()
    {
        audio.PlayOneShot(audioSobe);
    }

    public void PlayHit()
    {
        audio.PlayOneShot(audioHit);
        smokevfx.Play();
    }
}
