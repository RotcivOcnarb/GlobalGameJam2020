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
    public PlayerController player;

    [Header("Audio")]
    public AudioClip[] openInventory;
    public AudioClip[] closeInventory;
    public AudioClip[] equipSfx;
    public AudioClip[] dragSFX;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
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

    public void OpenSFX()
    {
        audio.clip = openInventory[Random.Range(0, openInventory.Length)];
        audio.Play();
    }

    public void CloseSFX()
    {
        audio.clip = closeInventory[Random.Range(0, openInventory.Length)];
        audio.Play();
    }

    public void EquipSFX()
    {
        audio.clip = equipSfx[Random.Range(0, equipSfx.Length)];
        audio.Play();
    }

    public void DragSFX()
    {
        audio.clip = dragSFX[Random.Range(0, dragSFX.Length)];
        audio.Play();
    }
}
