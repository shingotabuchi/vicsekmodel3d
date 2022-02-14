using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBoid : MonoBehaviour
{
    public Transform parent;
    public Vector3 offset;
    void Update()
    {
        try
        {
            transform.position = parent.position + offset;
            transform.rotation = parent.rotation;
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}
