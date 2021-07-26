using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    void Update()
    {
       transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime); 
    }
}
