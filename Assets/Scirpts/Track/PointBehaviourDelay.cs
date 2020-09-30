using UnityEngine;

namespace Track
{
    public class PointBehaviourDelay : PointBehaviour
    {
        private float delayTime;
        private float saveLastSpeed;

        /// <summary>
        /// 在该点等待 xxx秒后，开始移动到下个点
        /// </summary>
        public PointBehaviourDelay(TrackController trackController, PointInfo pointInfoTmp)
        {
            baseInit(trackController, pointInfoTmp);
           
            saveLastSpeed = pointInfoTmp.saveLastSpeed;
            delayTime = pointInfoTmp.delayTime;

            //从静止状态到运动状态，物体静止时方向 和瞬时速度的方向相差太大导致瞬间切换方向
            //解决： 在delayBehaviour
            //      1.给予一定的 初速度与物体静止方向相同，然后慢慢受加速度影响改变方向 
            //      2.也可以第一施加的力即加速度，与物体静止方向相同，物体就是沿着初始方向运动了

            //以下为第一点的解决方案
            //但是会导致物体，瞬间有个初速度，小还好，大了就会闪现，不太可取
            //saveCurentSpeed 保持默认 0，即 初速度 为0；
            trackController.CurrentVelocity = trackController.targetObjectTran.forward * saveLastSpeed;
        }

        //切换到下一个点的动作
        public override void forceChangePostion()
        {
            if (isRunning)
            {
                delayTime -= Time.deltaTime;
                if (delayTime < 0)
                {
                    endAction();
                    return;
                }
            }
        }
    }
}