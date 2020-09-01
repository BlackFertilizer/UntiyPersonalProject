using UnityEngine;

namespace Track
{
    public class PointBehaviourDelay : PointBehaviour
    {
        private float delayTime;

        /// <summary>
        /// 在该点等待 xxx秒后，开始移动到下个点
        /// </summary>
        public PointBehaviourDelay(TrackController trackController, PointInfo pointInfoTmp)
        {
            baseInit(trackController, pointInfoTmp);
            delayTime = pointInfoTmp.delayTime;
        }

        //切换到下一个点的动作
        public override void forceChangePostion(Transform moveObjectTran)
        {
            if (isRunning)
            {
                delayTime -= Time.deltaTime;
                if (delayTime < 0)
                {
                    endAction();
                }
            }
        }
    }
}