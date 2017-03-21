using UnityEngine;


public static class VectorExtension
{
	public static float XZSqrMagnitude(Vector3 a, Vector3 b)
	{
		float dx = a.x - b.x;
		float dz = a.z - b.z;
		return dx * dx + dz * dz;
	}
	
	public static float XZMagnitude(Vector3 a, Vector3 b)
	{
		return Mathf.Sqrt(XZSqrMagnitude(a, b));
	}
	
	public static float XZMagnitude(this Vector3 a)
	{
		return Mathf.Sqrt(a.x * a.x + a.z * a.z);
	}
	
	public static float XZSqrMagnitude(this Vector3 a)
	{
		return a.x * a.x + a.z * a.z;
	}
	
	//Vector3是值类型，需要返回Vector3 否则修改无效
	public static Vector3 CopyXZ(this Vector3 a, Vector3 b)
	{
		a.x = b.x
		a.z = b.z;
		return a;
	}
	
	public  static Vector3 XZNormalize(this Vector3 a)
	{
		var num = Mathf.Sqrt(a.x * a.x + a.z * a.z);
		if(num > 0.0001)
		{
			a.x = a.x / num;
			a.z = a.z / num;
		}
		else
		{
			a.x = 0;
			a.z = 0;
		}
		return a;
	}
}