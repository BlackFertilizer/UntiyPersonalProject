using System.Dynamic;
using System.Collections;
using System.Linq;
using System;
using System.ComponentModel;
namespace Art.Studio
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ArtStudio_BaseTrigger : MonoBehaviour,IRayTrigger
    {
        public delegate void  EventActionHandle();
        private EventActionHandle action_handle_;
        public event EventActionHandle OnTriggerClicked
        {
            add
            {
                action_handle_ += value;
            }

            remove
            {
                action_handle_ -=value;
            }
        }

        public virtual void ReceiveRay(Ray pCameraRay)
        {
           if(action_handle_ != null)
           {
               action_handle_();
           }
        }

        public virtual void UnReceiveRay()
        {
        //     if(doAction != null)
        //    {
        //        doAction();
        //    }
        } 

        void OnDestroy()
        {
            action_handle_ = null;
        }

    }
}
