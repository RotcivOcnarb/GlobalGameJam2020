using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public AudioClip[] audios;
    AudioSource audioS;
    public SFXManager sfx;

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBox()
    {
        sfx.PlayAudio(audios[Random.Range(0, audios.Length)]);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
