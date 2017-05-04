using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@todo 添加战斗属性
public class EntitySprite : EntityBase
{
    public EntityFightAttributeData attributeData;

    protected override void AddComponent()
    {
        AddComponent<MoveComponent>();
        AddComponent<ActionComponent>();
        AddComponent<SkillControlComponent>();
    }

    public override void InitDatas()
    {
        base.InitDatas();
        attributeData = new EntityFightAttributeData();
    }
}
