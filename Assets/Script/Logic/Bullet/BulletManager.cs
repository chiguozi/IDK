﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingleTon<BulletManager>
{
    Dictionary<uint, Bullet> _bullteMap = new Dictionary<uint, Bullet>();
    List<uint> _removeList = new List<uint>();

    public override void Init()
    {
        base.Init();
        EventManager.Regist<Bullet>(Events.FightEvent.AddBullet, AddBullet);
        EventManager.Regist<uint>(Events.FightEvent.RemoveBullet, RemoveBullet);
    }


    void AddBullet(Bullet bullet)
    {
        _bullteMap.Add(bullet.uid, bullet);
    }
    void RemoveBullet(uint uid)
    {
        _removeList.Add(uid);
    }

    public void Update(float deltaTime)
    {
        var iter = _bullteMap.GetEnumerator();
        while(iter.MoveNext())
        {
            iter.Current.Value.Update(deltaTime);
        }

        if(_removeList.Count > 0)
        {
            for (int i = 0; i < _removeList.Count; i++)
            {
                if(_bullteMap.ContainsKey(_removeList[i]))
                {
                    RemoveInternal(_removeList[i]);
                }
            }
            _removeList.Clear();
        }
    }

    void RemoveInternal(uint uid)
    {
        var bullet = _bullteMap[uid];
        bullet.Release();
        _bullteMap.Remove(uid);
    }
}
