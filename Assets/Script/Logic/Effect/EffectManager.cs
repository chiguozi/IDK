using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingleTon<EffectManager>, ILoop
{
    Dictionary<uint, Effect> _effectMap = new Dictionary<uint, Effect>();

    public void Init() { }

    public void Dispose() { }

    public void AddEffect(Effect effect)
    {
        _effectMap.Add(effect.uid, effect);
    }

    public void RemoveEffect(uint uid)
    {
        if(_effectMap.ContainsKey(uid))
        {
            _effectMap[uid].Release();
            _effectMap.Remove(uid);
        }
    }

    List<uint> _removeList = new List<uint>();
    public  void Update(float delTime)
    {
        if (_effectMap.Count <= 0)
            return;
        var iter = _effectMap.GetEnumerator();
        while(iter.MoveNext())
        {
            var eff = iter.Current.Value;
            if (eff.needUpdate)
                eff.Update(delTime);
            if(eff.lifeTime != -1)
            {
                eff.lifeTime -= delTime;
                if(eff.lifeTime < 0)
                {
                    _removeList.Add(eff.uid);
                }
            }
        }

        for(int i = 0; i < _removeList.Count; i++)
        {
            RemoveEffect(_removeList[i]);
        }
        _removeList.Clear();
    }
}
