public class ConfigFactory
{
	public static ConfigTextBase Get(string configName)
	{
		switch(configName)
		{
			case "Bullet":
				return new CfgBullet();
			case "DamageCheck":
				return new CfgDamageCheck();
			case "Effect":
				return new CfgEffect();
			case "MoveFlash":
				return new CfgMoveFlash();
			case "MoveTransfer":
				return new CfgMoveTransfer();
			case "Skill":
				return new CfgSkill();
			case "SubSkill":
				return new CfgSubSkill();
		}
		return null;
	}
}
