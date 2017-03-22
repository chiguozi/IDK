using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIDragEventListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static UIDragEventListener Get(GameObject go)
	{
		var listener = go.GetComponent<UIDragEventListener>();
		if(listener == null)
			listener = go.AddComponent<UIDragEventListener>();
		return listener;
	}
	
	public delegate void VoidDelegate(GameObject go);
	public delegate void VectorDelegate(GameObject go, Vector2 delta);
	
	public VoidDelegate onBeginDrag;
	public VectorDelegate onDrag;
	public VoidDelegate onEndDrag;
	
	public object _param;
	public PointerEventData _pointEventData;
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		_pointEventData = eventData;
		if(onBeginDrag != null)
			onBeginDrag(gameObject);
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		_pointEventData = eventData;
		if(onDrag != null)
			onDrag(gameObject, eventData.delta);
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		_pointEventData = eventData;
		if(onEndDrag != null)
			onEndDrag(gameObject);
	}
}