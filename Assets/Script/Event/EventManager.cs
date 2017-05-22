using System;
using System.Collections.Generic;

public class EventManager
{
    static EventController _eventCtrl = new EventController();

    #region Regist

    public static void Regist(string eventType, Action callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public static void Regist<T>(string eventType, Action<T> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public static void Regist<T, U>(string eventType, Action<T, U> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }

    public static void Regist<T, U, V>(string eventType, Action<T, U, V> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }


    public static void Regist<T, U, V, W>(string eventType, Action<T, U, V, W> callback)
    {
        _eventCtrl.Regist(eventType, callback);
    }
    #endregion

    #region UnRegist
    public static void UnRegist(string eventType, Action callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public static void UnRegist<T>(string eventType, Action<T> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public static void UnRegist<T, U>(string eventType, Action<T, U> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    public static void UnRegist<T, U, V>(string eventType, Action<T, U, V> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }


    public static void UnRegist<T, U, V, W>(string eventType, Action<T, U, V, W> callback)
    {
        _eventCtrl.UnRegist(eventType, callback);
    }

    #endregion

    #region Send
    public static void Send(string eventType)
    {
        _eventCtrl.Send(eventType);
    }

    public static void Send<T>(string eventType, T arg)
    {
        _eventCtrl.Send(eventType, arg);
    }

    public static void Send<T, U>(string eventType, T arg0, U arg1)
    {
        _eventCtrl.Send(eventType, arg0, arg1);
    }

    public static void Send<T, U, V>(string eventType, T arg0, U arg1, V arg2)
    {
        _eventCtrl.Send(eventType, arg0, arg1, arg2);
    }

    public static void Send<T, U, V, W>(string eventType, T arg0, U arg1, V arg2, W arg3)
    {
        _eventCtrl.Send(eventType, arg0, arg1, arg2, arg3);
    }
    #endregion

}