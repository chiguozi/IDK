using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能数据，用于子技能和bullet数据交换
public class SkillRuntimeData
{
    public int skillId;
    public uint ownerId;
    //0 表示没有目标
    public uint attackedId;
    //起始位置 默认为人物位置
    public Vector3 startPos;
    //指定位置 固定位置子弹使用  （怪物位置  或者 技能指示器位置）
    public Vector3 targetPos;

    //方向
    public Vector3 euler;
}
