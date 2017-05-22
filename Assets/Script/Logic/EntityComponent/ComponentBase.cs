using System;

public class ComponentBase
{
	protected EventController _eventCtrl;
	
	public virtual void Init(EventController eventMgr)
	{
		_eventCtrl = eventMgr;
        RegistEvent();
	}
	
	protected virtual void RegistEvent()
	{}

    #region Regist

    public void Regist(string eventType, Action callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public void Regist<T>(string eventType, Action<T> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public void Regist<T, U>(string eventType, Action<T, U> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public void Regist<T, U, V>(string eventType, Action<T, U, V> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }


    public void Regist<T, U, V, W>(string eventType, Action<T, U, V, W> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }
    #endregion

    #region UnRegist
    public void UnRegist(string eventType, Action callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public void UnRegist<T>(string eventType, Action<T> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public void UnRegist<T, U>(string eventType, Action<T, U> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public void UnRegist<T, U, V>(string eventType, Action<T, U, V> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }


    public void UnRegist<T, U, V, W>(string eventType, Action<T, U, V, W> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    #endregion

    #region Send
    public void Send(string eventType)
	{
		_eventCtrl.Send(eventType);
	}
	
    public void Send<T>(string eventType, T arg)
    {
        _eventCtrl.Send(eventType, arg);
    }

    public void Send<T, U>(string eventType, T arg0, U arg1)
    {
        _eventCtrl.Send(eventType, arg0, arg1);
    }

    public void Send<T, U, V>(string eventType, T arg0, U arg1, V arg2)
    {
        _eventCtrl.Send(eventType, arg0, arg1, arg2);
    }

    public void Send<T, U, V, W>(string eventType, T arg0, U arg1, V arg2, W arg3)
    {
        _eventCtrl.Send(eventType, arg0, arg1, arg2, arg3);
    }
    #endregion
    public virtual void Update(float delTime)
	{
		
	}
	
	public virtual void Dispose()
	{
		_eventCtrl = null;
	}
}