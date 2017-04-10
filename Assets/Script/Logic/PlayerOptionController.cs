using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionController : ControlBase
{

    public override void Init()
    {
        base.Init();
        EventManager.Regist(Events.SelfControlEvent.OnJoyStickMove, OnJoyStickMove);
    }


    void OnJoyStickMove(object obj)
    {
        //@todo  添加力度概念
    
        Vector2 dir = (Vector2)obj;
        if (World.ThePlayer == null)
            return;
        Vector3 forward = Quaternion.Euler(new Vector3(0, CameraControl.Instance.rotateY, 0)) * new Vector3(dir.x, 0, dir.y);
        World.ThePlayer.SendSMEvent(UnitStateEvent.MoveByDir, forward.x, forward.z, 5);
    }

}
