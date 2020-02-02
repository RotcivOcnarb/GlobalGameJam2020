using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gear : MonoBehaviour
{

    Animator animator;
    public UnityEvent evt;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(){

        if (player.inventory.OnArm() == 0) {
            animator.SetTrigger("Activate");
            evt.Invoke();
        }
    }


}
