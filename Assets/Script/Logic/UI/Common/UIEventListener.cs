using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, 
    IPointerUpHandler, IPointerClickHandler
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void VectorDelegate(GameObject go, Vector2 delta);

    public VoidDelegate onClick;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onDown;
    public VoidDelegate onUp;

    public PointerEventData pointEventData;

    protected GameObject _go;

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null)
            listener = go.AddComponent<UIEventListener>();
        listener._go = go;
        return listener;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onEnter != null)
            onEnter(_go);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onExit != null)
            onExit(_go);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onDown != null)
            onDown(_go);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onUp != null)
            onUp(_go);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pointEventData = eventData;
        if (onClick != null)
            onClick(_go);
    }
}
