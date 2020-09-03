using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScrollCircle :ScrollRect 
{
    protected float mRadius=0f;
    public Action<Vector3> _rotationAction;
    public Action<bool> _dragStateAction;
    protected override void Start()
    {
        base.Start();
        //计算摇杆块的半径
        mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        Debug.Log("startAnchorPos = " + (transform as RectTransform).anchoredPosition);
    }

    public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if( _dragStateAction!= null)
        {
            _dragStateAction(true);
        }
    }
 
    public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnDrag (eventData);
        var contentPostion = this.content.anchoredPosition;
    
        if(contentPostion.magnitude > mRadius)
        {
            contentPostion = contentPostion.normalized * mRadius ;
            //设置某点，限制触点最远距离
            SetContentAnchoredPosition(contentPostion);
           
        }

        if(_rotationAction != null)
        {
            _rotationAction(this.content.anchoredPosition);
        }
    }

    public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if(_dragStateAction != null)
            _dragStateAction(false);
    }


}