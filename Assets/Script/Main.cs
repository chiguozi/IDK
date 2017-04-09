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
        UIManager.Instance.Init();
        World.Test();
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
        if (Input.inputString == "@")
            World.ThePlayer.MoveByPos(10, 10, 1);
        World.Update();
	}
}
