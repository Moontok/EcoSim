using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target = null;    

    void LateUpdate()
    {        
        this.transform.position = target.transform.position;
    }
}
