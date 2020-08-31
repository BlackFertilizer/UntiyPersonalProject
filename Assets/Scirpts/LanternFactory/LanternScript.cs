using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    Vector3 speed;
    //初始化数据
    public void init()
    {
        speed = new Vector3(0, 0.05f, 0);
    }

    float endTime= 4f;
    float movetime = 0;

    void Update()
    {
        transform.position += speed;
        movetime += Time.deltaTime;
        if (movetime >= endTime)
        {
           Destroy(this.gameObject);
        }
    }
}
