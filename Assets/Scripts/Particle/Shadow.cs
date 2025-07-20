using System;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, -0.5f, 0);
    
    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
