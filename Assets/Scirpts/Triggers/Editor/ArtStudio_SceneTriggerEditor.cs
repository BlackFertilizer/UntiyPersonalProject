using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using MP = EditorPathDefine;
namespace Art.StudioEditor
{


    public class ArtSrudio_SceneTriggerEditor
    {
        //public static void init()
        //{
        //    AudioWindow window = (AudioWindow)EditorWindow.GetWindowWithRect(typeof(AudioWindow), new Rect(new Vector2(400, 300), new Vector2(800, 700)), true);
        //    window.Show();
        //}


        //void OnEnable()
        //{
        //    originObjList = new List<GameObject>();
        //    gainPrefabsHoldAudioSource();
        //}


        //void OnGUI()
        //{
        //    GUILayout.BeginVertical(GUILayout.Height(100));
        //    EditorGUILayout.HelpBox("为目标物体绑定声音资源，实现点击、距离感应触发，播放音频", MessageType.Info);
        //    sizeMultiplier = EditorGUILayout.IntField("目标数量", sizeMultiplier, GUILayout.Width(200));

        //    initObjectList(sizeMultiplier);

        //    //显示已经初始化的
        //    for (int i = 0; i < sizeMultiplier; i++)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        originObjList[i] = EditorGUILayout.ObjectField(string.Format("目标物体_{0}", i), originObjList[i], typeof(GameObject), true) as GameObject;

        //        EditorGUILayout.Space();

        //        bindAudioSource(originObjList[i], i);

        //        EditorGUILayout.EndHorizontal();
        //    }

        //    if (GUILayout.Button("Show Log"))
        //    {
        //        foreach (var tempObj in originObjList)
        //        {
        //            if (tempObj != null)
        //            {
        //                Debug.Log(tempObj.name);
        //            }

        //            else
        //            {
        //                Debug.Log("originObjList null");
        //            }
        //        }

        //        Debug.Log("sizeMultiplier " + sizeMultiplier);
        //    }

        //    GUILayout.EndVertical();
        //}


        ////初始化Object
        //private void initObjectList(int inUseNum)
        //{
        //    int count = 0;
        //    if (originObjList != null)
        //    {
        //        count = originObjList.Count;
        //    }

        //    if (inUseNum > count)
        //    {
        //        for (int i = count; i < inUseNum; i++)
        //        {
        //            //Debug.Log("add = " + i);
        //            originObjList.Add(null);
        //        }
        //    }
        //    else if (inUseNum < count)
        //    {
        //        for (int i = count - 1; i > inUseNum - 1; i--)
        //        {
        //            //Debug.Log("remove = " + i);
        //            originObjList.RemoveAt(i);
        //        }
        //    }
        //}


        ////获取并显示指定路径下的物体，其包含AudioSource资
        //private void bindAudioSource(GameObject audioObject, int index)
        //{
        //    if (audioObject == null)
        //    {
        //        //Debug.Log("current empty , index = "+ index);
        //        return;
        //    }

        //    AudioClip audioClip = null;
        //    AudioSource audioSource = audioObject.GetComponent<AudioSource>();

        //    if (audioSource != null)
        //        audioClip = audioSource.clip;

        //    audioClip = EditorGUILayout.ObjectField(string.Format("声音资源_{0}", index), audioClip, typeof(AudioClip), true) as AudioClip;

        //    //如果audioClip 不为空，则添加AudioSource 添加clip
        //    if (audioClip != null)
        //    {
        //        if (audioSource == null){
        //            audioSource = audioObject.AddComponent<AudioSource>();
        //        }

        //        audioSource.clip = audioClip;
        //        audioSource.playOnAwake =false;
        //    }
        //    else if (audioClip == null)
        //    {
        //        if (audioSource != null)
        //        {
        //            DestroyImmediate(audioSource, true);
        //        }
        //    }

        //}



        ////获取指定路径下面的所有带有AudioSource 组件的预制体
        //private void gainPrefabsHoldAudioSource()
        //{
        //    //string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "prefab");

        //    var audioFiles = MP.GetFiles(MP.kAudioObjectResourcePath, MP.kAudioObjectFilter, SearchOption.AllDirectories);
        //    for (int idx = 0; idx < audioFiles.Length; idx++)
        //    {
        //        audioFiles[idx] = audioFiles[idx].Replace('\\', '/');
        //    }

        //    for (int i = 0; i < audioFiles.Length; i++)
        //    {
        //        GameObject go = AssetDatabase.LoadAssetAtPath(audioFiles[i], typeof(System.Object)) as GameObject;
        //        //Debug.Log("go name = " + go.name);
        //        if (go != null)
        //        {
        //            //var thetype = scriptObj.GetClass();
        //            Component[] co = go.GetComponentsInChildren(typeof(AudioSource), true);
        //            foreach (var _child in co)
        //            {
        //                Debug.Log("<color=darkblue>Find it: " + go.name + "--->>" + _child.name + "</color>");
        //                originObjList.Add(_child.gameObject);
        //                sizeMultiplier++;
        //            }
        //        }
        //    }

        //}

    }
}
