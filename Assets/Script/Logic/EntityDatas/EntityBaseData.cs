using UnityEngine;

public class EntityBaseData
{
	public uint uid = 0;
	public string url;
	public Vector3 initPos = Vector3.zero;
	public Vector3 initEuler = Vector3.zero;
	public string name;
	public Vector3 initScale = Vector3.one;
	public entityType = EntityType.Base;
}