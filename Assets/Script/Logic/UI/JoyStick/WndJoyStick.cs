using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WndJoyStick : WndBase
{
    const int MOVE_DISTANCE = 100;
    const float CHECK_INTERVAL = 0.001f;
    RectTransform _bgRt;
    RectTransform _stickRt;
    UIDragEventListener _listener;

    bool _isTouch = false;
    Vector2 _currentPos;
    float _currentDistance;
    float _time;

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
        _isTouch = true;
    }

    void OnEndDrag(GameObject go)
    {
        _isTouch = false;
        _stickRt.anchoredPosition = Vector2.zero;
        EventManager.Send(Events.SelfControlEvent.OnJoyStickMove, Vector2.zero);
    }

    void OnDrag(GameObject go, Vector2 delta)
    {
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, UIManager.Instance.uiCamera, out pos);
        var magnitude = pos.magnitude;
        if (magnitude > MOVE_DISTANCE)
        {
            pos.x = pos.x * MOVE_DISTANCE / magnitude;
            pos.y = pos.y * MOVE_DISTANCE / magnitude;
        }
        _stickRt.anchoredPosition = pos;
        _currentPos = pos.normalized;
        _currentDistance = magnitude;
        //EventManager.Send(Events.SelfControlEvent.OnJoyStickMove, pos.normalized);
    }

    public override void Update()
    {
        base.Update();
        if (!_isTouch)
            return;
        _time -= Time.unscaledDeltaTime;
        if (_time > 0)
        {
            return;
        }
        _time = CHECK_INTERVAL;
        EventManager.Send(Events.SelfControlEvent.OnJoyStickMove, _currentPos);
    }

}
