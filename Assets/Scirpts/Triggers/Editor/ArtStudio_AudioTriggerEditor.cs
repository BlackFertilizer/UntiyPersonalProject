using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Art.Studio;

namespace Art.StudioEditor
{
    [CustomEditor(typeof(ArtStudio_AudioTrigger))]
    public class ArtStudio_AudioTriggerEditor : Editor
    {
        [MenuItem("GameObject/XRStudio/SceneTriggle/AudioTriggle", false, 12)]
        public static void AutoAddAudioTriggleComponent()
        {
           
            GameObject vs = new GameObject("AudioTrigger");
            if(Selection.activeGameObject!=null)
            {
                vs.transform.parent = Selection.activeTransform;
                vs.transform.localRotation = Quaternion.identity;
                vs.transform.localPosition = Vector3.zero;
                vs.transform.localScale = Vector3.one;
            }
            if (vs.GetComponent<ArtStudio_AudioTrigger>() == null)
            {
                vs.AddComponent<ArtStudio_AudioTrigger>();
            }
        }

    
        [MenuItem("ArtStudio/AudioTrigger", false, 3)]
        public static void AutoAddAudioTriggle()
        {
            GameObject vs = Selection.activeGameObject;
            if (vs.GetComponent<ArtStudio_AudioTrigger>() == null)
            {
                vs.AddComponent<ArtStudio_AudioTrigger>();
            }
        }

        private ArtStudio_AudioTrigger _audio;
        private void OnEnable() 
        {
            _audio = target as ArtStudio_AudioTrigger;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.BeginHorizontal();
            if(EditorApplication.isPlaying)
            {
            if (GUILayout.Button("Test Play"))
            {
                if(_audio.AudioClipRes != null)
                {
                    _audio.ReceiveRay(new Ray());
                }
            }
            if (GUILayout.Button("Test Stop"))
            {
                if(_audio.AudioClipRes != null)
                {
                    _audio.UnReceiveRay();
                }    
            }
           
            }
            GUILayout.EndVertical();
        }
    }

}