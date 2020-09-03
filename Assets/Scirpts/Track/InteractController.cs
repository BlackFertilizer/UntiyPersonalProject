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
        public AudioClip audioClip;
    }

    public class InteractController : MonoBehaviour
    {
        public TrackController trackController;

        Animator animator;
        AudioSource audioSource;

        [SerializeField]
        public List<MotionInfo> motionInfos;

        private int index;
        private int count;

        private bool isPlaying;

        private void OnEnable()
        {
            index = -1;
            isPlaying = false;
            animator = transform.GetComponent<Animator>();
            audioSource = transform.GetComponent<AudioSource>();

            if (motionInfos == null || motionInfos.Count == 0)
            {
                Debug.LogError("animation count == 0 !!!!");
                Destroy(this);
            }

            count = motionInfos.Count;
        }


        public void play()
        {
            // Debug.Log("play" + isPlaying);
            if (!isPlaying)
            {
                index++;
                if (index == count)
                {
                    index = 0;
                }

                playAnimationFade(motionInfos[index].name, motionInfos[index].fadeTime);
                if (audioSource != null && motionInfos[index].audioClip != null)
                {
                    audioSource.clip = motionInfos[index].audioClip;
                    audioSource.Play();
                }
            }
        }


        void Update()
        {
            if (isPlaying)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(motionInfos[index].name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
                {
                    // Debug.Log("1");
                    stop();
                }
            }

        }

        public void stop()
        {
            isPlaying = false;

            if (trackController != null)
            {
                trackController.Continue();
            }
            else
            {

            }

        }

        private void playAnimationFade(string animationName, float fadeTime)
        {
            if (trackController != null)
            {
                trackController.Pause();
            }

            if (animator != null)
            {
                 isPlaying = true;
                // animator.(animationName,true);
                animator.CrossFade(animationName, fadeTime);
            }
        }


    }
}