using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgSkill : ConfigTextBase
{
	public string skillName;
	public string iconUrl;
	public float skillCd;
	public float lifeTime;
	public int faceTarget;
	public int canUseIfNoTarget;
	public List<List<string>> subSkillList;
	public float range;
	public List<string> selectTargetParam;
	public List<List<string>> sortParam;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = ParseInt(value);
				break;
			case 1:
				skillName = ParseString(value);
				break;
			case 2:
				iconUrl = ParseString(value);
				break;
			case 3:
				skillCd = ParseFloat(value);
				break;
			case 4:
				lifeTime = ParseFloat(value);
				break;
			case 5:
				faceTarget = ParseInt(value);
				break;
			case 6:
				canUseIfNoTarget = ParseInt(value);
				break;
			case 7:
				subSkillList = ParseListListString(value);
				break;
			case 8:
				range = ParseFloat(value);
				break;
			case 9:
				selectTargetParam = ParseListString(value);
				break;
			case 10:
				sortParam = ParseListListString(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
