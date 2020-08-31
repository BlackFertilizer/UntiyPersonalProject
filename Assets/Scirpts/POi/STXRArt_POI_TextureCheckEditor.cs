using System.Security.Cryptography;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace STXR.XRStudioArt.POIEditor
{
    public class STXRArt_POI_TextureCheckEditor : EditorWindow
    {
        public string kAssetArtsPath = "Assets/Arts";
        public string kAudioObjectFilter = @"*.prefab";
        public const string kTextureFormat = @".png|.jpeg|.tga|.meta";
        public const string kMeta = @"meta";
        public const string kPng = @"png";
        public const string kJpeg = @"jpeg";
        public const string kTga = @"tga";


        public const string kTextureFolder = @"Textures";
        // public static string kAudioTypeFilter = @"*.mp3|*.wav";

        List<string> texturePathList;

        private string[] GetAllFiles(string sourceFolder)
        {
            return System.IO.Directory.GetFiles(sourceFolder).ToArray();
        }

        public string[] GetFiles(string sourceFolder, string filters, System.IO.SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }




        //[MenuItem(STXRArt_XRStudio_POIEditorLanguage.Texture_Format_Check, false, 0)]
        public static void AutoTextureCheck()
        {
            STXRArt_POI_TextureCheckEditor window = (STXRArt_POI_TextureCheckEditor)EditorWindow.GetWindowWithRect(typeof(STXRArt_POI_TextureCheckEditor), new Rect(new Vector2(400, 300), new Vector2(800, 700)), true);
            window.Show();
        }


        void OnEnable()
        {
            texturePathList = new List<string>();
            gainTexturePath(kAssetArtsPath);
        }


        void OnGUI()
        {
            if (GUILayout.Button("GainTexture"))
            {
                gainTextureSource();
            }
        }

        private void gainTextureSource()
        {
            // string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "prefab");

            if (texturePathList != null)
                foreach (var path in texturePathList)
                {
                    var textureFiles = GetAllFiles(path);
                    //var textureFiles = GetFiles(path,kAudioObjectFilter, SearchOption.AllDirectories);
                    for (int idx = 0; idx < textureFiles.Length; idx++)
                    {
                        textureFiles[idx] = textureFiles[idx].Replace('\\', '/');

                        bool isValid = false;
                        
                        if(kTextureFormat.Split('|')==null)
                            return;

                        foreach (var formatTexture in kTextureFormat.Split('|'))
                        {
                            if (textureFiles[idx].EndsWith(formatTexture))
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (!isValid)
                        {
                            Debug.LogError(string.Format("Error TextureFormat ====>>>>   {0}", textureFiles[idx]));
                        }
                    }

                    for (int i = 0; i < textureFiles.Length; i++)
                    {
                        GameObject go = AssetDatabase.LoadAssetAtPath(textureFiles[i], typeof(System.Object)) as GameObject;

                        if (go != null)
                        {
                            Component[] co = go.GetComponentsInChildren(typeof(Texture), true);
                            foreach (var _child in co)
                            {
                                Debug.Log("<color=darkblue>Find it: " + go.name + "--->>" + _child.name + "</color>");
                                //originObjList.Add(_child.gameObject);
                                //sizeMultiplier++;
                            }
                        }
                    }
                }


        }

        private void gainTexturePath(string path)
        {

            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                if (dirs == null || dirs.Count() == 0)
                {
                    return;
                }

                foreach (var dir in dirs)
                {
                    if (dir.EndsWith(kTextureFolder))
                    {
                        //Debug.Log(dir);
                        texturePathList.Add(dir);

                    }
                    gainTexturePath(dir);
                }
            }
        }

    }


}
