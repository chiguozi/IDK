using UnityEngine;

public class CameraControl : SingleTon<CameraControl>
{
	CameraManager _cameraMgr;
	public void Init()
	{
		var cameraGo = Camera.main.gameObject;
		GameObject.DontDestroyOnLoad(cameraGo);
		_cameraMgr = cameraGo.AddComponent<CameraManager>();
		//×¢²áÊÂ¼þ
	}
	
	public void InitCamera()
	{
		_cameraMgr.SetCameraParams(45, CameraClearFlags.Depth, Color.black, false, 10);
		_cameraMgr.SetParams(new Vectro3(55,0,0), new Vectro3(0, 3, -2), 18);
	}
	
	public void SetFocus(Transform target, float duration)
	{
		_cameraMgr.SetTarget(target, duration);
	}
}