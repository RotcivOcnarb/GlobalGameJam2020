using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePicker : MonoBehaviour
{

    public int index;
    public InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pick(){
        inventory.itens[index] = true;
        Destroy(gameObject);
        inventory.player.pickupvfx.Play();
    }
}
