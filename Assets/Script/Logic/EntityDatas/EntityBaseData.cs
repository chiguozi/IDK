using UnityEngine;

public class EntityBaseData
{
	public uint uid = 0;
	public Vector3 initPos = Vector3.zero;
	public Vector3 initEuler = Vector3.zero;
	public Vector3 initScale = Vector3.one;
	public EntityType entityType = EntityType.Base;

    public int campId = 1;

    //配置
    public int careerId = 1;
    public string url;
    public string name;
    //人物半径
    public float radius = 1;

    //不算基础属性
    public float speed = 5;
    public float angleSpeed = 10;

}