using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFactory
{
    Transform parentTransfrom;
    GameObject lanternPrefab;

    public LanternFactory(GameObject prefab,Transform parentTran)
    {
        lanternPrefab = prefab;
        parentTransfrom = parentTran;
    }


    //进行加工
    public void CreateLantern()
    {
      new Lantern(lanternPrefab, parentTransfrom);
    }

}
