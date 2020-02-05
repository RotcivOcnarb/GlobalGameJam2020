using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionWrench : MonoBehaviour
{

    public GameObject toFollow;

    public Vector3 offset;

    AudioSource audio;

    public AudioClip[] hovers;
    public AudioClip[] confirms;

    public Image logoLight;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 position = transform.position;
        position += ((toFollow.transform.position + offset) - position) / 5f;
        transform.position = position;

        logoLight.color = new Color(1, 1, 1, Random.Range(0.5f, 1f));

    }

    public void Hover(GameObject button)
    {
        if (!clicked) {
            toFollow = button;

            audio.clip = hovers[Random.Range(0, hovers.Length)];
            audio.Play();
        }
    }
    bool clicked = false;
    public void Click()
    {
        if (!clicked) {
            clicked = true;
            audio.clip = confirms[Random.Range(0, confirms.Length)];
            audio.Play();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
