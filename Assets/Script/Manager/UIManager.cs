using UnityEngine;
using System;
using System.Collections.Generic;

public class UILayer
{
	public const string nameLayer = "nameLayer";
	public const string mainUILayer = "mainUILayer";
	public const string popupLayer = "popupLayer";
	public const string loadingLayer = "loadingLayer";
	public const string topLayer = "topLayer";
}

public class UIManager : SingleTon<UIManager>
{
	GameObject _uiRoot;
	public GameObject uiRoot {get {return _uiRoot;}}
	
	Transform _canvasTransform;
	
	Canvas _mainCanvas;
	public Canvas mainCanvas {get {return _mainCanvas;}}
	
	Camera _uiCamera;
	public Camera uiCamera {get {return _uiCamera;}}
	
	Dictionary<string, Transform> _layerMap = new Dictionary<string, Transform>();
	Dictionary<string, WndBase> _wndMap = new Dictionary<string, WndBase>();
	
	public void Init()
	{
		_uiRoot  = GameObject.Find("UI");
		_canvasTransform = GameObject.Find("UI/MainCanvas").transform;
		_mainCanvas = GameObject.Find("UI/MainCanvas").GetComponent<Canvas>();
		_uiCamera = GameObject.Find("UI/UICamera").GetComponent<Camera>();
	}
}