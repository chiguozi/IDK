using UnityEngine;

public class BaseUIView
{
	protected Transform _transform;
	protected RectTransform _rectTransform;
	protected GameObject _gameObject;
	protected bool _isActive;
	public GameObject gameObject
	{
		get { return _gameObject;}
		set
		{
			if(_gameObject != null || value == null)
				return;
			_gameObject = value;
			_transform = _gameObject.transform;
			_rectTransform = _gameObject.GetComponent<RectTransform>();
			InitView();
			_isActive = _gameObject.activeSelf;
		}
	}
	
	public Transform transform { get {return _transform;}}
	
	public RectTransform rectTransform {get {return _rectTransform;}}
	
	public bool isActive 
	{
		get {return _isActive;}
		set
		{
			if(_isActive == value)
				return;
			_isActive = value;
			_gameObject.SetActive(_isActive);
		}
	}
	
	
	protected virtual void InitView()
	{
		
	}
	
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