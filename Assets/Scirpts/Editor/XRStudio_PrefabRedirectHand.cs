using UnityEngine;
using UnityEditor;
namespace STXR.XRStudio
{
    public class XRStudio_PrefabRedirectHand : EditorWindow
    {

        //[MenuItem("PrefabManager/Create Prefab")]
        //static void CreatePrefab()
        //{

        //    GameObject[] objectArray = Selection.gameObjects;

        //    foreach (GameObject gameObject in objectArray)
        //    {

        //        string localPath = "Assets/" + gameObject.name + ".prefab";

        //        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        //        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        //    }
        //}


        //// Disable the menu item if no selection is in place.
        //[MenuItem("Examples/Create Prefab", true)]
        //static bool ValidateCreatePrefab()
        //{
        //    return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
        //}

        [MenuItem("GameObject/PrefabManager/PrefabRedirectHand", false, 1)]
        static void ShowRefObject()
        {
            XRStudio_PrefabRedirectHand window = (XRStudio_PrefabRedirectHand)EditorWindow.GetWindowWithRect(typeof(XRStudio_PrefabRedirectHand), new Rect(new Vector2(400, 400), new Vector2(600, 400)), true);
            window.Show();
        }


        void OnGUI()
        {

            GUILayout.BeginVertical(GUILayout.Height(100));

            EditorGUILayout.HelpBox("注意： Gameobject原Prefab名字必须与新Prefab完全相同！！！！", MessageType.Error, true);

            if (GUILayout.Button("开始重定向到新的Prefab", GUILayout.Width(200), GUILayout.Height(30)))//GUILayout.Width(200), GUILayout.Height(30)
            {
                excuteFunc();
                //this.Close();
            }

            GUILayout.EndVertical();
        }

        private void excuteFunc()
        {
            //var newPrefabName = "cubePrefab";
            GameObject[] objectArray = Selection.gameObjects;

            if (objectArray == null || objectArray.Length == 0)
            {
                this.ShowNotification(new GUIContent("请先选中所有需要重新指定Prefab的 GameObjects"));
                return;
            }


            var filters = new[] { "prefab file", "prefab" };

            var absluteTarPath = EditorUtility.OpenFilePanelWithFilters("select target", Application.dataPath, filters);
            Debug.Log("absluteTarPath = " + absluteTarPath);

            if (string.IsNullOrEmpty(absluteTarPath))
                return;

            var relativeTarPath = FileUtil.GetProjectRelativePath(absluteTarPath);

            Debug.Log("relativeTarPath = " + relativeTarPath);

            var tarPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativeTarPath);

            if (tarPrefab == null)
            {
                Debug.LogError("绝对路径下的Prefab 无法在相对路径中找到");
                return;
            }
            

            foreach (GameObject gameObject in objectArray)
            {
                Debug.Log("selected name = " + gameObject.name);

                //string localPath = "Assets/" + gameObject.name + ".prefab";
                //Debug.LogWarning("localPath = "+ localPath);

                //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
                //Debug.LogWarning("UniqueAssetPath localPath = "+ localPath);

                //获取该GameObject 的 Prefab
                string rootPrefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                Object rootPrefab = AssetDatabase.LoadAssetAtPath(rootPrefabPath, typeof(GameObject));
                // Debug.Log("rootPrefab name = "+ rootPrefab.name);
                // Debug.Log("rootPrefab path= "+AssetDatabase.GetAssetPath(rootPrefab));

                if (rootPrefab == null)
                {
                    Debug.LogError("Failure !!! : rootPrefab == null ， 该物体没有指向任何 Prefab");
                    continue;
                }
                else if (rootPrefab.name != tarPrefab.name)
                {
                    Debug.LogError("Failure !!! :  rootPrefab == " + rootPrefab.name + "； tarPrefabName = " + tarPrefab.name + "； 两者名字不同，确认是够有误");
                    continue;
                }

                //获取rootPrefab的路径，打印Log
                var oripath = AssetDatabase.GetAssetPath(rootPrefab);

                var gname = gameObject.name;

                var enable = gameObject.activeInHierarchy;

                var pos = gameObject.transform.localPosition;

                var rot = gameObject.transform.localRotation;

                var scale = gameObject.transform.localScale;

                var go = PrefabUtility.ConnectGameObjectToPrefab(gameObject, tarPrefab);

                go.transform.localPosition = pos;

                go.transform.localRotation = rot;

                go.transform.localScale = scale;

                go.name = gname;

                go.SetActive(enable);

                Debug.LogFormat("  Replace Prefab From:\t{0} ===>>> {1}\n \t\t\tSUCCESS !!!!!!!!!!", oripath, relativeTarPath);
            }
        }

    }
}