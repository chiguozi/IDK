public static class ComponentEvents
{
	public const string BeginLoadModel = "BegineLoadModel";
	public const string OnModelLoaded = "OnModelLoaded";
	public const string UpdatePos = "UpdatePos";
	public const string OnMoveEnd = "OnMoveEnd";
	public const string UpdateAngle = "UpdateAngle";
    public const string CrossFade = "CrossFade";
    //通知状态机切换
    public const string OnSkillEnd = "OnSkillEnd";

    //人物相关特效 buff  近战技能  吟唱技能
    public const string OnAddRoleEffect = "OnAddRoleEffect";
    public const string OnRemoveRoleEffect = "OnRemoveRoleEffect";
}