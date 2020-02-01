using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    public GameObject toFollow;
    public Rect bounds;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos += (toFollow.transform.position - pos) / 10f;
        pos.z = -10;

        float cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraHeight = mainCamera.orthographicSize;

        if (pos.x < bounds.x - bounds.width/2f + cameraWidth) {
            pos.x = bounds.x - bounds.width / 2f + cameraWidth;
        }
        else if(pos.x > bounds.x + bounds.width/2f - cameraWidth) {
            pos.x = bounds.x + bounds.width / 2f - cameraWidth;
        }
        
        if(pos.y < bounds.y - bounds.height/2f + cameraHeight) {
            pos.y = bounds.y - bounds.height / 2f + cameraHeight;
        }
        else if(pos.y > bounds.y + bounds.height/2f - cameraHeight) {
            pos.y = bounds.y + bounds.height / 2f - cameraHeight;
        }

        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bounds.position + bounds.size * new Vector2(-0.5f, 0.5f), bounds.position + bounds.size *new Vector2(0.5f, 0.5f));
        Gizmos.DrawLine(bounds.position + bounds.size * new Vector2(0.5f, 0.5f), bounds.position + bounds.size * new Vector2(0.5f, -0.5f));
        Gizmos.DrawLine(bounds.position + bounds.size * new Vector2(0.5f, -0.5f), bounds.position + bounds.size * new Vector2(-0.5f, -0.5f));
        Gizmos.DrawLine(bounds.position + bounds.size * new Vector2(-0.5f, -0.5f), bounds.position + bounds.size * new Vector2(-0.5f, 0.5f));
    }
}
