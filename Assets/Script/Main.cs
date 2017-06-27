using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{
	static Main _instance;
	public static Main Instance {get {return _instance;}}

    public bool showDrawArea = false;

	void Awake()
	{
		_instance = this;
        Init();
        SystemConfig.ShowDrawArea = showDrawArea;
    }

	void Init()
	{
		InitMVC();
        InitSingleTons();
        EventManager.Send(Events.SceneEvent.EnterScene, 1);
        World.Test();
	}

    void InitSingleTons()
    {
        RegistLoopManager();
        UIManager.Instance.Init();
        ConfigTextManager.Instance.Init();
    }

    void RegistLoopManager()
    {
        GameLoop.Regist(World.Instance);
        GameLoop.Regist(BulletManager.Instance);
        GameLoop.Regist(EffectManager.Instance);
    }

	void InitMVC()
	{
		ModelManager.Init();
		ControlManager.Init();
		ViewManager.Init();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ControlManager.Update();
		ViewManager.Update();
        UIManager.Instance.Update();
        if (Input.inputString == "@")
        {
            //World.ThePlayer.UseSkill(1);
            var go = GameObject.Find("GameObject");
            World.ThePlayer.MoveByTargetAndSpeed(go.transform, 10);
            //World.ThePlayer.MoveToPos(10, 10, 10);
            //Effect.CreateEffect("Prefab/Effect/hero001@atk_1_sfx", World.ThePlayer.position, World.ThePlayer.eulers, World.ThePlayer.uid, 3);
        }
        GameLoop.Update(Time.deltaTime);
	}
}
