using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Track
{
    public class TrackController : MonoBehaviour
    {
        public Transform targetObjectTran;
        [HideInInspector]
        public Animator animator;
        // public TrackAnimatorManager animatorManager;

        [Header("是否沿路径，循环运动")]
        public bool isLoop;

        [Header("路径上每个控制点的信息")]
        public PointManager[] arrayPointMgr;
        private LinkedList<PointManager> pointList;
        private LinkedListNode<PointManager> currentPointNode;
        private PointInfo currentPointInfo; 

        Vector3 currentVelocity;
        public Vector3 CurrentVelocity
        {
            set { currentVelocity = value; }
            get { return currentVelocity; }
        }

        Action updatePositionAction;
        public Action<bool>  animatorNotify;

        void OnEnable() 
        {
            animator = targetObjectTran.GetComponent<Animator>();
            // animatorManager = new TrackAnimatorManager(animator);
            pointList = new LinkedList<PointManager>(arrayPointMgr);
            currentVelocity = Vector3.zero;
        }

        void Start() 
        {
            #region  //初始化第一个点
            currentPointNode = pointList.First;
            currentPointInfo = currentPointNode.Value.GetPointInfo();
            GetUpdateAction();
            #endregion

        }

        void Update()
        {
            if (updatePositionAction != null)
            {
                updatePositionAction();
            }
        }


        //切换到下个定位点
        public void turnToNextPoint()
        {
            //当前节点下 所有的PointInfo 都执行结束
            currentPointInfo = currentPointNode.Value.GetPointInfo();
            if (currentPointInfo == null)
            {
                if (currentPointNode.Next != null)
                {
                    currentPointNode = currentPointNode.Next;
                    currentPointInfo = currentPointNode.Value.GetPointInfo();
                    GetUpdateAction();
                }
                else
                {
                    //如果需要循环播放
                    if(isLoop)
                    {
                        currentPointNode = pointList.First;
                        currentPointInfo = currentPointNode.Value.GetPointInfo();
                        GetUpdateAction();
                    }
                    else
                    {
                        updatePositionAction = null;
                        currentPointNode = null;
                    }

                }
            }
            else
            {
                GetUpdateAction();
            }
        }

        private void GetUpdateAction()
        {
            switch (currentPointInfo.pointType)
            {
                case PointType.DelayPoint:
                    updatePositionAction = new PointBehaviourDelay(this, currentPointInfo).forceChangePostion;
                    break;
                case PointType.AttractPoint:
                    updatePositionAction = new PointBehaviourForce(this, currentPointInfo).forceChangePostion;
                    break;
                case PointType.SelfControlPoint:
                    updatePositionAction = new PointBehaviourSelfControl(this, currentPointInfo).forceChangePostion;
                    break;
                case PointType.PatrolPoint:
                    updatePositionAction = null;
                    break;
                default:
                    updatePositionAction = null;
                    break;
            }
        }


        public void Pause()
        {
            if(animatorNotify != null)
                animatorNotify(false);
            currentVelocity = Vector3.zero;//currentSpeed.normalized*0.01f;
        }

        public void Continue()
        { 
            if(animatorNotify != null)
                animatorNotify(true);
        }
    }
}
