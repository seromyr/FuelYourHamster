using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    
    void FixedUpdate()
    {
        Vector3 lookDirection = (transform.position - Camera.main.transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
    }
}
