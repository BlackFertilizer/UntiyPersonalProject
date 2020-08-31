using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using UnityEditor;
using UnityEngine;
using Art.Studio;

namespace Art.StudioEditor
{
    [CustomEditor(typeof(ArtStudio_AnimatorTrigger))]
    public class ArtStudio_AnimatorTriggerEditor : Editor
    {
        private ArtStudio_AnimatorTrigger animatorTrigger;
        private List<EmitterPack> emitterTrans;
        private GUIStyle _fontStyle;

        [MenuItem("GameObject/XRStudio/SceneTriggle/AnimatorTriggle", false, 13)]
        public static void AutoAddAnimatorTriggleComponent()
        {
            GameObject aniTrigger = new GameObject("AnimatorTrigger");
            Transform aniTriggerTran = aniTrigger.transform;
            if (Selection.activeGameObject != null)
            {
                aniTriggerTran.parent = Selection.activeGameObject.transform;
                aniTriggerTran.localRotation = Quaternion.identity;
                aniTriggerTran.localPosition = Vector3.zero;
                aniTriggerTran.localScale = Vector3.one;
            }

            if (aniTrigger.GetComponent<ArtStudio_AnimatorTrigger>() == null)
            {
                aniTrigger.AddComponent<ArtStudio_AnimatorTrigger>();
            }
        }

        [MenuItem("ArtStudio/AnimatorTrigger", false, 4)]
        public static void AutoAddAnimatorTriggle()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            if (activeGameObject.GetComponent<ArtStudio_AnimatorTrigger>() == null)
            {
                activeGameObject.AddComponent<ArtStudio_AnimatorTrigger>();
            }
        }

        private void OnEnable()
        {
            _fontStyle = new GUIStyle();
            animatorTrigger = (ArtStudio_AnimatorTrigger)target;
            animatorTrigger.Init();
        }

        //bool showWeapons = true;

        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.HelpBox("NeedAddBoxCollider", MessageType.Warning);

            EditorGUI.BeginChangeCheck();
            animatorTrigger._animatorRes = EditorGUILayout.ObjectField("Animator",animatorTrigger._animatorRes, typeof(Animator), true) as Animator;
            if (EditorGUI.EndChangeCheck())
            {
                animatorTrigger.resetState();
            }

            animatorTrigger._stateIndex = EditorGUILayout.Popup("State Name",animatorTrigger._stateIndex, animatorTrigger._stateNameArr.ToArray());
            animatorTrigger._speed = EditorGUILayout.FloatField("Speed Scale ", animatorTrigger._speed);
            animatorTrigger._handPriority = EditorGUILayout.Toggle("Hand Priority",animatorTrigger._handPriority);
            
            //添加发射器
            EditorGUILayout.BeginVertical("Box");
            if(GUILayout.Button("Add Emitter"))
            {
                animatorTrigger.addEmptyEmmitter();
            }

            emitterTrans = animatorTrigger._emitterPacks;
            if(emitterTrans != null)
            {
                for(int i =0; i< emitterTrans.Count;i++)
                {
                     GUILayout.BeginHorizontal();
                        GUILayout.Label("Emmiter:");
                        emitterTrans[i].emitterTran = EditorGUILayout.ObjectField(emitterTrans[i].emitterTran, typeof(Transform), true, GUILayout.MinWidth(50)) as Transform;
                        GUILayout.Space(20);
                        GUILayout.Label("Range:");
                        emitterTrans[i].recRange = EditorGUILayout.FloatField(emitterTrans[i].recRange,GUILayout.MinWidth(30),GUILayout.MaxWidth(80));
                        GUILayout.Space(20);
                        if(GUILayout.Button("-",GUILayout.MaxWidth(60)))
                        {
                            animatorTrigger.removeEmitterPack(emitterTrans[i]);
                        }
                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();

            //检查配置的合法性
            EditorGUILayout.Space();
            //EditorGUILayout.ColorField  =
            GUILayout.BeginHorizontal();
             //_fontStyle.fontStyle = FontStyle.Bold;
            
            if (GUILayout.Button("Check !!!"))
            {
                animatorTrigger.checkResource();
            }

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Test Play"))
                {
                    animatorTrigger.checkResource();
                    animatorTrigger.ReceiveRay(default);
                }
                if (GUILayout.Button("Test Stop"))
                {
                    animatorTrigger.UnReceiveRay();
                }
            }

            GUILayout.EndVertical();

            if (GUI.changed)
            {

                EditorUtility.SetDirty(target);
            }
        }
    }
}
