using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Track
{
    public class TrackRayCast : MonoBehaviour
    {
        private const string kKeyName = @"Fire1";
        private Camera camera_;
        // public Text text;

        private void Start() {
            camera_ = this.GetComponent<Camera>();
            if(camera_ == null)
            {
                Debug.LogError("camera == null");
                Destroy(this);
            }
        }

        Ray ray;

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (Input.GetButtonDown(kKeyName))
            {
                ray = camera_.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    var hitTransform = hitInfo.transform;
                    if(hitTransform != null)
                    {
                        InteractController vTrs = hitTransform.GetComponentInChildren<InteractController>();
                        if(vTrs!= null)
                        {
                            // text.text += "ButtonDown \n";
                            vTrs.play();
                        }  
                    }
                }
            }

            // if(Input.touchCount > 0)
            // {
            //     if (Input.GetTouch(0).phase == TouchPhase.Began)
            //     {   //从手指触碰点沿相机方向发射一条射线
            //         ray = camera_.ScreenPointToRay(Input.GetTouch(0).position);
            //         RaycastHit hitInfo;
            //         if (Physics.Raycast(ray, out hitInfo, 100))
            //         {
            //             var hitTransform = hitInfo.transform;
            //             if(hitTransform != null)
            //             {
            //                 InteractController vTrs = hitTransform.GetComponentInChildren<InteractController>();
            //                 if(vTrs!= null)
            //                 {
            //                     vTrs.play();
            //                     // text.text += "GetTouch \n";
            //                 }  
            //             }

            //         }
            //     }
            // }
        }
    
    
    }
}
