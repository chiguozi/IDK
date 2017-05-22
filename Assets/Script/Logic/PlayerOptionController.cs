using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionController : ControlBase
{

    public override void Init()
    {
        base.Init();
        EventManager.Regist<Vector2>(Events.SelfControlEvent.OnJoyStickMove, OnJoyStickMove);
        EventManager.Regist<int>(Events.SelfControlEvent.OnUseSkill, OnUseSkill);
    }


    void OnJoyStickMove(Vector2 dir)
    {
        //@todo  添加力度概念
    
        if (World.ThePlayer == null)
            return;
        Vector3 forward = Quaternion.Euler(new Vector3(0, CameraControl.Instance.rotateY, 0)) * new Vector3(dir.x, 0, dir.y);
        //状态机只记录动画，和状态，不操作逻辑？
        World.ThePlayer.SendSMEvent(UnitStateEvent.MoveByDir, forward.x, forward.z, 5);
    }

    void OnUseSkill(int skillId)
    {
    }

}
