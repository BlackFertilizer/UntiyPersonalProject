using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Track
{
    public class PointBehaviourForce : PointBehaviour
    {
        public PointInfo point;
        Transform moveObjectTran;

        ///<summary>
        ///外力改变运动方向
        ///</summary>
        public PointBehaviourForce(TrackController trackController, PointInfo pointInfo)
        {
            this.point = pointInfo;
            moveObjectTran = trackController.moveObjectTran;
            baseInit(trackController, pointInfo);
        }

        public override void forceChangePostion()
        {
            if (!isRunning)
            {
                return;
            }

            Vector3 displacement = point.transform.position - moveObjectTran.position;

            //当物体运动到目标范围，开始寻找下一个目标点
            if (displacement.magnitude < point.touchRange)
            {
                endAction();
                return;
            }

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
}
