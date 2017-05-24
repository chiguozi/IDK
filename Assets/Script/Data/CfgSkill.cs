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
	public List<List<string>> subSkillList;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = PraseInt(value);
				break;
			case 1:
				skillName = PraseString(value);
				break;
			case 2:
				iconUrl = PraseString(value);
				break;
			case 3:
				skillCd = PraseFloat(value);
				break;
			case 4:
				lifeTime = PraseFloat(value);
				break;
			case 5:
				faceTarget = PraseInt(value);
				break;
			case 6:
				subSkillList = PraseListListString(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
