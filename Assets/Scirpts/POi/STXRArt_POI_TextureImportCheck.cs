using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

namespace STXR.XRStudioArt.POIEditor
{
    public class STXRArt_POI_TextureImportCheck : AssetPostprocessor
    {
        private const string kTextureFormat = @".png|.jpg|.tga|.hdr";

        private const string kTextureNoFormat = @".hdr";
        private const string kAssetArtsPath = "Assets/Arts";

        private const int kTextureMaxSize = 1024;

        void OnPreprocessTexture()
        {
            //暂时不启用
            //startCheck();
        }

        private void startCheck()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            //在指定的目录下面的纹理进行校验
            if (assetPath.StartsWith(kAssetArtsPath))
            {
                // Debug.Log("is kAssetArtsPath");

                bool isValid = CheckTextureValid(importer);
                if (isValid)
                {
                    ChangeTexureFormat(importer);
                }
            }
            else
            {
                Debug.Log("not is kAssetArtsPath");
            }
        }


        //检查导入的纹理是够符合纹理格式要求，不符合直接删除
        private bool CheckTextureValid(TextureImporter importer)
        {
            if (importer != null)
            {
                //string textureProfile =string.Format("Name = {0},  type = {1},  Path = {2}",importer.name, importer.textureType.ToString(), assetPath);
                bool isValid = false;

                if (kTextureFormat.Split('|') == null)
                    return true;

              
                foreach (var formatTexture in kTextureFormat.Split('|'))
                {
                    if (assetPath.EndsWith(formatTexture))
                    {
                        isValid = true;
                        break;
                    }
                }

                if (!isValid)
                {
                    Debug.LogError(string.Format("InValid Texture Format ====>>>>   {0}", assetPath));
                    try
                    {
                        File.Delete(assetPath);
                        File.Delete(string.Format(assetPath + ".meta"));
                    }
                    catch (DirectoryNotFoundException dirNotFound)
                    {
                        Debug.LogError(dirNotFound);
                    }

                    return false;
                }
            }

            return true;
        }

        //进行资源格式转换
        private void ChangeTexureFormat(TextureImporter textureImporter)
        {
            //不设置图片的格式
            //textureImporter.textureType = TextureImporterType.Default;
            bool isPowOf2 = IsPowOf2(textureImporter);
            TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings("iPhone");
            TextureImporterFormat defaultAlpha = isPowOf2 ?TextureImporterFormat.PVRTC_RGBA4 : TextureImporterFormat.ASTC_RGBA_4x4;
            TextureImporterFormat defaultNotAlpha = isPowOf2? TextureImporterFormat.PVRTC_RGB4 :TextureImporterFormat.ASTC_RGB_6x6;

            settings.overridden = true;
            settings.maxTextureSize = kTextureMaxSize;
            settings.format = textureImporter.DoesSourceTextureHaveAlpha() ? defaultAlpha : defaultNotAlpha;
            textureImporter.SetPlatformTextureSettings(settings);

            settings = textureImporter.GetPlatformTextureSettings("Android");
            settings.overridden = true;
            bool isDivisibleOf4 = IsDivisibleOf4(textureImporter);
            //Android 平台ETC 必须满足被4整除，否则使用ASTC,不带alpha使用4x4 带Alpha 6x6
            defaultAlpha = isDivisibleOf4? TextureImporterFormat.ETC2_RGBA8 : TextureImporterFormat.ASTC_RGBA_4x4;
            defaultNotAlpha =isDivisibleOf4? TextureImporterFormat.ETC_RGB4 : TextureImporterFormat.ASTC_RGB_6x6;

            settings.maxTextureSize = kTextureMaxSize;
            settings.format = textureImporter.DoesSourceTextureHaveAlpha() ? defaultAlpha : defaultNotAlpha;
            textureImporter.SetPlatformTextureSettings(settings);

            if(!isPowOf2)
                Debug.LogWarning("Texture is not pow of 2");
            if(!isDivisibleOf4)
                Debug.LogWarning("Texture is not divisible of 4");
        }


        //判断是否为2的整数次幂
        private bool IsPowOf2(TextureImporter textureImporter)
        {
            (int width, int height) = GetTextureImporterSize(textureImporter);
            return (width == height) && (width > 0) && ((width & (width - 1)) == 0);
        }

        private bool IsDivisibleOf4(TextureImporter importer)
        {
            (int width, int height) = GetTextureImporterSize(importer);
            return (width % 4 == 0 && height % 4 == 0);
        }

        (int, int) GetTextureImporterSize(TextureImporter importer)
        {
            if (importer != null)
            {
                object[] args = new object[2];


                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                //Debug.Log("mi name = "+ mi.Name);

                // MethodInfo[] methods = typeof(TextureImporter).GetMethods(BindingFlags.NonPublic |BindingFlags.Instance);
              
                // foreach(var temp in methods)
                // {
                //     Debug.Log(string.Format("method name =  {0}, isPublic = {1},isVisual = {2}",temp.Name,temp.IsPublic,temp.IsVirtual));
                // }
              
                
                
                mi.Invoke(importer, args);
                return ((int)args[0], (int)args[1]);
            }
            return (0, 0);
        }
    }
}