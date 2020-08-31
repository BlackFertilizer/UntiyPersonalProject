using UnityEngine;



public class PointBehaviourDelay : PointBehaviour
{
    TrackController trackController;
    private float delayTime;

    private PointBehaviourForce pointBehaviourForce;

/// <summary>
/// 在该点等待 xxx秒后，开始移动到下个点
/// </summary>
    public PointBehaviourDelay(TrackController trackController, PointInfo pointInfo)
    {
        pointBehaviourForce = new PointBehaviourForce(trackController, pointInfo);
        this.trackController = trackController;
        this.delayTime = pointInfo.delayTime;
    }

//切换到下一个点的动作
    public override void forceChangePostion(Transform moveObjectTran)
    {
        if(pointBehaviourForce.CanRunning)
        {
            pointBehaviourForce.forceChangePostionEndOrNot(moveObjectTran);
        }
        else
        {
            delayTime -= Time.deltaTime;
            if(delayTime < 0)
            {
                trackController.turnToNextPoint();
            }
        }
    }
}