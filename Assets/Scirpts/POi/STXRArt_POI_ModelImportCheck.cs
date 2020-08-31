using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace STXR.XRStudioArt.POIEditor
{
    public class STXRArt_POI_ModelImportCheck : AssetPostprocessor
    {
         private const string kAssetArtsPath = "Assets/Arts";

        void OnPostprocessModel(GameObject g)
        {
            //在指定的目录下面的纹理进行校验
            if (assetPath.StartsWith(kAssetArtsPath))
            {
                MeshFilter meshFilter = g.GetComponent<MeshFilter>();
                string meshName = meshFilter.sharedMesh.name;

                char[] nameChars = meshName.ToCharArray();
                bool isInValid = false;
                if (nameChars == null || nameChars.Length == 0)
                {
                    isInValid = true;
                }
                else
                {
                    foreach (var idxName in nameChars)
                    {
                        if (idxName < 48 || idxName > 137)
                        {
                            isInValid = true;
                        }
                    }
                }

                if (isInValid)
                {
                    Debug.LogError(string.Format("Fail Import !!!\n  inValidName ={0},   AssetPath = {1}",meshName, assetPath));
                    File.Delete(assetPath);
                    File.Delete(assetPath + ".meta");
                }

                //模型默认不带入material
                // ModelImporter modelImporter = assetImporter as ModelImporter;
                // modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            }
        }
    }
}