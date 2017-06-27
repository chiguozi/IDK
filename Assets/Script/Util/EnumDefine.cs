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
    //固定方向 如位移技能 前方或者后方 角度offset设置
    fixedDir = 1,
    //指定方向， 冲刺技能
    assginDir,
    //固定坐标 当前朝向 指定距离  暂时用不到
    fixedPos,
    //指定目标点  如闪现
    assginPos,
    // 根据目标位置设置方向  位置
    followTarget,
}