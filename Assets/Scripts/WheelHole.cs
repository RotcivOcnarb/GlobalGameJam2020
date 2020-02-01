using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelHole : MonoBehaviour
{

    public PlayerController player;
    BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerController>() != null){
            if(player.inventory.OnLegs() != 0 || Mathf.Abs(player.body.velocity.x) < player.wheelSpeed *0.9f){
                col.isTrigger = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>() != null){
            if(player.inventory.OnLegs() == 0 && Mathf.Abs(player.body.velocity.x) > player.wheelSpeed *0.9f){
                col.isTrigger = false;
            }
        }
    }
}
