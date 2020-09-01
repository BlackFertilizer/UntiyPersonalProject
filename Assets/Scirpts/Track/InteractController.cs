using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Track
{

    [Serializable]
    public struct MotionInfo
    {
        public string name;
        public float fadeTime;
    }

    public class InteractController : MonoBehaviour
    {
        public TrackController trackController;
        Animator animator;

        [SerializeField]
        public List<MotionInfo> motionInfos;

        private int index;
        private int count;

        void Start() 
        {
            animator = this.transform.GetComponent<Animator>();

            if(motionInfos == null || motionInfos.Count == 0)
            {
                Debug.LogError("animation count == 0 !!!!");
                Destroy(this);
            }
            
            index = 0;
            count = motionInfos.Count;
        }
        
        public void play()
        {
            if(index >= count)
            {
                index = 0;
            }

            playAnimationFade(motionInfos[index].name, motionInfos[index].fadeTime);
            index ++; 
        }

        
        private void Update() 
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("ready_walk") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                stop();
            }
        }

        public void stop() 
        {
           
            if(trackController != null)
                trackController.animatorNotify(true);
        }

        private void playAnimationFade(string animationName, float fadeTime)
        {
            if(trackController != null)
                trackController.animatorNotify(false);

            if (animator != null)
            {    
                animator.CrossFade(animationName, fadeTime);
                animator.GetCurrentAnimatorClipInfo(0);
            }     
        }


    }
}