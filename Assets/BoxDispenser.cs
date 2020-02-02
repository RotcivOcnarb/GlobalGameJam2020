using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDispenser : MonoBehaviour
{

    public GameObject boxPrefab;
    public GameObject spawnPoint;
    public GameObject spawnedBox;
    Animator animator;
    public SFXManager sfx;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedBox == null) {
            spawnedBox = Instantiate(boxPrefab, spawnPoint.transform.position, boxPrefab.transform.rotation);
            spawnedBox.GetComponent<Box>().sfx = sfx;
            animator.SetTrigger("Dispense");
        }
    }
}
