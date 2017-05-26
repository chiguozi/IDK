public class ConfigFactory
{
	public static ConfigTextBase Get(string configName)
	{
		switch(configName)
		{
			case "Bullet":
				return new CfgBullet();
			case "Effect":
				return new CfgEffect();
			case "Skill":
				return new CfgSkill();
			case "SubSkill":
				return new CfgSubSkill();
		}
		return null;
	}
}
