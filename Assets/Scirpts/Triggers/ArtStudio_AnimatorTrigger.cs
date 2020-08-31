using System;
using System.Collections.Generic;
using UnityEngine;

namespace Art.Studio
{
    [Serializable]
    public class EmitterPack
    {
        public Transform emitterTran;

        public float recRange;

        public float actSqrLimitRange
        {
            get { return recRange * recRange; }
        }

        public EmitterPack() { }

        public EmitterPack(Transform emitterTran, float recRange)
        {
            this.emitterTran = emitterTran;
            this.recRange = recRange;
        }
    }

    [AddComponentMenu("XRStudio/SceneTriggle/AnimatorTriggle")]
    //[RequireComponent(typeof(BoxCollider))]
    public class ArtStudio_AnimatorTrigger : ArtStudio_BaseTrigger
    {
        public Animator _animatorRes;
        public List<string> _stateNameArr;
        public int _stateIndex;
        public float _speed;

        /// <summary>
        /// 如果选中handPriority ，则自动触发关闭
        /// 否则当有自动触发物体的情况下，手动触发关闭
        /// </summary>
        public bool _handPriority;

        [SerializeField]
        public List<EmitterPack> _emitterPacks;
        private const string kNoneState = "none";

        private bool isPlaying;

        private float lastRealTime;
        private const float kUpdateInterval = 0.4f;

        private bool isEmitterExit;

        #region Editor下添加删除发射器
        public void addEmptyEmmitter()
        {
            _emitterPacks.Add(new EmitterPack());
        }

        public void removeEmitterPack(EmitterPack emitterPack)
        {
            _emitterPacks.Remove(emitterPack);
        }
        #endregion

        /// <summary>
        /// 添加发射器和有效距离，如果在有效距离内，则自动触发动作
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="range"></param>
        public void addEmmiter(Transform transform, float range)
        {
            
            Init();
            _emitterPacks.Add(new EmitterPack(transform, range));
            CheckEmitterExist();
        }

        public void setHandPriority(bool isHandPriority = false)
        {
            _handPriority = isHandPriority;
        }

        public void Init()
        {
            if (_emitterPacks == null)
            {
                _emitterPacks = new List<EmitterPack>();
            }

            if (_stateNameArr == null)
            {
                _stateNameArr = new List<string>();
                _stateNameArr.Add(kNoneState);
                _stateIndex = 0;
            }
        }

        void Start()
        {
            isPlaying = false;
            lastRealTime = Time.realtimeSinceStartup;
            CheckEmitterExist();
        }

        void CheckEmitterExist()
        {
             isEmitterExit = false;
            if(_emitterPacks != null)
            {
                for (var i = 0; i < _emitterPacks.Count; i++)
                {
                    var tempTran = _emitterPacks[i].emitterTran;
                    if (tempTran != null){
                        isEmitterExit = true;
                        break;
                    }
                }
            }
        }

        //Update 检测限定范围内是否存在自动触发装置
        void Update()
        {
            if (_emitterPacks != null && isEmitterExit && !_handPriority)
            {
                var curTime = Time.realtimeSinceStartup;
                if (curTime > lastRealTime + kUpdateInterval)
                {
                    var isRange =false;
                    for (var i = 0; i < _emitterPacks.Count; i++)
                    {
                        var emitterPack = _emitterPacks[i];
                        var tempTran = emitterPack.emitterTran;
                        if (tempTran != null)
                        {
                            var actSqrRange = (tempTran.position - transform.position).sqrMagnitude;
                            
                            //Debug.Log("limitSqrRange = " + emitterPack.actSqrLimitRange + " , actSqrRange = " + actSqrRange);
                            if (emitterPack.actSqrLimitRange > actSqrRange)
                            {
                                isRange =true;
                                break;
                            }
                        }
                    }    
                    //如果不是Hand 优先
   
                    if(isRange)
                    {
                        if(!isPlaying)
                        {
                            playAnimator();
                        }
                    }
                    else
                    {   
                        if(isPlaying)
                            stopAnimator();
                    }  

                    lastRealTime = curTime;
                }
            }
        }

        public override void ReceiveRay(Ray pCameraRay)
        {
            //Debug.Log("Animator ReceiveRay");
            base.ReceiveRay(default);
            if(isEmitterExit && !_handPriority)
                return;
            
            if(!isPlaying)
                playAnimator();
            else if(isPlaying)
                stopAnimator();
        }

        public override void UnReceiveRay()
        {
            //Debug.Log("Animator UnReceiveRay");
            base.UnReceiveRay();
             if(isEmitterExit && !_handPriority)
                return;
            if(isPlaying)
                stopAnimator();
        }

        //播放动画
        private void playAnimator()
        {
            if (_animatorRes != null)
            {
                if (_speed != _animatorRes.speed)
                {
                    _animatorRes.speed = _speed;
                }

                if (_animatorRes != null)
                {
                    _animatorRes.SetBool(_stateNameArr[_stateIndex], true);
                }

                isPlaying = true;
            }
            else
            {
                Debug.LogError("_animatorRes == null");
            }
            // _animatorRes.speed = _speed;
        }

        //停止动画
        private void stopAnimator()
        {
            if (_animatorRes != null)
            {
                _animatorRes.SetBool(_stateNameArr[_stateIndex], false);
                isPlaying = false;
            }
            // _animatorRes.speed =0;

        }
        public void checkResource()
        {
            bool isValid = true;
            if (this.GetComponent<BoxCollider>() == null)
            {
                isValid = false;
                Debug.LogError("Target need BoxCollider");
            }

            if (_animatorRes == null)
            {
                isValid = false;
                Debug.LogError("AnimatorTrigger need animator");
            }

            if (_stateIndex == 0)
            {
                isValid = false;
                Debug.LogError("choose state name");
            }

            if (isValid)
                Debug.Log("SUCCESS !!!");
        }

        public void resetState()
        {
            _stateNameArr.Clear();
            _stateNameArr.Add(kNoneState);

            if (_animatorRes != null)
            {
                //获取Animator parameters 之前必须的操作，不然clip count = 0，找不到parameters
                _animatorRes.gameObject.SetActive(false);
                _animatorRes.gameObject.SetActive(true);

                AnimatorControllerParameter[] parames = _animatorRes.parameters;
                for (int i = 0; i < parames.Length; i++)
                {
                    _stateNameArr.Add(parames[i].name);
                }
                _speed = _animatorRes.speed;
            }
            else
            {
                _stateIndex = 0;
                _speed = 0;
            }

            CheckEmitterExist();
        }

    }
}
