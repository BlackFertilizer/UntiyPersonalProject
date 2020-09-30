using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Track
{

    [Serializable]
    public class MotionInfo
    {
        public string name;
        public float fadeTime;
        public float fadeBackTime = 0.05f;
        public AudioClip audioClip;

        [Header("插播其他特效")]
        public Transform addedEffect;
        public float addedEffectShowTime;
    }

    [RequireComponent(typeof(BoxCollider))]
    public class InteractController : MonoBehaviour
    {
        [Header("如果自定义规则，要特殊处理")]
        public TrackController trackController;

        public Animator animator;

        public AudioSource audioSource;

        [SerializeField]
        public List<MotionInfo> motionInfos;

        private int index;
        private int count;

        private bool isPlaying;

        private MotionInfo currentMotionInfo;

        private float pauseTime = 0;
        private int idelHash;
        private bool canPlay;

        private void OnEnable()
        {
            index = -1;
            isPlaying = false;
            canPlay = true;
            //animator = transform.GetComponent<Animator>();
            //audioSource = transform.GetComponent<AudioSource>();


            if (animator == null || motionInfos == null || motionInfos.Count == 0)
            {
                Debug.LogError("animation count == 0 !!!!");
                Destroy(this);
            }

            count = motionInfos.Count;
        }


        public void play()
        {
            if (count == 0)
                return;

            if (canPlay)
            {
                index++;
                if (index == count)
                {
                    index = 0;
                }

                currentMotionInfo = motionInfos[index];

                playAnimationFade(currentMotionInfo.name, currentMotionInfo.fadeTime);
                if (audioSource != null && currentMotionInfo.audioClip != null)
                {
                    audioSource.clip = currentMotionInfo.audioClip;
                    audioSource.Play();
                }
            }
        }




        void Update()
        {
            if (isPlaying)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(currentMotionInfo.name))
                {
                    float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (currentMotionInfo.addedEffect != null && normalizedTime > currentMotionInfo.addedEffectShowTime)
                    {
                        // Debug.Log("Show Effect");
                        currentMotionInfo.addedEffect.gameObject.SetActive(true);
                    }

                    if (normalizedTime > 0.99)
                    {
                        // Debug.Log("Stop");
                        if (currentMotionInfo.addedEffect != null)
                            currentMotionInfo.addedEffect.gameObject.SetActive(false);
                        stop();
                    }
                }
            }

        }

        public void stop()
        {
            if (trackController != null)
            {
                trackController.Continue();
            }
            else
            {
                animator.CrossFade(idelHash, motionInfos[index].fadeBackTime, 0, pauseTime);
            }


            isPlaying = false;
            StartCoroutine(delayEnablePlay());

        }

        private IEnumerator delayEnablePlay()
        {
            yield return new WaitForSeconds(1f);
            canPlay = true;
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
                pauseTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                idelHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                animator.CrossFade(animationName, fadeTime);
            }

            canPlay = false;
        }


    }
}