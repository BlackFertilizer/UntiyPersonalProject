using System;
using System.Collections.Generic;
using UnityEngine;
namespace Track
{
    public enum PointType : byte
    {
        None,
        DelayPoint,
        AttractPoint,
        SelfControlPoint,
        PatrolPoint,
    }

    [Serializable]
    public class PointInfo
    {
        [HideInInspector]
        public Transform transform;

        //如果在接触范围，就会寻找下一个点位
        public string animationName = "None";
        public float animationFadeTime = 0;

        public PointType pointType = PointType.None;


        [Header("Attract Point 的参数设置")] 
#region  外力的方式移动
        public float maxSpeed;

        public float maxAccelerateValue;
        public float accelerateValue;
        public float touchRange;
        public float rotateFadeTime = 2;
        public float animatorSpeedRate = 1;

        //缓慢停止
        public bool openFadeStop = false;
        public float fadeStopLength = 0;
        public float fadeMinSpeed = 0.5f;
#endregion

        ///delay point
        [Header("Delay Point 的参数设置")]
        public float delayTime;
        public float saveLastSpeed = 0;
    }


    public class PointManager : MonoBehaviour
    {
        [Header("每个点默认有引力 可以添加延迟等行为")]
        [SerializeField]
        public List<PointInfo> pointList;

        private int index;
        private int count;

        void OnEnable()
        {
            index = 0;
            if (pointList != null)
            {
                count = pointList.Count;
            }
        }

        public PointInfo GetPointInfo()
        {

            if (index < count)
            {
                index++;
                pointList[index - 1].transform = this.transform;
                return pointList[index - 1];
            }
            else
            {
                index = 0;
                return null;
            }

            
        }


        #region  used in Editor 

        private void Init()
        {
            if(pointList == null)
                pointList= new List<PointInfo>();
        }

        public void addPointInfo()
        {
            Init();
            pointList.Add(new PointInfo());
        }

        public void removePointInfo(PointInfo pointInfo)
        {
            pointList.Remove(pointInfo);
        }
        #endregion

    }

}

