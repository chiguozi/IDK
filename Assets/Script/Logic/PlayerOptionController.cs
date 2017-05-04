using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionController : ControlBase
{

    public override void Init()
    {
        base.Init();
        EventManager.Regist(Events.SelfControlEvent.OnJoyStickMove, OnJoyStickMove);
        EventManager.Regist(Events.SelfControlEvent.OnUseSkill, OnUseSkill);
    }


    void OnJoyStickMove(object obj)
    {
        //@todo  添加力度概念
    
        Vector2 dir = (Vector2)obj;
        if (World.ThePlayer == null)
            return;
        Vector3 forward = Quaternion.Euler(new Vector3(0, CameraControl.Instance.rotateY, 0)) * new Vector3(dir.x, 0, dir.y);
        //状态机只记录动画，和状态，不操作逻辑？
        World.ThePlayer.SendSMEvent(UnitStateEvent.MoveByDir, forward.x, forward.z, 5);
    }

    void OnUseSkill(object obj)
    {
        int skillId = (int)obj;
    }

}
