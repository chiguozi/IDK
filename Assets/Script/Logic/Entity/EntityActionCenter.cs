using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityActionType
{
    //添加buff
    AddBuff = 1, 
    //替换技能
    //做成buff? 2017-6-8 13:40:42
    ChangeSkill = 2,
}

//技能击中后的效果函数集合
//依赖于DamageChecker 2017-6-8 13:41:39
public class EntityActionCenter
{

    public EntitySprite centerOwner;

    void AddBuffAction(List<string> args)
    {
    }


    void ChangeSkillAction(List<string> args)
    {

    }


}
