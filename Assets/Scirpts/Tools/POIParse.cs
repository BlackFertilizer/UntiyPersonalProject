using System;
using UnityEngine;

[Serializable]
public class PoiInfo
{
    public Poi[] poiList;
}

[Serializable]
public class Poi
{
    public string poiId;
    public string site1;
    public string site2;
    public string class1;
    public string class2;
    public float posX;
    public float posY;
    public float posZ;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
    public float rotateX;
    public float rotateY;
    public float rotateZ;
    public string defaultResource;

}

public static class PoiParseTool
{
    const string kJonsName = @"POI_DOT_INFOMATION";

    public static PoiInfo GetPoiObject()
    {
        Debug.Log(Application.dataPath);
        TextAsset textAsset = (TextAsset)Resources.Load(kJonsName);

        return ParseJson(textAsset.text);
    }

    private static PoiInfo ParseJson(string poiJson)
    {
        PoiInfo poiInfo = JsonUtility.FromJson<PoiInfo>(poiJson);
        return poiInfo;
    }
}


