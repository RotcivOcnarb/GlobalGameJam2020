using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EquipUIController : MonoBehaviour
{

    public Image iconImage;
    public InventoryManager inventory;
    
    [HideInInspector]
    public SlotUIController equip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iconImage.color = new Color(1, 1, 1, equip == null ? 0 : 1);
    }

    public void BeginDrag(){
        if(equip != null){
            if(inventory.itens[equip.index]){
                inventory.mouseSelection.sprite = equip.icon.sprite;
                inventory.mouseSelection.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void Drop(BaseEventData data){
        PointerEventData pointer = (PointerEventData) data;

        if(pointer.pointerDrag != null){
            SlotUIController controller = pointer.pointerDrag.GetComponent<SlotUIController>();
            if(controller == null){
                Debug.Log("SlotUI null");
                EquipUIController equipController = pointer.pointerDrag.GetComponent<EquipUIController>();

                if(equipController == null) return;
                else{
                    Debug.Log("Equip controller NOT null");
                    if(inventory.itens[equipController.equip.index]){
                        equip = equipController.equip;
                        equipController.equip = null;
                        iconImage.sprite = pointer.pointerDrag.transform.Find("Image").GetComponent<Image>().sprite;
                    }
                }
            }
            else{
                if(inventory.itens[controller.index] && !controller.equipped){
                    if(equip != null)
                        equip.equipped = false;
                    controller.equipped = true;
                    equip = controller;
                    iconImage.sprite = pointer.pointerDrag.transform.Find("Image").GetComponent<Image>().sprite;
                }
            }
        }
        inventory.mouseSelection.color = new Color(1, 1, 1, 0);
    }
}
