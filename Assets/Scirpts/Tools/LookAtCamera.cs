using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
   Transform cameraTran;

    // Start is called before the first frame update
    void Start()
    {
        cameraTran = GameObject.Find("AR Camera").transform;
    }

    void Update()
    {
        if(cameraTran != null)
           transform.LookAt(cameraTran); 
    }
}
