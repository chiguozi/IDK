using UnityEngine;


public static class Extension
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
		a.x = b.x;
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
	
	static public void SetXY(this RectTransform rt, float x, float y)
	{
		Vector2 v2 = rt.anchoredPosition;
		v2.x = x;
		v2.y = y;
		rt.anchoredPosition = v2;
	}
	
	static public void SetX(this RectTransform rt, float x)
	{
		Vector2 v2 = rt.anchoredPosition;
		v2.x = x;
		rt.anchoredPosition = v2;
	}
	
	static public void SetY(this RectTransform rt, float y)
	{
		Vector2 v2 = rt.anchoredPosition;
		v2.y = y;
		rt.anchoredPosition = v2;
	}
	
	static public void SetWH(this RectTransform rt, float width, float height)
	{
		Vector2 v2 = rt.sizeDelta;
		v2.x = width;
		v2.y = height;
		rt.sizeDelta = v2;
	}
	static public void SetH(this RectTransform rt, float height)
	{
		Vector2 v2 = rt.sizeDelta;
		v2.y = height;
		rt.sizeDelta = v2;
	}
	static public void SetW(this RectTransform rt, float width)
	{
		Vector2 v2 = rt.sizeDelta;
		v2.x = width;
		rt.sizeDelta = v2;
	}
	
	static public float Height(this RectTransform rt)
	{
		return rt.rect.height;
	}
	
	static public float Width(this RectTransform rt)
	{
		return rt.rect.width;
	}
}