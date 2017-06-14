using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill
{
    public int id;
    float _delay;
    public CfgSubSkill cfg;
    List<SkillBehaviourBase> _skillActionList = new List<SkillBehaviourBase>();

    bool _hasTriggered = false;

    public SkillRuntimeData runtimeData;

    List<SkillBehaviourBase> _updateActionList = new List<SkillBehaviourBase>();

    //@todo 子技能初始化需要重构 2017-6-14 15:35:34
    //初始化不能包含动态数据   使用的时候初始化？  分步初始化？
    public void Init(EventController eventMgr, List<string> args)
    {
        id = StringUtil.ParseInt(args[0]);
        _delay = StringUtil.ParseFloat(args[1], 0);
        cfg = ConfigTextManager.Instance.GetConfig<CfgSubSkill>(id);
        if(cfg == null)
        {
            Debug.LogError("找不到子技能 id :" + id);
            return;
        }
        for(int i = 0; i < cfg.skillActionList.Count; i++)
        {
            var behaviour = SkillBehaviourFactory.Create(cfg.skillActionList[i], this, eventMgr);
            _skillActionList.Add(behaviour);
        }
    }

    public void SetRuntimeData(SkillRuntimeData data)
    {
        runtimeData = data;
        for(int i = 0; i < _skillActionList.Count; i++)
        {
            _skillActionList[i].runtimeData = data;
        }
    }

    public void Update(float delTime)
    {
        if(!_hasTriggered && _delay >= 0)
        {
            _delay -= delTime;
            if(_delay <= 0)
            {
                _hasTriggered = true;
                Trigger();
            }
        }
        if (!_hasTriggered)
            return;
        UpdateSkillActions(delTime);
    }

    //重用使用
    public void Release()
    {
        _updateActionList.Clear();
        _hasTriggered = false;
        //skillActionList.Clear();
        _delay = 0;
        id = 0;
    }

    void Trigger()
    {
        for(int i = 0; i < _skillActionList.Count; i++)
        {
            _skillActionList[i].Trigger();
            if (_skillActionList[i].needUpdate)
                _updateActionList.Add(_skillActionList[i]);
        }
    }
    
    void UpdateSkillActions(float delTime)
    {
        for(int i = 0; i  < _updateActionList.Count; i++)
        {
            _updateActionList[i].Update(delTime);
        }
    }

}
