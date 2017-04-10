using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WndJoyStick : WndBase
{
    const int MOVE_DISTANCE = 100;
    RectTransform _bgRt;
    RectTransform _stickRt;
    UIDragEventListener _listener;


    protected override void InitView()
    {
        base.InitView();
        _bgRt = GetChildTransform("background");
        _stickRt = GetChildTransform("stick");
        _listener = UIDragEventListener.Get(gameObject);
        _listener.onBeginDrag = OnBeginDrag;
        _listener.onEndDrag = OnEndDrag;
        _listener.onDrag = OnDrag;
    }


    void OnBeginDrag(GameObject go)
    {
        
    }

    void OnEndDrag(GameObject go)
    {
        _stickRt.anchoredPosition = Vector2.zero;
        EventManager.Send(Events.SelfControlEvent.OnJoyStickMove, Vector2.zero);
    }

    void OnDrag(GameObject go, Vector2 delta)
    {
        Debug.LogError("drag");
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, UIManager.Instance.uiCamera, out pos);
        var magnitude = pos.magnitude;
        if (magnitude > MOVE_DISTANCE)
        {
            pos.x = pos.x * MOVE_DISTANCE / magnitude;
            pos.y = pos.y * MOVE_DISTANCE / magnitude;
        }
        _stickRt.anchoredPosition = pos;
        EventManager.Send(Events.SelfControlEvent.OnJoyStickMove, pos.normalized);
    }


}
