using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esteira : MonoBehaviour
{

    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerController>() != null){
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.body.AddForce(direction);
        }
    }
}
