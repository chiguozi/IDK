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


    static void AddBuffAction(EntitySprite self , List<string> args)
    {
    }


    static void ChangeSkillAction(EntitySprite self, List<string> args)
    {

    }

    public static void ExecuteEntityAction(EntitySprite self, List<List<string>> actions)
    {
        if (actions.Count == 0)
        {
            return;
        }
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i] == null || actions[i].Count == 0)
                continue;
            var type = (EntityActionType)StringUtil.ParseIntFromList(actions[i], 0, 1);
            switch (type)
            {
                case EntityActionType.AddBuff:
                    AddBuffAction(self, actions[i]);
                    break;
                case EntityActionType.ChangeSkill:
                    ChangeSkillAction(self, actions[i]);
                    break;
            }
        }
    }


}
