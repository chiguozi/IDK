using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILoop
{
    void Update(float delTime);

    void Init();
    void Dispose();
}

public class GameLoop
{
    //ILoop 需要再分类吗？ 2017-6-15 17:45:38
    static Dictionary<Type, ILoop> _loopMap = new Dictionary<Type, ILoop>();
    static float _timeSale = 2f;
    static bool _play = true;
    public static void Regist(ILoop loop)
    {
        var type = loop.GetType();
        if(_loopMap.ContainsKey(type))
        {
            Debug.LogError("重复注册类型");
            return;
        }
        loop.Init();
        _loopMap.Add(type, loop);
    }

    public static void UnRegist(ILoop loop)
    {
        var type = loop.GetType();
        _loopMap.Remove(type);
        loop.Dispose();
    }

    public static void SetGameTimeScale(float timeScale)
    {
        _timeSale = timeScale;
    }

    public static void SetRealTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public static void Play(bool play)
    {
        _play = play;
    }

    public static void Update(float delTime)
    {
        if (!_play)
            return;

        delTime *= _timeSale;

        var iter = _loopMap.GetEnumerator();
        while(iter.MoveNext())
        {
            iter.Current.Value.Update(delTime);
        }
    }

}
