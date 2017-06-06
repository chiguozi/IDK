using System;

[Flags]
public enum EntityType
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
public enum CampType
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