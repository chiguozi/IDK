using UnityEngine;
using UnityEngine.UI;


public class WndBase
{
	string _defalutPath = "";
	string defaultPath 
	{
		get
		{
			if(string.IsNullOrEmpty(_defalutPath))
				_defalutPath = "Prefab/UI/" + GetType().Name;
			return _defalutPath;
		}
	}
	
	GameObject _gameObject;
	public GameObject gameObject {get {return _gameObject;}}
	
	Transform _transform;
	public Transform transform {get {return _transform;}}
	
	RectTransform _rectTransform;
	public RectTransform rectTransform {get { return _rectTransform;}}
	
	protected string _uiLayer = UILayer.popupLayer;
	public string uiLayer {get {return _uiLayer;}}
	
	object[] _param;
	
	bool _isVisible = false;
	protected string _path = null;
	
	Canvas _canvas;
	GraphicRaycaster _raycaster;
	
	public bool IsVisible
	{
		get {return _isVisible;}
		set 
		{
			if(_isVisible == value)
			{
				//是否需要刷新界面
				return;
			}
			_isVisible = value;
			if(!_isVisible)
			{
				EventManager.Send(Events.UIEvent.OnWndOpen, this);
			}
			if(_gameObject == null)
			{
				if(string.IsNullOrEmpty(_path))
					_path = defaultPath;
				//@todo 依赖加载
				ResourceManager.LoadResAsset(_path, OnWndLoaded);
                return;
			}
			else
			{
				DoShowOrHide();
			}
			_gameObject.SetActive(_isVisible);
		}
	}
	
	void OnWndLoaded(object obj)
	{
		if(obj == null || _gameObject != null)
			return;
		_gameObject = GameObject.Instantiate(obj as GameObject) as GameObject;
		_rectTransform = _gameObject.GetComponent<RectTransform>();
		_transform = _gameObject.transform;
		_canvas = _gameObject.AddComponent<Canvas>();
		//部分面板可以不加？
		_raycaster = _gameObject.AddComponent<GraphicRaycaster>();
		EventManager.Send(Events.UIEvent.OnWndLoaded, this);
		InitView();
		DoShowOrHide();
		_gameObject.SetActive(_isVisible);
		//设置renderorder
	}
	
	private void DoShowOrHide()
	{
		if(_isVisible)
		{
			_transform.SetAsLastSibling();
			OnShow();
			EventManager.Send(Events.UIEvent.OnWndOpen, this);
		}
		else
		{
			OnHide();
			EventManager.Send(Events.UIEvent.OnWndHide,this);
		}
	}
	
	public virtual void Show(object[] param)
	{
		_param = param;
		IsVisible = true;
	}
	
	public virtual void Hide()
	{
		IsVisible = false;
	}
	
	public WndBase()
	{
		Init();
	}
	
	protected virtual void Init()
	{}
	
	protected virtual void InitView(){}
	
	protected virtual void OnShow() {}
	protected virtual void OnHide() {}
	
	protected T GetComponent<T>(string path) where T : MonoBehaviour
	{
		var tChild = _transform.FindChild(path);
		var com = tChild.GetComponent<T>();
		return (T)com;
	}
	
	protected RectTransform GetChildTransform(string path)
	{
		var tChild = _transform.FindChild(path);
		return tChild as RectTransform;
	}
	
	protected GameObject GetGameObject(string path)
	{
		Transform t = _transform.FindChild(path);
		return (null == t)? null : t.gameObject;
	}
}
