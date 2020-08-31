using System.Linq;
namespace Art.Studio
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("ArtStudio/Camera/RayUtils")]
    public class ArtStudio_Camera_RayUtils : MonoBehaviour
    {
        private const string kKeyName = @"Fire1";

        private List<IRayTrigger> oldTranList;

 
        void Start()
        {
            oldTranList = new List<IRayTrigger>();
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (Input.GetButtonDown(kKeyName))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    var hitTransform = hitInfo.transform;

                    // IRayTrigger vTr  = hitTransform.GetComponent<IRayTrigger>();

                    IRayTrigger[] vTrs = hitTransform.GetComponents<IRayTrigger>();

                    if (vTrs != null)
                    {
                        for (var i = 0; i < vTrs.Length; i++)
                        {
                            if(vTrs != null)
                            {
                                vTrs[i].ReceiveRay(ray);
                                if(oldTranList!=null && !oldTranList.Contains(vTrs[i]))
                                     oldTranList.Add(vTrs[i]);
                            }
                                
                        }
                    }

                    // if (oldTrans != null && oldTrans != vTrs)
                    // {
                    //     // Debug.Log("111");
                    //     for (var i = 0; i < oldTrans.Length; i++)
                    //     {
                    //         oldTrans[i].UnReceiveRay();
                    //     }
                    // }


                }
                else
                {
                    if (oldTranList != null)
                    {
                        for (var i = 0; i < oldTranList.Count; i++)
                        {
                            if(oldTranList[i] != null)
                            {
                                oldTranList[i].UnReceiveRay();
                            }
                        }

                        oldTranList.Clear();   
                    }

                }
            }
        }

        void OnDestroy()
        {
            oldTranList.Clear();
            oldTranList = null;
        }
    }
}