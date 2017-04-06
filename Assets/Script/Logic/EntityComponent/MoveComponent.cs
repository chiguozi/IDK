using UnityEngine;
using System.Collections.Generic;

enum MoveType
{
	pos = 1,
	dir = 2,
	target = 3,
	astar = 4,
}

//@todo
//1. speed抽离 不需要每次移动设置， 特殊移动修改和还原
public class MoveComponent : ComponentBase
{
	Vector3 _curPos = Vector3.zero;
	Vector3 _dstPos = Vector3.zero;
	Vector3 _lastPos = Vector3.zero;
	Vector3 _dstDir = Vector3.zero;
	Vector3 _curEulers = Vector3.zero;
	Vector3 _dstEulers = Vector3.zero;
	Vector3 _moveDir = Vector3.zero;
	Vector3 _forward = Vector3.zero;
	Vector3 _right = Vector3.zero;
	MoveType _moveType = MoveType.pos;
	float _speed = 0;
	float _angleSpeed = 0;
	float _distance = 0;
	float _angleOffset = 0;
	float _angleSymbol = 1;
	bool _isMoving = false;
	bool _isRotating = false;
	
	List<Vector3> _wayPoints = new List<Vector3>();
	public Vector3 curPos {get {return _curPos;}}
	public Vector3 curEulers {get {return _curEulers;}}
	public Vector3 forward {get {return _forward;}}
	public Vector3 right {get{return _right;}}
	
	Transform _target;
	
	float _reachDistance = 0;
	float _checkTargetInterval = 0;
	float _checkTargetTime = 0;
	
	public void StopMove()
	{
		if(_isMoving || _wayPoints.Count > 0)
		{
			_wayPoints.Clear();
			_isMoving = false;
		}
	}
	
	//跟随目标移动
	public void MoveByTargetAndSpeed(Transform target, float speed, float reachDistance = 0.1f, float checkInterval = 0.1f)
	{
		StopMove();
		_target = target;
		_speed = speed;
		_reachDistance =  reachDistance;
		_checkTargetInterval = checkInterval;
		_moveType = MoveType.target;
		_isMoving = true;
		_checkTargetTime = 0;
	}
	
	//移动到指定点
	public void MoveByPosAndSpeed(float x, float z, float speed)
	{
		StopMove();
		_speed = speed;
		_reachDistance = 0;
		UpdateNextMovePos(x, z);
		_moveType = MoveType.pos;
		_isMoving = true;
	}
	
	//路点移动
	public void MoveByWayPoints(List<Vector3>wayPoints, float speed)
	{
		if(_wayPoints.Count == 0)
			StopMove();
		_wayPoints.AddRange(wayPoints);
		_reachDistance = 0;
		_speed = speed;
		_isMoving = true;
		_moveType = MoveType.pos;
		CheckNextWayPoints();
	}
	
	//自己移动使用  dir必须规范化
	public void MoveByDirAndSpeedImmediately(float dirX, float dirZ, float speed)
	{
		_moveDir.x = dirX;
		_moveDir.z = dirZ;
		_speed = speed;
		_reachDistance = 0;
		var tmpPos = speed * _moveDir * Time.deltaTime + _curPos;
		var x = tmpPos.x;
		var z = tmpPos.z;
		if(CanWalk(tmpPos))
		{
			_curPos = tmpPos;
		}
		else
		{
			//判断x可走 z不可走
			tmpPos.z = _curPos.z;
			if(CanWalk(tmpPos))
			{
				_curPos.x = x;
			}
			else
			{
				//判断z可走 x不可走
				tmpPos.z = z;
				tmpPos.x = _curPos.x;
				if(CanWalk(tmpPos))
				{
					_curPos.z = z;
				}
			}
		}
		Send(ComponentEvents.UpdatePos, _curPos);
	}
	
	//-----转向相关---------
	//按角速度转向
	public void RotateByDirAndSpeed(float x, float z, float angleSpeed)
	{
		if(Mathf.Abs(angleSpeed - _angleSpeed) <= 0.01 && Mathf.Abs(x - _dstDir.x) <= 0.01 
			&& Mathf.Abs(z - _dstDir.z) <= 0.01)
			return;
		_dstDir.x = x;
		_dstDir.z = z;
		CaculateAngleOffsetAndSymbol(x,z);
		_angleSpeed = angleSpeed;
		_isRotating = true;
	}
	
	public void RotateByDirAndTime(float x, float z, float time)
	{
		if(Mathf.Abs(x - _dstDir.x) <= 0.01 && Mathf.Abs(z - _dstDir.z) <= 0.01)
			return;
		_dstDir.x = x;
		_dstDir.z = z;
		CaculateAngleOffsetAndSymbol(x,z);
		_angleSpeed = _angleOffset / time;
		_isRotating = true;
	}
	
	public void SetAngleByDir(float dirX, float dirZ)
	{
		_isRotating = false;
		var angle = Mathf.Acos(Mathf.Clamp(Vector3.forward.x * dirX * Vector3.forward.z * dirZ, -1, 1)) * Mathf.Rad2Deg;
		if(dirX < 0)
		{
			angle = -angle;
		}
		_dstDir.x = dirX;
		_dstDir.z = dirZ;
		_moveDir = _moveDir.CopyXZ(_dstDir);
		_forward = _forward.CopyXZ(_dstDir);
		CaculateRight();
		SetAngle(angle);
	}
	
	//直接设置角度
	public void SetDirByAngle(float angle)
	{
		_isRotating = false;
		var dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
		_dstDir = _dstDir.CopyXZ(dir);
		_moveDir = _moveDir.CopyXZ(dir);
		_forward = _forward.CopyXZ(dir);
		CaculateRight();
		SetAngle(angle);
	}
	
	void CheckNextWayPoints()
	{
		if(_wayPoints.Count == 0)
		{
			_isMoving = false;
			Send(ComponentEvents.OnMoveEnd);
			return;
		}
		var nextPos = _wayPoints[0];
		_wayPoints.RemoveAt(0);
		var vec = nextPos - curPos;
		if(vec.XZSqrMagnitude() <= 0.0001)
		{
			CheckNextWayPoints();
		}
		else
		{
			//@todo 转向和移动合并
			UpdateNextMovePos(nextPos.x, nextPos.z);
			//RotateByDirAndTime(_moveDir.x, _mvoeDir.z, 0.1f);
			_isMoving = true;
		}
	}
	
	void CaculateAngleOffsetAndSymbol(float x, float z)
	{
		//计算向量在Vector3.forward的偏转角度
		var angle = Mathf.Acos(Mathf.Clamp(Vector3.forward.x * x * Vector3.forward.z * z, -1, 1)) * Mathf.Rad2Deg;
		if(x < 0)
		{
			angle = -angle;
		}
		_angleOffset = Mathf.DeltaAngle(_curEulers.y, angle);
		if(_angleOffset < 0)
		{
			_angleOffset = _angleOffset;
			_angleSymbol = -1;
		}
		else
		{
			_angleSymbol = 1;
		}
	}
	
	//更新位置信息
	void UpdateNextMovePos(float x, float z)
	{
		_lastPos.x = _dstPos.x;
		_lastPos.z = _dstPos.z;
		_moveDir.x = x - _curPos.x;
		_moveDir.z = z - _curPos.z;
		_distance = _moveDir.XZMagnitude();
		_moveDir = _moveDir.XZNormalize();
	}
	
	void CaculateRight()
	{
		var right = Quaternion.Euler(0,90,0) * _forward;
		_right = _right.CopyXZ(right);
	}
	
	public override void Update()
	{
		if(!_isMoving)
		{
			return;
		}
		if(_moveType == MoveType.pos || _moveType == MoveType.dir)
			UpdatePosByDstPos(true);
		else if(_moveType == MoveType.target)
			UpdatePosByTarget();
	}
	
	void UpdatePosByDstPos(bool checkWayPoints)
	{
		var moveOffset = _speed * Time.deltaTime;
		if(moveOffset > _distance)
			moveOffset = _distance;
		_distance -= moveOffset;
		_curPos.x += _moveDir.x * moveOffset;
		_curPos.z += _moveDir.z * moveOffset;
		Send(ComponentEvents.UpdatePos, _curPos);
		
		if(_distance <= _reachDistance)
		{
			if(checkWayPoints && _wayPoints.Count > 0)
			{
				CheckNextWayPoints();
			}
			else
			{
				_isMoving = false;
				Send(ComponentEvents.OnMoveEnd);
			}
		}
	}
	
	void UpdatePosByTarget()
	{
		RefeshTargetPos();
		UpdatePosByDstPos(false);
	}
	
	void RefeshTargetPos()
	{
		_checkTargetInterval -= Time.deltaTime;
		if(_checkTargetInterval <= 0 )
		{
			_checkTargetTime = _checkTargetInterval;
			if(_target == null)
				return;
			var nextPos = _target.position;
			UpdateNextMovePos(nextPos.x, nextPos.z);
			if(_distance == _reachDistance)
			{
				_isMoving = false;
				Send(ComponentEvents.OnMoveEnd);
				return;
			}
			//RotateByDirAndTime(_moveDir.x, _moveDir.z, _checkTargetInterval);
		}
	}
	
	void UpdateAngle()
	{
		if(!_isRotating)
			return;
		var positiveAngleOffset = _angleOffset * Time.deltaTime;
		if(_angleOffset < positiveAngleOffset)
		{
			_angleOffset = positiveAngleOffset;
		}
		_angleOffset -= positiveAngleOffset;
		var angle = positiveAngleOffset * _angleSymbol + _curEulers.y;
		var dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
		_forward.x = dir.x;
		_forward.z = dir.z;
		CaculateRight();
		SetAngle(angle);
		if(_angleOffset <= 0)
			_isRotating = false;
	}
	
	void SetAngle(float angle)
	{
		_curEulers.y = angle;
		Send(ComponentEvents.UpdateAngle);
	}
	
	bool CanWalk(Vector3 pos)
	{
		return true;
	}
}