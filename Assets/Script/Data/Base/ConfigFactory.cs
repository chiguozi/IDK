public class ConfigFactory
{
	public static ConfigTextBase Get(string configName)
	{
		switch(configName)
		{
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
