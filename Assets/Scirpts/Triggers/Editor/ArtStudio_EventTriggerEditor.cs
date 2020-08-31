using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using UnityEditor;
using UnityEngine;
using Art.Studio;

namespace Art.StudioEditor
{
    [CustomEditor(typeof(ArtStudio_EventTrigger))]
    public class ArtStudio_EventTriggerEditor : Editor
    {

        [MenuItem("GameObject/XRStudio/SceneTriggle/EventTrigger", false, 11)]
        public static void AutoAddEventTriggleComponent()
        {
            GameObject aniTrigger = new GameObject("eventTrigger");
            Transform aniTriggerTran = aniTrigger.transform;
            if (Selection.activeGameObject != null)
            {
                aniTriggerTran.parent = Selection.activeGameObject.transform;
                aniTriggerTran.localRotation = Quaternion.identity;
                aniTriggerTran.localPosition = Vector3.zero;
                aniTriggerTran.localScale = Vector3.one;
            }

            if (aniTrigger.GetComponent<ArtStudio_EventTrigger>() == null)
            {
                aniTrigger.AddComponent<ArtStudio_EventTrigger>();
            }
        }

        [MenuItem("ArtStudio/EventTrigger", false, 1)]
        public static void AutoAddEventTriggle()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            if (activeGameObject.GetComponent<ArtStudio_EventTrigger>() == null)
            {
                activeGameObject.AddComponent<ArtStudio_EventTrigger>();
            }
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            EditorGUILayout.HelpBox("NeedBoxCollider", MessageType.Warning);
        }
    }
}
