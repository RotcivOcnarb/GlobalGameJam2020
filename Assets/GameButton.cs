using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameButton : MonoBehaviour
{

    public Animator animator;
    public UnityEvent evt;
    bool pressed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Box>() != null) {
            if (!pressed) {
                pressed = true;
                animator.SetBool("Pressed", true);
                evt.Invoke();
            }
        }
    }
}
