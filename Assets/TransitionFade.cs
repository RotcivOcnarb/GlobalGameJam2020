using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionFade : MonoBehaviour
{

    bool intro;
    bool outro;

    float alpha;

    CanvasGroup group;
    string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        intro = true;
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (intro) {
            alpha -= Time.deltaTime;
            if(alpha < 0) {
                alpha = 0;
                intro = false;
            }
        }

        else if (outro) {
            alpha += Time.deltaTime;
            if(alpha > 1) {
                alpha = 1;
                outro = false;
                SceneManager.LoadScene(nextScene);
            }
        }

        group.alpha = alpha;
    }

    public void ExitToScene(string scene)
    {
        outro = true;
        intro = false;
        nextScene = scene;
    }
}
