using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public Transform moveObjectTran;
    private Animator animator;
    AnimatorManager animatorManager;
  
    public PointInfo[] arrayPoint;
    private LinkedList<PointInfo> pointList;
    private LinkedListNode<PointInfo> currentPointNode;
    private PointInfo currentPointInfo;
    
    Vector3 currentSpeed;
    public Vector3 CurrentSpeed
    {
        set{currentSpeed  = value;}
        get{return currentSpeed;}
    }

    Vector3 currentAccelerate;

    Action<Transform> updatePositionAction;

    void Start()
    {
        animator = moveObjectTran.GetComponent<Animator>();
        animatorManager = new AnimatorManager(animator);
        pointList = new LinkedList<PointInfo>(arrayPoint);
        
        
        #region  //初始化第一个点
        currentPointNode = pointList.First;
        currentPointInfo = currentPointNode.Value;
        if(!string.IsNullOrEmpty(currentPointInfo.animationName))
        {
            animatorManager.playAnimationFade(currentPointInfo.animationName, currentPointInfo.animationFadeTime);
        }
        GetUpdateAction();
        #endregion

    }


    void Update()
    {
        if(updatePositionAction != null)
        {
            updatePositionAction(moveObjectTran);
        }
    }


    //切换到下个定位点
    public void turnToNextPoint()
    {
        if (currentPointNode.Next != null)
        {
            currentPointNode = currentPointNode.Next;
            currentPointInfo = currentPointNode.Value;

            if(!string.IsNullOrEmpty(currentPointInfo.animationName))
            {
                animatorManager.playAnimationFade(currentPointInfo.animationName, currentPointInfo.animationFadeTime);
            }

            GetUpdateAction();
        }
        else
        {
            updatePositionAction = null;
            currentPointNode = null;
        }
    }



    private void GetUpdateAction()
    {
        switch(currentPointInfo.pointType)
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

}
