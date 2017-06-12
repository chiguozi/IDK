using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletActionType
{
    CreateBullet  = 1,
    ChangeTarget = 2,
}

public class BulletActionCenter
{
    //args[0] 为行为类型
    public static void CreateBulletAction(Bullet parent, List<string> args)
    {
        int bulletId = StringUtil.ParseIntFromList(args, 1, 0);
        int maxChildDepth = StringUtil.ParseIntFromList(args, 4, 0);
        //判断子弹最大深度  这样自己可以创建自己 2017-6-12 17:28:54
        if ((maxChildDepth != 0 && maxChildDepth <= parent.childDepth))
            return;

        float angleOffset = StringUtil.ParseFloatFromList(args, 2, 0);
        Vector3 posOffset = StringUtil.ParseVector3FromList(args, 3, Vector3.zero);
        var eulers = parent.eulers;
        eulers.y += angleOffset;
        var bullet = Bullet.CreateBullet(bulletId, parent.runTimeData, parent.eventControl, parent.position + posOffset, parent.eulers);
        bullet.Fire();
    }

    public static void ChangeTargetAction(Bullet parent, List<string> args)
    {
        int maxChildDepth = StringUtil.ParseIntFromList(args, 1, 1);
        //默认弹射的搜索半径都是圆
        float range = StringUtil.ParseFloatFromList(args, 2, 1);
    }
}
