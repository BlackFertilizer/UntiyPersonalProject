using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Track
{
    public abstract class PointBehaviour
    {
        protected TrackController trackController;

        private Animator animator;
        private string animationName;
        private float animationFadeTime;
        protected bool isRunning;

        public abstract void forceChangePostion();

        public void baseInit(TrackController trackController, PointInfo pointInfo)
        {
            this.trackController = trackController;
            this.trackController.animatorNotify += receiveNotify;


            animator = trackController.animator;
            animationName = pointInfo.animationName;
            animationFadeTime = pointInfo.animationFadeTime;
            receiveNotify(true);
        }

        public void receiveNotify(bool isrunning)
        {
            //Debug.Log("Switch = " + isrunning);
            isRunning = isrunning;
            if (isRunning)
            {
                playAnimator();
            }
        }

        private void playAnimator()
        {
            if (!string.IsNullOrEmpty(animationName))
            {
                if (animator != null)
                    animator.CrossFade(animationName, animationFadeTime);
            }
        }

        protected void endAction()
        {
            trackController.animatorNotify -= receiveNotify;
            trackController.turnToNextPoint();
        }
    }
}
