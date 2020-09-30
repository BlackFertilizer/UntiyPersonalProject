using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Track
{
    public class PointBehaviourForce : PointBehaviour
    {
        Transform moveObjectTran;
        Transform pointTran;
        float touchRange;
        float maxSpeed;
        float accelebrate;
        float rotateFadeTime;
        float animatorSpeedRate;

        bool openFadeStop;
        float fadeStopLength;
        float fadeMinSpeed;


        ///<summary>
        ///外力改变运动方向
        ///</summary>
        public PointBehaviourForce(TrackController trackController, PointInfo pointInfo)
        {
            baseInit(trackController, pointInfo);

            moveObjectTran = trackController.targetObjectTran;
            pointTran = pointInfo.transform;
            touchRange = pointInfo.touchRange;
            maxSpeed = pointInfo.maxSpeed;
            accelebrate = pointInfo.accelerateValue;
            rotateFadeTime = pointInfo.rotateFadeTime;
            animatorSpeedRate = pointInfo.animatorSpeedRate;

            //缓慢停止
            openFadeStop = pointInfo.openFadeStop;
            fadeStopLength = pointInfo.fadeStopLength;
            fadeMinSpeed = pointInfo.fadeMinSpeed;
        }

        private bool beginRotateSoft;
        private float allDeltaTime =0;
        private Vector3 savedForward;

        public override void forceChangePostion()
        {
            if (!isRunning)
            {
                //还原animator speed

                //animator.speed = 1;
                return;
            }

            Vector3 displacement = pointTran.position - moveObjectTran.position;
            float displaceLength = displacement.magnitude;

            //当物体运动到目标范围，开始寻找下一个目标点
            if (displaceLength < touchRange)
            {
                //还原animator speed
                animator.speed = 1;
                endAction();
                return;
            }

            //计算加速度  计算速度
            Vector3 currentaccelebrate = getAccelebrate(displacement, accelebrate);
            Vector3 currentSpeed = trackController.CurrentVelocity;
            currentSpeed = getSpeed(currentSpeed, currentaccelebrate, maxSpeed);


#region  改变方向
            //从静止状态到运动状态，物体静止时方向 和瞬时速度的方向相差太大导致瞬间切换方向
            //解决： 
            //在delayBehaviour
            //      1.给予一定的 初速度与物体静止方向相同，然后慢慢受加速度影响改变方向 
            //          缺点：初速度大了就会导致物体向前突然位移，小了，起不到拉扯加速度的作用
            //      2.也可以第一个施加的力 即加速度，与物体静止方向相同，物体就是沿着初始方向运动了，
            //          缺点：1.麻烦  要摆放下个引力点的位置在 物体默认方向指向上  
            //               2.如果运动的过程中，停止，而下个引力点已经初始摆好，除非（******  动态的自动添加引力点到物体朝向上，优解  *******）
            //ForceBehaviour
            //      1.如果物体初速度为0，并且方向与瞬时速度即加速度差异过大，******** 使用插值的方式解决瞬间改变方向的问题（优解）******

            //默认方向 等于 加速度方向 y为0 
            Vector3 direction = new Vector3(currentSpeed.x, 0, currentSpeed.z);
            if(Mathf.Approximately(trackController.CurrentVelocity.magnitude,0) && rotateFadeTime > 0)
            {
                savedForward = moveObjectTran.forward;
                beginRotateSoft = true;
            }

            if(allDeltaTime > rotateFadeTime)
            {   
                beginRotateSoft = false;
                allDeltaTime = 0;
                savedForward = Vector3.zero;
            }

            
            if(beginRotateSoft)//开始旋转，对初始朝向和目标朝向进行插值
            {
                allDeltaTime += Time.deltaTime;
                var rate = allDeltaTime/rotateFadeTime;
                Vector3 dir = Vector3.Lerp(savedForward, direction, rate);
                moveObjectTran.forward = dir;
            }
            else
            {
                moveObjectTran.rotation = Quaternion.LookRotation(direction);
            }   
#endregion

#region 改变位置
            //速度 *deltaTime，每秒 转 每帧的移动距离
            //改变target 的位置和方向
            var tempSpeed = currentSpeed;
            if(openFadeStop)
            {
                if((displaceLength - touchRange) < fadeStopLength)
                {
                    tempSpeed = currentSpeed * (displaceLength - touchRange) / fadeStopLength;
                    if(tempSpeed.magnitude < fadeMinSpeed)
                    {
                        tempSpeed = tempSpeed.normalized*fadeMinSpeed;
                    }
                }    
            }

            moveObjectTran.position += tempSpeed * Time.deltaTime;
#endregion
            //调整动画的频率跟运动速度匹配    animator speed 
            animator.speed = tempSpeed.magnitude * animatorSpeedRate;

            trackController.CurrentVelocity = currentSpeed;
        }



        //获取加速度
        Vector3 getAccelebrate(Vector3 displaceValue, float accelerateValue)
        {
            displaceValue = displaceValue.normalized * accelerateValue;
            return displaceValue * Time.deltaTime;
        }

        //获取速度
        Vector3 getSpeed(Vector3 speedValue, Vector3 accelerateValue, float maxSpeedValue)
        {
            speedValue += accelerateValue;
            if (speedValue.magnitude > maxSpeedValue)
            {
                speedValue = speedValue.normalized * maxSpeedValue;
            }

            return speedValue;
        }

        public override void receiveNotify(bool isrunning)
        {
            base.receiveNotify(isrunning);
            if(!isRunning)
            {
                animator.speed = 1;
            }
                
         
        }

    }


}
