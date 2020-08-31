using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehaviourForce : PointBehaviour
{
    public PointInfo point;
    public TrackController trackController;

    private bool canRunning;
    public bool CanRunning
    {
        get{ return canRunning;}
    }


    ///<summary>
    ///外力改变运动方向
    ///</summary>
    public PointBehaviourForce(TrackController trackController, PointInfo pointInfo)
    {
         this.trackController = trackController;
         this.point = pointInfo;
         canRunning = true;
    }


    public void forceChangePostionEndOrNot(Transform moveObjectTran)
    {
        Vector3 displacement =point.transform.position - moveObjectTran.position;
        //计算加速度
        Vector3 accelebrate = getAccelebrate(displacement, point.accelerateValue);
        //计算速度
        Vector3 speed = trackController.CurrentSpeed;
        speed = getSpeed(speed, accelebrate, point.maxSpeed);
        trackController.CurrentSpeed = speed;
        speed *= Time.deltaTime;

        //改变target 的位置和方向
        moveObjectTran.forward = new Vector3(speed.x, 0, speed.z);
        moveObjectTran.position += speed;

        //当物体运动到目标范围，开始寻找下一个目标点
        if (displacement.magnitude < point.touchRange )
        {
            canRunning = false;
        }
    }

    public override void forceChangePostion(Transform moveObjectTran)
    {
        Vector3 displacement = point.transform.position - moveObjectTran.position;
        //计算加速度
        Vector3 accelebrate = getAccelebrate(displacement, point.accelerateValue);
        //计算速度
        Vector3 speed = trackController.CurrentSpeed;
        speed = getSpeed(speed, accelebrate, point.maxSpeed);
        trackController.CurrentSpeed = speed;
        speed *= Time.deltaTime;

        //改变target 的位置和方向
        moveObjectTran.forward = new Vector3(speed.x, 0, speed.z);
        moveObjectTran.position += speed;

        //当物体运动到目标范围，开始寻找下一个目标点
        if (displacement.magnitude < point.touchRange )
        {
            trackController.turnToNextPoint();
        }
    }

    //获取加速度
    Vector3 getAccelebrate(Vector3 displacement, float accelerateValue)
    {
        displacement = displacement.normalized * accelerateValue;
        return displacement * Time.deltaTime;
    }

    //获取速度
    Vector3 getSpeed(Vector3 speed, Vector3 accelerate, float maxSpeed)
    {
        speed += accelerate;

        if (speed.magnitude > maxSpeed)
        {
            speed = speed.normalized * maxSpeed;
        }

        return speed;
    }
}
