using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeChanger : MonoBehaviour
{

    public ObjectFollow cam;
    public float size;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null) {
            cam.targetSize = size;
        }
    }
}
