using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{
	static Main _instance;
	public static Main Instance {get {return _instance;}}

	void Awake()
	{
		_instance = this;
		Init();
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
        BulletManager.Instance.Init();
        UIManager.Instance.Init();
        ConfigTextManager.Instance.Init();
        EffectManager.Instance.Init();
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
            World.ThePlayer.UseSkill();
            //Effect.CreateEffect("Prefab/Effect/hero001@atk_1_sfx", World.ThePlayer.position, World.ThePlayer.eulers, World.ThePlayer.uid, 3);
        }
        EffectManager.Instance.Update(Time.deltaTime);
        BulletManager.Instance.Update(Time.deltaTime);
        World.Update(Time.deltaTime);
	}
}
