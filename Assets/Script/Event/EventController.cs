using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController
{
    Dictionary<string, Delegate> _eventMap = new Dictionary<string, Delegate>();

    #region send
    public  void Send(string eventType)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action callback = callbacks[i] as Action;
            if (callback != null)
                callback();
        }
    }

    public  void Send<T>(string eventType, T arg1)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T> callback = callbacks[i] as Action<T>;
            if (callback != null)
                callback(arg1);
        }
    }

    public  void Send<T, U>(string eventType, T arg1, U arg2)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U> callback = callbacks[i] as Action<T, U>;
            if (callback != null)
                callback(arg1, arg2);
        }
    }


    public  void Send<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V> callback = callbacks[i] as Action<T, U, V>;
            if (callback != null)
                callback(arg1, arg2, arg3);
        }
    }

    public  void Send<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V, W> callback = callbacks[i] as Action<T, U, V, W>;
            if (callback != null)
                callback(arg1, arg2, arg3, arg4);
        }
    }
    #endregion

    #region regist
    public  void Regist(string eventType, Action handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action)_eventMap[eventType] + handler;
    }

    public  void Regist<T>(string eventType, Action<T> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T>)_eventMap[eventType] + handler;
    }
    public  void Regist<T, U>(string eventType, Action<T, U> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U>)_eventMap[eventType] + handler;
    }
    public  void Regist<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U, V>)_eventMap[eventType] + handler;
    }

    public  void Regist<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U, V, W>)_eventMap[eventType] + handler;
    }

    #endregion

    #region unregist
    public  void UnRegist(string eventType, Action handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public  void UnRegist<T>(string eventType, Action<T> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public  void UnRegist<T, U>(string eventType, Action<T, U> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public  void UnRegist<T, U, V>(string eventType, Action<T, U, V> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U, V>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }


    public  void UnRegist<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U, V, W>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }




    #endregion

    private  bool CheckAddCallback(string eventType, Delegate callback)
    {
        if (!_eventMap.ContainsKey(eventType))
        {
            _eventMap.Add(eventType, null);
        }

        Delegate d = _eventMap[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            Debug.LogError(string.Format(
                    "Try to add not correct event {0}. Current type is {1}, adding type is {2}.",
                    eventType, d.GetType().Name, callback.GetType().Name));
            return false;
        }
        return true;
    }


    private  bool CheckRemoveCallback(string eventType, Delegate callback)
    {
        if (!_eventMap.ContainsKey(eventType))
        {
            return false;
        }

        Delegate d = _eventMap[eventType];
        if (( d != null ) && ( d.GetType() != callback.GetType() ))
        {
            Debug.LogError(string.Format(
                 "Remove listener {0}\" failed, Current type is {1}, adding type is {2}.",
                 eventType, d.GetType(), callback.GetType()));
            return false;
        }
        else
            return true;
    }
}
