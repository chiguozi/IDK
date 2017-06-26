using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent2 : ComponentBase
{
    //默认移动速度
    float _defaultSpeed;
    //默认角速度 0 表示立即转向
    float _defaultAngleSpeed = 0;

    Vector3 _curPos = Vector3.zero;
    public Vector3 curPos { get { return _curPos; } }
    Vector3 _dstPos = Vector3.zero;
    Vector3 _lastPos = Vector3.zero;
    public Vector3 lastPos { get { return _lastPos; } }
    //目标朝向
    Vector3 _dstDir = Vector3.zero;
    Vector3 _curEulers = Vector3.zero;
    public Vector3 curEulers { get { return _curEulers; } }
    //目标欧拉角
    Vector3 _dstEulers = Vector3.zero;

    //当前点到目标点
    Vector3 _moveDir = Vector3.zero;

    Vector3 _forward = Vector3.forward;
    public Vector3 forward { get { return forward; } }

    //right 不需要每次都赋值 取时计算即可
    public Vector3 right
    {
        get
        {
            return ( Quaternion.Euler(0, 90, 0) * _forward ).CloneXZ();
        }
    }

    float _speed;
    float _angleSpeed = 0;

    //距离差
    float _distance;
    //角度差
    float _angleDistance = 0;

    //角度符号
    float _angleSymbol = 1;

    bool _isMoving;
    bool _isRotating;

    List<Vector3> _wayPoints = new List<Vector3>();

    Transform _target;

    //距离误差
    float _distanceDeviation;

    float _checkTargetInterval = 0;
    float _checkTargetTime = 0;

    public void InitData(EntityBaseData data)
    {
        _curPos = data.initPos;
        SetDefaultSpeed(data.speed);
        SetDefaultAngleSpeed(data.angleSpeed);
        SetAngle(data.initEuler.y);
    }


    //加速buff等 通过这个方法设置基础速度
    public void SetDefaultSpeed(float defaultSpeed)
    {
        _defaultSpeed = defaultSpeed;
        //默认为变速立刻生效  如果需要等待移动结束生效 需要加参数
        _speed = defaultSpeed;
    }

    public void SetDefaultAngleSpeed(float angleSpeed)
    {
        _defaultAngleSpeed = angleSpeed;
        _angleSpeed = angleSpeed;
    }

    //直接设置角度
    public void SetAngle(float angle)
    {
        _isRotating = false;
        var dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        _dstDir = _dstDir.CopyXZ(dir);
        _forward = _forward.CopyXZ(dir);
        SetAngleInternal(angle);
    }

    public bool CanWalk(Vector3 pos)
    {
        return true;
    }

    //方向需要归一化
    public void JoystickMove(float dirX, float dirZ)
    {
        SetDir(dirX, dirZ);
        _moveDir.x = dirX;
        _moveDir.z = dirZ;
        SetJoystickPosInternal();
    }

    public void SetDir(float dirX, float dirZ)
    {
        _isRotating = false;
        var angle = Mathf.Acos(Mathf.Clamp(Vector3.forward.x * dirX + Vector3.forward.z * dirZ, -1, 1)) * Mathf.Rad2Deg;
        if (dirX < 0)
        {
            angle = -angle;
        }
        _dstDir.x = dirX;
        _dstDir.z = dirZ;
        _forward = _forward.CopyXZ(_dstDir);
        SetAngleInternal(angle);
    }




    public override void Update(float delTime)
    {

        base.Update(delTime);
    }


    void SetJoystickPosInternal()
    {
        var tmpPos = _speed * _moveDir * Time.deltaTime + _curPos;
        var x = tmpPos.x;
        var z = tmpPos.z;
        if (CanWalk(tmpPos))
        {
            _curPos = tmpPos;
        }
        else
        {
            //判断x可走 z不可走
            tmpPos.z = _curPos.z;
            if (CanWalk(tmpPos))
            {
                _curPos.x = x;
            }
            else
            {
                //判断z可走 x不可走
                tmpPos.z = z;
                tmpPos.x = _curPos.x;
                if (CanWalk(tmpPos))
                {
                    _curPos.z = z;
                }
            }
        }
        Send(ComponentEvents.UpdatePos, _curPos);
    }

    void SetAngleInternal(float angle)
    {
        _curEulers.y = angle;
        Send(ComponentEvents.UpdateAngle);
    }



}
