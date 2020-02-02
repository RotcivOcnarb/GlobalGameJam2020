using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonController : MonoBehaviour
{

    public bool activated;
    Animator animator;
    float timer = -1;
    bool timed = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
}
