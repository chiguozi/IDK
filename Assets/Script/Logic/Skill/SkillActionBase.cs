using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionBase
{
    public SubSkill subSkill;
    public bool needUpdate = false;
    
    public virtual void Trigger()
    {

    }

    //伤害检测会使用  @todo  移走
    public virtual void Update()
    {

    }
}
