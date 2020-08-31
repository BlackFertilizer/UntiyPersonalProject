using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternManager : MonoBehaviour
{
    LanternFactory lanternFactory;
    public GameObject LanternPrefab;


    public Texture[] heandTextures;
    public Texture[] endTextures;


    
    void Start()
    {
        lanternFactory = new LanternFactory(LanternPrefab,transform);
    }

    float deltaTime = 1f;
    void Update()
    {
        if(deltaTime < 0)
        {
            lanternFactory.CreateLantern();
            deltaTime = 1f;
        }
        
        deltaTime -= Time.deltaTime;
    }

}