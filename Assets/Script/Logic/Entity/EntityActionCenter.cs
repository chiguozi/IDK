using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityActionType
{
    //添加buff
    AddBuff = 1, 
    //创建子弹
    CreateBullet = 2,

}

public class EntityActionCenter
{

    public EntitySprite centerOwner;

    void AddBuffAction(List<string> args)
    {
    }

    void CreateBulletAction(List<string> args)
    { }


}
