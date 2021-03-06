﻿using System.Collections.Generic;
using UnityEngine;
using System;

//状态机的类型
public enum UnitState
{
    None,
    Idle,
    Run,
    Skill,
    Die,
}


//触发状态改变的事件
[Flags]
public enum UnitStateEvent
{
    None = 0,
    MoveByDir = 1,
    MoveToPos = 1 << 2,
    MoveByPosList = 1 << 3,
    MoveByTarget = 1 << 4,
    UseSkill = 1 << 5,
    ClearState = 1 << 6,  //清除上一个状态
}

//状态切换时，处理方式
public enum UnitStateChangeEvent
{
    None,
    Enter,      //当前状态压入栈
    Switch,     //替换当前状态
    Exit,       //当前状态出栈，返回上一状态
    Clear,      //清除栈中状态，只保留栈低状态
}

public class UnitSMBase
{
    public EntityBase target;   //状态机对象
    public UnitStateChangeEvent change; //切换下一状态的方式
    public UnitState nextState;   //下一个状态

    public virtual UnitState state { get { return UnitState.Idle; } }
    public void Init(EntityBase target)
    {
        this.target = target;
        Init();
    }

    public virtual void Init()
    { }

    //进入状态  
    public virtual void Enter(UnitStateEvent evt, object[] param)
    {
        change = UnitStateChangeEvent.None;
        nextState = UnitState.None;
    }

    public virtual void Exit()
    { }

    protected bool IsMoveEvt(UnitStateEvent evt)
    {
        return ( (int)evt & 0x01f ) > 0;
    }

    //状态切换的处理 设置切换方式以及下一个状态
    public virtual void ProcessEvent(UnitStateEvent evt, object[] param)
    { }


    public virtual void DoProcess(UnitStateEvent evt, object param)
    {
    }

    public virtual void Dispose()
    {
        target = null;
    }
}

public class UnitSMManager
{
    EntityBase _target;
    Dictionary<UnitState, UnitSMBase> _stateMap = new Dictionary<UnitState, UnitSMBase>();
    Dictionary<UnitSMBase, UnitState> _smMap = new Dictionary<UnitSMBase, UnitState>();
    List<UnitSMBase> _smStackList = new List<UnitSMBase>();

    public UnitSMManager(EntityBase target)
    {
        _target = target;
    }

    public void InitState(UnitState state, UnitSMBase sm)
    {
        sm.Init(_target);
        _stateMap[state] = sm;
        _smMap[sm] = state;
        _smStackList.Add(sm);
        sm.Enter(UnitStateEvent.None, null);
    }

    public UnitState GetCurrentState()
    {
        var state = _smStackList[_smStackList.Count - 1];
        return state.state;
    }

    public void RegistState(UnitState state, UnitSMBase sm)
    {
        sm.Init(_target);
        if (!_stateMap.ContainsKey(state))
        {
            _stateMap[state] = sm;
            _smMap[sm] = state;
        }
        else
        {
            var lastSm = _stateMap[state];
            _stateMap[state] = sm;
            _smMap[sm] = state;
            int index = _smStackList.IndexOf(lastSm);
            if (index != -1)
            {
                _smStackList[index] = sm;
                if (index == 0)
                {
                    sm.Enter(UnitStateEvent.None, null);
                }
            }
            sm.Dispose();
        }
    }

    public void ProcessEvent(UnitStateEvent evt, object[] param)
    {
        var curSm = _smStackList[0];
        curSm.ProcessEvent(evt, param);
        var change = curSm.change;
        var nextState = curSm.nextState;
        switch (change)
        {
            case UnitStateChangeEvent.Enter:
                {
                    var nextSM = _stateMap[nextState];
                    curSm.Exit();
                    _smStackList.Insert(0, nextSM);
                    _smStackList[0].Enter(evt, param);
                    break;
                }
            case UnitStateChangeEvent.Switch:
                {
                    var nextSM = _stateMap[nextState];
                    curSm.Exit();
                    _smStackList[0] = nextSM;
                    _smStackList[0].Enter(evt, param);
                    break;
                }
            case UnitStateChangeEvent.Exit:
                {
                    if (_smStackList.Count < 2)
                    {
                        Debug.LogError("栈数据异常");
                        return;
                    }
                    curSm.Exit();
                    _smStackList.RemoveAt(0);
                    _smStackList[0].Enter(evt, param);
                    break;
                }
            case UnitStateChangeEvent.Clear:
                {
                    if (_smStackList.Count >= 2)
                    {
                        curSm.Exit();
                        while (_smStackList.Count >= 2)
                        {
                            _smStackList.RemoveAt(0);
                        }
                        _smStackList[0].Enter(evt, param);
                    }
                    break;
                }
        }
    }
}