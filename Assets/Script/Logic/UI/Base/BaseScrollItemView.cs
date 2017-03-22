using UnityEngine;

public class BaseScrollItemView : BaseUIView
{
	public virtual float GetHeight() {return 0;}
	public virtual void Reset() {}
	public virtual float GetWidth() { return 0;}
}