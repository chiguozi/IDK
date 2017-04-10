using System;

public class ComponentBase
{
	ComponentEventManager _eventMgr;
	
	public virtual void Init(ComponentEventManager eventMgr)
	{
		_eventMgr = eventMgr;
        RegistEvent();
	}
	
	protected virtual void RegistEvent()
	{}
	
	public void Regist(string eventType, Action<Object> callback)
	{
		_eventMgr.Regist(eventType, callback);
	}
	
	public void UnRegist(string eventType, Action<Object>callback)
	{
		_eventMgr.UnRegist(eventType, callback);
	}
	
	public void Send(string eventType, Object obj = null)
	{
		_eventMgr.Send(eventType, obj);
	}
	
	public virtual void Update()
	{
		
	}
	
	public virtual void Dispose()
	{
		_eventMgr = null;
	}
}