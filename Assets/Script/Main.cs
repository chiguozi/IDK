using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{
	void Awake()
	{
		Init();
	}

	void Init()
	{
		InitMVC();
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
	}
}
