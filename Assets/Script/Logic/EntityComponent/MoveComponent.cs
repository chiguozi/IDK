using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveComponent : ComponentBase
{
    enum MoveType
    {
        MoveToPos,      //直接朝向目标点移动
        MoveForward,    //向当前朝向移动   角速度不为0时有效
        MoveByAstar,
    }

    enum MoveTargetType
    {
        None,
        FollowTarget,
    }

    //默认移动速度
    float _defaultSpeed;
    //默认角速度 0 表示立即转向
    float _defaultAngleSpeed = 0;
    Vector3 _curPos = Vector3.zero;
    Vector3 _dstPos = Vector3.zero;
    Vector3 _lastPos = Vector3.zero;
    Vector3 _curEulers = Vector3.zero;
    //目标欧拉角
    Vector3 _dstEulers = Vector3.zero;
    //当前点到目标点
    Vector3 _moveDir = Vector3.zero;
    Vector3 _forward = Vector3.forward;
    public Vector3 forward { get { return forward; } }
    //目标朝向  
    public Vector3 dstDir { get { return Quaternion.Euler(0, _dstEulers.y, 0) * Vector3.forward; } }
    public Vector3 lastPos { get { return _lastPos; } }
    public Vector3 curEulers { get { return _curEulers; } }
    public Vector3 curPos { get { return _curPos; } }

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

    MoveTargetType _moveTargetType = MoveTargetType.None;
    MoveType _moveType = MoveType.MoveToPos;

    //距离误差
    float _distanceDeviation = 0.1f;

    float _checkTargetInterval = 0.2f;
    float _checkTargetTime = 0;

    public void InitData(EntityBaseData data)
    {
        _curPos = data.initPos;
        SetDefaultSpeed(data.speed);
        SetDefaultAngleSpeed(data.angleSpeed);
        SetDstAngle(data.initEuler.y, true);
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


    public void SetAngleSpeedOnce(float angleSpeed)
    {
        _angleSpeed = angleSpeed;
    }


    public bool CanWalk(Vector3 pos)
    {
        return true;
    }

    //方向需要归一化  移动方向与朝向相同
    public void JoystickMove(float dirX, float dirZ)
    {
        SetDstAngle(dirX, dirZ, true);
        _moveDir.x = dirX;
        _moveDir.z = dirZ;
        SetJoystickPosInternal();
    }

    //是否可走由外部控制
    public void MoveToPos(float x, float z, float speed = 0)
    {
        if (speed > 0)
            _speed = speed;
        UpdateNextMovePosInternal(x, z);
        _isMoving = true;
        SetDstAngle(_moveDir.x, _moveDir.z);
    }

    public void MoveToTarget(Transform target)
    {
        StopMove();
        _target = target;
        _moveTargetType = MoveTargetType.FollowTarget;
        _isMoving = true;
        _checkTargetTime = 0;
    }

    public void MoveByWayPoints(List<Vector3> posList, float speed = 0)
    {
        if (_wayPoints.Count == 0)
            StopMove();
        _wayPoints.AddRange(posList);
        if (speed > 0)
            _speed = speed;
        _isMoving = true;
        CheckNextWayPointsInternal();

    }

    public void SetDstAngle(float x, float z, bool force = false)
    {
        var angle = Mathf.Acos(Mathf.Clamp(Vector3.forward.x * x + Vector3.forward.z * z, -1, 1)) * Mathf.Rad2Deg;
        if (x < 0)
        {
            angle = -angle;
        }
        SetDstAngle(angle, force);
    }
    //设置最终角度
    public void SetDstAngle(float angle, bool force = false)
    {
        if (hasAngleSpeed()&& !force && !Mathf.Approximately(angle, curEulers.y))
        {
            SetDstAngleInternal(angle);
        }
        else
        {
            _isRotating = false;
            SetAngleInternal(angle);
        }
    }

    public void StopMove()
    {
        _speed = _defaultSpeed;
        _angleSpeed = _defaultAngleSpeed;
        if(_isMoving || _wayPoints.Count > 0)
        {
            _wayPoints.Clear();
            _isMoving = false;
        }
        _moveTargetType = MoveTargetType.None;
        if (_target != null)
            _target = null;
    }



    public override void Update(float delTime)
    {
        UpdateAngleInternal(delTime);
        UpdatePosInternal(delTime);
    }

    bool hasAngleSpeed()
    {
        return _angleSpeed != 0;
    }

    void UpdatePosInternal(float delTime)
    {
        if (!_isMoving)
            return;
        if (_moveTargetType == MoveTargetType.FollowTarget)
            RefreshTargetPos(delTime);

        if(_moveType == MoveType.MoveToPos)
        {
            MoveToPosInternal(delTime, true);
        }
    }

    void RefreshTargetPos(float delTime)
    {
        _checkTargetTime -= delTime;
        if (_checkTargetTime <= 0)
        {
            _checkTargetTime = _checkTargetInterval;
            if (_target == null)
            {
                _moveTargetType = MoveTargetType.None;
                return;
            }
            var nextPos = _target.position;
            //UpdateNextMovePosInternal(nextPos.x, nextPos.z);
            MoveToPos(nextPos.x, nextPos.z);
            if (_distance == _distanceDeviation)
            {
                OnReachInternal();
            }
        }
    }

    void MoveToPosInternal(float delTime, bool checkWayPoints = true)
    {
        var moveOffset = _speed * delTime;
        if (moveOffset > _distance)
            moveOffset = _distance;
        _distance -= moveOffset;
        _curPos.x += _moveDir.x * moveOffset;
        _curPos.z += _moveDir.z * moveOffset;
        Send(ComponentEvents.UpdatePos, _curPos);

        if (_distance <= _distanceDeviation)
        {
            if (checkWayPoints && _wayPoints.Count > 0)
            {
                CheckNextWayPointsInternal();
            }
            else
            {
                OnReachInternal();
                //_isMoving = false;
                //Send(ComponentEvents.OnMoveEnd);
            }
        }
    }


    void CheckNextWayPointsInternal()
    {
        if (_wayPoints.Count == 0)
        {
            _isMoving = false;
            Send(ComponentEvents.OnMoveEnd);
            return;
        }
        var nextPos = _wayPoints[0];
        _wayPoints.RemoveAt(0);
        var vec = nextPos - curPos;
        if (vec.XZSqrMagnitude() <= 0.0001)
        {
            CheckNextWayPointsInternal();
        }
        else
        {
            MoveToPos(nextPos.x, nextPos.z);
            _isMoving = true;
        }
    }


    void OnReachInternal()
    {
        _isMoving = false;
        Send(ComponentEvents.OnMoveEnd);
        if(_moveTargetType == MoveTargetType.FollowTarget)
        {
            _moveTargetType = MoveTargetType.None;
            _target = null;
        }
        _speed = _defaultSpeed;
    }

    void UpdateAngleInternal(float delTime)
    {
        if (!_isRotating || _angleSpeed == 0)
            return;
        //角速度大于0
        var angleDelta = _angleSpeed * delTime; 
        if(_angleDistance < angleDelta)
        {
            angleDelta = _angleDistance;
        }
        _angleDistance -= angleDelta;
        var angle = angleDelta * _angleSymbol + _curEulers.y;
        SetAngleInternal(angle);
        if(_angleDistance <= 0)
        {
            _isRotating = false;
            _angleSpeed = _defaultAngleSpeed;
        }
    }

    void SetDstAngleInternal(float angle)
    {
        _dstEulers.y = angle;
        _angleDistance = Mathf.DeltaAngle(_curEulers.y, angle);
        if (_angleDistance < 0)
        {
            _angleDistance = -_angleDistance;
            _angleSymbol = -1;
        }
        else
        {
            _angleSymbol = 1;
        }
        _isRotating = true;
    }


    void UpdateNextMovePosInternal(float x, float z)
    {
        _lastPos = _lastPos.CopyXZ(_dstPos);
        _moveDir.x = x - _curPos.x;
        _moveDir.z = z - _curPos.z;
        _distance = _moveDir.XZMagnitude();
        _moveDir = _moveDir.XZNormalize();
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
        var dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        _forward = _forward.CopyXZ(dir);
        Send(ComponentEvents.UpdateAngle);
    }



}
