using UnityEngine;

public enum PointType : byte
{
    None,
    DelayPoint,
    PassPoint,
    RotatePoint,
    PatrolPoint,
}


public class PointInfo : MonoBehaviour
{
    public PointType pointType = PointType.None;

    public float maxSpeed;
    public float accelerateValue;
    
    //如果在接触范围，就会寻找下一个点位
    public float touchRange;

    public string animationName = "None";

    public float animationFadeTime = 0;


    public float delayTime;
}

