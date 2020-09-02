using System;
using System.Collections.Generic;
using UnityEngine;
namespace Track
{
    public enum PointType : byte
    {
        None,
        DelayPoint,
        PassPoint,
        RotatePoint,
        PatrolPoint,
    }

    [Serializable]
    public class PointInfo
    {
        public PointType pointType = PointType.None;

        [HideInInspector]
        public Transform transform;

        public float maxSpeed;
        public float accelerateValue;

        //如果在接触范围，就会寻找下一个点位
        public float touchRange;

        public string animationName = "None";

        public float animationFadeTime = 0;

        public float delayTime;
    }


    public class PointManager : MonoBehaviour
    {
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

    }

}

