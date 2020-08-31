
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art.Studio
{

    public interface IRayTrigger
    {
         void ReceiveRay(Ray pCameraRay);
         void UnReceiveRay();

    }
}

