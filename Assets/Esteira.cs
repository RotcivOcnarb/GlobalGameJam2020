using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esteira : MonoBehaviour
{

    public Vector2 direction;
    public bool activated;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (activated) {
            if (other.gameObject.GetComponent<Rigidbody2D>() != null) {
                Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D>();
                body.AddForce(direction);
            }
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
}
