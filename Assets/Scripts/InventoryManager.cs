using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool[] itens;
    public Image mouseSelection;
    public Camera mainCamera;

    public EquipUIController arm, leg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int OnArm(){
        if(arm.equip != null){
            return arm.equip.index;
        }
        return -1;
    }

    public int OnLegs(){
        if(leg.equip != null){
            return leg.equip.index;
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        mouseSelection.transform.position = pos;

        if(Input.GetMouseButtonUp(0)){
            mouseSelection.color = new Color(1, 1, 1, 0);
        }
    }
}
