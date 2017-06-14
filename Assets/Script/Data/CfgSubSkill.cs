using System;
using System.Collections.Generic;
using UnityEngine;

public class CfgSubSkill : ConfigTextBase
{
	public List<List<string>> skillActionList;
	public int canMove;
	public int canSelect;


	public override void Write(int i, string value)
	{
		switch (i)
		{
			case 0:
				ID = ParseInt(value);
				break;
			case 1:
				skillActionList = ParseListListString(value);
				break;
			case 2:
				canMove = ParseInt(value);
				break;
			case 3:
				canSelect = ParseInt(value);
				break;
			default:
				UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
				break;
		}
	}
}
