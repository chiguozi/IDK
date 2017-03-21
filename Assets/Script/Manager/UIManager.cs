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
		InitLayer();
		RegistEvent();
	}
	
	void RegistEvent()
	{
		EventManager.Regist(Events.UIEvent.OnWndLoaded, OnWndLoaded);
	}
	
	void OnWndLoaded(object obj)
	{
		var wnd = obj as WndBase;
		wnd.transform.SetParent(_layerMap[wnd.uiLayer], false);
	}
	
	void InitLayer()
	{
		AddLayer(UILayer.nameLayer);
		AddLayer(UILayer.mainUILayer);
		AddLayer(UILayer.popupLayer);
		AddLayer(UILayer.loadingLayer);
		AddLayer(UILayer.topLayer);
	}
	
	void AddLayer(string name)
	{
		GameObject layer = new GameObject(name);
		layer.transform.SetParent(_canvasTransform, false);
		RectTransform rt = layer.AddComponent<RectTransform>();
		rt.anchorMax = new Vector2(1,1);
		rt.anchorMin = new Vector2(0,0);
		rt.offsetMax = new Vector2(0,0);
		rt.offsetMin = new Vector2(0,0);
		_layerMap.Add(name, rt);
	}
	
	public void Open<T>(params object[] param) where T : WndBase
	{
		var type = typeof(T);
		if(!_wndMap.ContainsKey(type.Name))
			_wndMap.Add(type.Name, Activator.CreateInstance<T>());
		var wnd = _wndMap[type.Name];
		wnd.Show(param);
	}
	
	public void Hide<T>() where T : WndBase
	{
		var type = typeof(T);
		if(!_wndMap.ContainsKey(type.Name))
			return;
		var wnd = _wndMap[type.Name];
		wnd.Hide();
	}
}