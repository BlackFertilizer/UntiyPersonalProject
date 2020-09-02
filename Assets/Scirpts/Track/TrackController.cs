using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Track
{
    public class TrackController : MonoBehaviour
    {
        public Transform moveObjectTran;
        [HideInInspector]
        public Animator animator;
        // public TrackAnimatorManager animatorManager;

        public bool isLoop;

        public PointManager[] arrayPointMgr;
        private LinkedList<PointManager> pointList;
        private LinkedListNode<PointManager> currentPointNode;
        private PointInfo currentPointInfo; 

        Vector3 currentSpeed;
        public Vector3 CurrentSpeed
        {
            set { currentSpeed = value; }
            get { return currentSpeed; }
        }

        Action updatePositionAction;
        public Action<bool>  animatorNotify;

        void OnEnable() 
        {
            animator = moveObjectTran.GetComponent<Animator>();
            // animatorManager = new TrackAnimatorManager(animator);
            pointList = new LinkedList<PointManager>(arrayPointMgr);
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
                case PointType.PassPoint:
                    updatePositionAction = new PointBehaviourForce(this, currentPointInfo).forceChangePostion;
                    break;
                case PointType.RotatePoint:
                    updatePositionAction = null;
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
            animatorNotify(false);
            currentSpeed = currentSpeed.normalized*0.01f;
        }

        public void Continue()
        {
            animatorNotify(true);
        }
    }
}
