using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletActionType
{
    CreateBullet  = 1,
    ChangeTarget = 2,
}

//子弹表现回调集合
//在子弹碰撞，销毁等操作下执行
public class BulletActionCenter
{
    //args[0] 为行为类型
    //暂时不区分子子弹数量和子子弹深度，假定子弹只能伤害一次
    public static void CreateBulletAction(Bullet parent, List<string> args)
    {
        if (parent.childDepth >= 8)
        {
            Debug.LogError("子弹层数太深");
            return;
        }
        int bulletId = StringUtil.ParseIntFromList(args, 1, 0);
        int maxChildDepth = StringUtil.ParseIntFromList(args, 4, 0);
        //判断子弹最大深度  这样自己可以创建自己 2017-6-12 17:28:54
        if ((maxChildDepth != 0 && maxChildDepth <= parent.childDepth))
            return;

        float angleOffset = StringUtil.ParseFloatFromList(args, 2, 0);
        Vector3 posOffset = StringUtil.ParseVector3FromList(args, 3, Vector3.zero);
        var eulers = parent.eulers;
        eulers.y += angleOffset;
        var bullet = Bullet.CreateBullet(bulletId, parent.runTimeData, parent.eventControl, parent.position + posOffset, eulers);
        bullet.childDepth = ++parent.childDepth;
        bullet.Fire();
    }

    //@todo  bullet之间转换
    //弹射子弹
    public static void ChangeTargetAction(Bullet bullet, List<string> args)
    {
        if (bullet.childDepth >= 8)
        {
            bullet.Dispose();
            Debug.LogError("子弹层数太深");
            return;
        }
        LockTargetBullet lockBullet = bullet as LockTargetBullet;
        if(lockBullet == null)
        {
            lockBullet.Dispose();
            Debug.LogError("暂时只支持锁定目标子弹弹射");
            return;
        }
        int maxChildDepth = StringUtil.ParseIntFromList(args, 1, 1);
        if(maxChildDepth <= lockBullet.childDepth)
        {
            lockBullet.Dispose();
            return;
        }
        //默认弹射的搜索半径都是圆
        float range = StringUtil.ParseFloatFromList(args, 2, 1);
        var targetId = Util.SkillSelectTarget(lockBullet.runTimeData.ownerId, ConfigTextManager.Instance.GetConfig<CfgSkill>(lockBullet.runTimeData.skillId));
        var target = World.GetEntity(targetId);
        if(target == null)
        {
            lockBullet.Dispose();
            return;
        }
        lockBullet.runTimeData.attackedId = targetId;
        lockBullet.childDepth++;
        lockBullet.RefreshTarget();
    }

    public static void ExecuteBulletAction(Bullet bullet, List<List<string>> actions)
    {
        if(actions.Count == 0)
        {
            return;
        }
        for(int i = 0; i < actions.Count; i++)
        {
            if (actions[i] == null || actions[i].Count == 0)
                continue;
            var type =(BulletActionType)StringUtil.ParseIntFromList(actions[i], 0, 1);
            switch(type)
            {
                case BulletActionType.CreateBullet:
                    CreateBulletAction(bullet, actions[i]);
                    break;
                case BulletActionType.ChangeTarget:
                    ChangeTargetAction(bullet, actions[i]);
                    break;
            }
        }
    }
}
