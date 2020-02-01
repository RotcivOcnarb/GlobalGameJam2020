using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUIController : MonoBehaviour
{
    public Image icon;
    public InventoryManager inventory;
    public int index;
    public bool equipped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        icon.color = new Color(1, 1, 1, inventory.itens[index] ? ( equipped ? 0.5f : 1f ) : 0);
    }

    public void BeginDrag(){
        if(inventory.itens[index] && !equipped){
            inventory.mouseSelection.sprite = icon.sprite;
            inventory.mouseSelection.color = new Color(1, 1, 1, 1);
        }
    }

    public void Drop(BaseEventData data){
        PointerEventData pointer = (PointerEventData) data;

        if(pointer.pointerDrag != null){
            EquipUIController controller = pointer.pointerDrag.GetComponent<EquipUIController>();
            if(controller == null) return;

            if(controller.equip == this){
                equipped = false;
                controller.equip = null;
            }
        }
    }

}
