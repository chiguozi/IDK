using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    string _url;
    bool _lockTarget;
    float _speed;
    float _maxDistance;
    float _lifeTime;

    Vector3 _initPos;
    Vector3 _initEulers;

    EventController _eventControl;
    MoveComponent _moveCom;
    Effect _effect;

    uint _ownerId;
    uint _defaultTargetId;

    public void Update(float delTime)
    {

    }
}

public interface IBulletAction
{
    void OnTrigger(Bullet bullet, params object[] args);
}
