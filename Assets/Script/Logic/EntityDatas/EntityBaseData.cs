using UnityEngine;

public class EntityBaseData
{
	public uint uid = 0;
	public Vector3 initPos = Vector3.zero;
	public Vector3 initEuler = Vector3.zero;
	public Vector3 initScale = Vector3.one;
	public EntityType entityType = EntityType.Base;

    public int campId = 1;

    //≈‰÷√
    public int careerId = 1;
    public string url;
    public string name;
    //»ÀŒÔ∞Îæ∂
    public float radius = 1;

    public float speed = 3;
    public float angleSpeed = 0;

}