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
    City = 1,  // ����
    Battle = 2, // ս������
}

public enum RecycleType
{
    ByTime,
    ByChangeScene
}


//ѡ��Ŀ��ʱ��Ӫ����
[Flags]
public enum CampType : byte
{
    Friend = 1,
    Enemy = 2,
    Neutrality = 4, //����
}

//�Ӽ�������
public enum SkillBehaviourType
{
    None,
    Action = 1,
    Effect = 2,
    Bullet = 3,
    //�˺����
    Damage = 4,
    Move = 5,
    Flash = 6,
}

public enum SelectTargetType
{
    //��ѡ��Ŀ��
    None = 1,
    //���ղ���ѡ��Ŀ��
    Normal = 2,
}


//�˺���ⷶΧ����
public enum DamageRangeType 
{
    Rect = 1,
    Circle,
    Sector,
}

//���ܷ�������
public enum SkillDirectionType
{
    //���ݷ����ƶ�  ������ݼ��ܷ����趨   �Ӽ��ܲ���Ҫ֪���ǹ̶�������ָ������
    dir = 1,
  //����
    pos,
  
    followTarget,
}