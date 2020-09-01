using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Track
{
    public class TrackRayCast : MonoBehaviour
    {
        private const string kKeyName = @"Fire1";
        private Camera camera;

        private void Start() {
            camera = Camera.main;
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (Input.GetButtonDown(kKeyName))
            {

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 100))
                {
                    var hitTransform = hitInfo.transform;
                    InteractController vTrs = hitTransform.GetComponentInChildren<InteractController>();
                    vTrs.play();

                }
                else
                {

                }
            }
        }

    }
}
