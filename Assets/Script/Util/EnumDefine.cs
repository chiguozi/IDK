using System;

[Flags]
public enum EntityType : byte
{
	Base = 0,
    Self = 1,
    Player = 2,
	Monster = 4,
}

public enum SceneType
{
    City = 1,  // 主城
    Battle = 2, // 战斗场景
}

public enum RecycleType
{
    ByTime,
    ByChangeScene
}


//选择目标时阵营类型
[Flags]
public enum CampType : byte
{
    Friend = 1,
    Enemy = 2,
    Neutrality = 4, //中立
}

//子技能类型
public enum SkillBehaviourType
{
    None,
    Action = 1,
    Effect = 2,
    Bullet = 3,
    //伤害检测
    Damage = 4,
    Move = 5,
    Flash = 6,
}

public enum SelectTargetType
{
    //不选择目标
    None = 1,
    //按照参数选择目标
    Normal = 2,
}


//伤害检测范围类型
public enum DamageRangeType 
{
    Rect = 1,
    Circle,
    Sector,
}

//技能方向类型
public enum SkillDirectionType
{
    //根据方向移动  方向根据技能方向设定   子技能不需要知道是固定方向还是指定方向
    dir = 1,
  //根据
    pos,
  
    followTarget,
}