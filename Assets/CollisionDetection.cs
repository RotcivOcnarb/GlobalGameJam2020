using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collision2D>
    {
    }

    public ColliderEvent evt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        evt.Invoke(collision);
    }
}
