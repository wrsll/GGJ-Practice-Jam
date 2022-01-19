using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    Vector3 boundryPos;

    private void Start()
    {
        boundryPos = transform.position;      
    }

    void LateUpdate()
    {
        Vector3 CameraPos = Camera.main.transform.position;
        transform.position = new Vector3(boundryPos.x, CameraPos.y - boundryPos.y, boundryPos.z);       
    }
}
