using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using Art.Studio;

namespace Art.StudioEditor
{
    [CustomEditor(typeof(ArtStudio_EffectTrigger))]
    public class ArtStudio_EffectTriggerEditor : Editor
    {
        private ArtStudio_EffectTrigger effectTrigger;
        private List<GameObject> gameobjectList;

        [MenuItem("GameObject/ArtStudio/SceneTriggle/EffectTrigger", false, 15)]
        public static void AutoAddEffectTriggleComponent()
        {
            GameObject aniTrigger = new GameObject("EffectTrigger");
            Transform aniTriggerTran = aniTrigger.transform;
            if (Selection.activeGameObject != null)
            {
                aniTriggerTran.parent = Selection.activeGameObject.transform;
                aniTriggerTran.localRotation = Quaternion.identity;
                aniTriggerTran.localPosition = Vector3.zero;
                aniTriggerTran.localScale = Vector3.one;
            }

            if (aniTrigger.GetComponent<ArtStudio_EffectTrigger>() == null)
            {
                aniTrigger.AddComponent<ArtStudio_EffectTrigger>();
            }
        }

        [MenuItem("ArtStudio/EffectTrigger", false, 4)]
        public static void AutoAddEffectTriggle()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            if (activeGameObject.GetComponent<ArtStudio_EffectTrigger>() == null)
            {
                activeGameObject.AddComponent<ArtStudio_EffectTrigger>();
            }
        }

        private void OnEnable()
        {
            effectTrigger = (ArtStudio_EffectTrigger)target;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            EditorGUILayout.HelpBox("NeedAddBoxCollider", MessageType.Warning);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

              //添加发射器
            EditorGUILayout.BeginVertical("Box");
            if(GUILayout.Button("Add GameObject"))
            {
                effectTrigger.addEmptyEffect();
            }

            gameobjectList = effectTrigger._gameObjectList;
            if(gameobjectList != null)
            {
                for(int i =0; i< gameobjectList.Count;i++)
                {
                     GUILayout.BeginHorizontal();
                        GUILayout.Label("Effect GameObject: ",GUILayout.MinWidth(40));
                        gameobjectList[i] = EditorGUILayout.ObjectField(gameobjectList[i],typeof(GameObject),true) as GameObject;
                        GUILayout.Space(20);
                        if(GUILayout.Button("-",GUILayout.MaxWidth(40)))
                        {
                            effectTrigger.removeEffect(gameobjectList[i]);
                        }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

        }
    }
}
