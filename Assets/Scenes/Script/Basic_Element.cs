using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Element : MonoBehaviour
{
    protected Vector3 RespPoint = Vector3.zero;

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual void Resp()
    {
        transform.position = RespPoint;
    }

    public virtual void SetRespPoint(Vector3 point)
    {
        RespPoint = point;
    }

    public virtual Vector3 GetRespPoint()
    {
        return RespPoint ;
    }

}
