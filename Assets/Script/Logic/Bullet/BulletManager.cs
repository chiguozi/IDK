using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingleTon<BulletManager>
{
    Dictionary<uint, Bullet> _bullteMap = new Dictionary<uint, Bullet>();
    public void Update(float deltaTime)
    {

    }
}
