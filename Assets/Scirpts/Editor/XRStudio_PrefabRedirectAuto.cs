using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace STXR.XRStudio
{
    public class XRStudio_PrefabRedirctAuto : EditorWindow
    {
        private GameObject originGameObject;
        private GameObject targetPrefab;

        [MenuItem("XRStudio/PrefabManager/PrefabRedirectAuto", false, 2)]
        static void ShowRefObject()
        {
            XRStudio_PrefabRedirctAuto window = (XRStudio_PrefabRedirctAuto)EditorWindow.GetWindowWithRect(typeof(XRStudio_PrefabRedirctAuto), new Rect(new Vector2(400, 400), new Vector2(600, 400)), true);
            window.Show();
        }


        void OnGUI()
        {

            GUILayout.BeginVertical(GUILayout.Height(100));

            EditorGUILayout.HelpBox("注意： Gameobject  currentPrefab名字必须与targetPrefab完全相同！！！！", MessageType.Error, true);

            originGameObject = EditorGUILayout.ObjectField("重定向 根物体", originGameObject, typeof(GameObject), true) as GameObject;
            targetPrefab = EditorGUILayout.ObjectField("重定向  targetPrefab", targetPrefab, typeof(GameObject), true) as GameObject;


            if (GUILayout.Button("重新指向Prefab", GUILayout.Width(200)))
            {
                if (originGameObject == null)
                {
                    Debug.LogError("对不起你需要指定  rootGameobject");
                    return;
                }

                if (targetPrefab == null)
                {
                    Debug.LogError("对不起你需要指定物体的 targetPrefab");
                    return;
                }


                List<Transform> trans = new List<Transform>();
                findAll(originGameObject.transform, trans);

                if (trans == null || trans.Count == 0)
                {
                    Debug.LogError("没有找到rootPrefab 与 targetPrefab相同的 GameObejct");
                    return;
                }

                var oripath = AssetDatabase.GetAssetPath(trans[0]);

                excuteFunc(trans, oripath);
                //this.Close();
            }


            GUILayout.EndVertical();

            GUILayout.BeginArea(new Rect(500, 0, 300, 100));
            GUILayout.Button("1");
            GUILayout.EndArea();
        }

        private void findAll(Transform tran, List<Transform> trans)
        {

            //获取该GameObject 的 Prefab
            GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(tran.gameObject);
            if (rootPrefab != null && rootPrefab.name == targetPrefab.name)
            {
                Debug.LogFormat("找到 " + tran.name + " 的rootPrefab=targetPrefab = " + targetPrefab.name);
                trans.Add(tran);
            }

            if (tran.childCount > 0)
            {
                foreach (Transform tempTran in tran)
                {
                    findAll(tempTran, trans);
                }
            }
            else
            {
                return;
            }
        }


        private void excuteFunc(List<Transform> trans, string oripath)
        {
            if (trans == null)
            {
                return;
            }

            GameObject gameObject;

            foreach (Transform tran in trans)
            {

                gameObject = tran.gameObject;

                //获取rootPrefab的路径，打印Log


                var gname = gameObject.name;

                var enable = gameObject.activeInHierarchy;

                var pos = gameObject.transform.localPosition;

                var rot = gameObject.transform.localRotation;

                var scale = gameObject.transform.localScale;

                var go = PrefabUtility.ConnectGameObjectToPrefab(gameObject, targetPrefab);

                go.transform.localPosition = pos;

                go.transform.localRotation = rot;

                go.transform.localScale = scale;

                go.name = gname;

                go.SetActive(enable);
            }

            //获取rootPrefab的路径，打印Log
            var relativeTarPath = AssetDatabase.GetAssetPath(targetPrefab);
            Debug.LogFormat("  Replace Prefab From:\t{0} ===>>> {1}\n \t\t\tSUCCESS !!!!!!!!!!", oripath, relativeTarPath);
        }
    }
}