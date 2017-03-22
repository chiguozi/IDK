using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Scroll<T> : BaseUIView where T : BaseScrollItemView
{
	ScrollRect _scroll;
	RectTransform _scrollRt;
	RectTransform _gridRt;
	public Action <int , T> callback;
	public GameObject itemGo;
	
	int _maxCount;
	int _head;
	int _tail;
	
	public int HeadIndex 
	{
		return _head;
	}
	
	public int TailIndex
	{
		return _tail;
	}
	
	float _totalHeight;
	
	List<T> _itemList = new List<T>();
	public List<T> itemList {get {return _itemList;}}
	List<T> _cacheList = new List<T>();
	PointerEventData _pointData;
	UIDragEventListener _dragListener;
	
	public Action<PointerEventData> beginDragCallback;
	public Action<PointerEventData> endDragCallback;
	
	bool _isDrag = false;
	float _lastGridPosY;
	float _tmpHeight;
	
	protected override void InitView()
	{
		_scroll = GetComponent<ScrollRect>("");
		_scrollRt = _scroll.gameObject.GetComponent<RectTransform>();
		_scrollHeight = _scrollRt.rect.height;
		_gridRt = GetGameObject("grid").GetComponent<RectTransform>();
		_dragListener = UIDragEventListener.Get(_scroll.gameObject);
		_dragListener._onBeginDrag = OnBeginDrag;
		_dragListener._onDragOut = OnDragOut;
		_scroll.onValueChanged.AddListener(OnScrollValueChange);
	}
	
	public RectTransform contentRt {get {return _gridRt;}}
	
	void OnBeginDrag(GameObject go)
	{
		_isDrag = true;
		_pointData = _dragListener._pointData;
		if(beginDragCallback != null)
		{
			beginDragCallback(_pointData);
		}
	}
	
	void OnDragOut(GameObject go)
	{
		_isDrag = false;
		if(endDragCallback != null)
		{
			endDragCallback(_pointData);
		}
		_pointData = null;
	}
	
	void RestartDrag()
	{
		if(_pointData != null)
		{
			_scroll.OnBeginDrag(_pointData);
		}
	}
	
	public void SetData(int count)
	{
		_maxCount = count;
		InitScroll();
	}
	
	public void SetPosYAndHeight(float posY, float height)
	{
		_scrollRt.SetY(posY);
		_scrollRt.SetHeight(height);
		_scrollHeight = _scrollRt.Height();
		_gridRt.SetHeight(height);
	}
	
	public void Clear()
	{
		int count = _itemList.Count;
		for(int i = 0; i < count; i ++)
		{
			AddCache(0, false);
		}
		totalHeight = 0;
		//_scroll.onValueChanged.RemoveListener(OnScrollValueChange);
		_scroll.StopMovement();
		_gridRt.SetY(0);
		_head = 0;
		_tail = 0;
	}
	
	public void Refresh(bool updateItem)
	{
		totalHeight = 0;
		for(int i = 0; i < _itemList.Count; i++)
		{
			if(updateItem && callback)
			{
				callback(head + i, _itemList[i]);
			}
			_itemList[i].SetY(-totalHeight);
			_totalHeight += _itemList[i].GetHeight();
		}
		_gridRt.SetHeight(totalHeight);
	}
	
	public void Refresh(int count, bool updateItem)
	{
		if(count == _maxCount)
		{
			Refresh(updateItem);
			return;
		}
		_maxCount = count;
		Clear();
		InitScroll();
	}
	
	void InitScroll()
	{
		int offSize = 0;
		for(int i = 0; i < _maxCount; i++)
		{
			if(_totalHeight >= _scrollHeight)
			{
				offSize++;
				if(offSize > 2)
					break;
			}
			AddLast();
		}
	}
	
	void OnScrollValueChange(Vector2 normalPos)
	{
		if(_itemList.Count == 0)
			return;
		float gridPosY = _gridRt.anchoredPosition.y;
		if((int)gridPosY == (int) _lastGridPosY)
			return;
		bool up = gridPosY - _lastGridPosY > 0;
		bool needRepos = false;
		_tmpHeight = 0;
		if(up)
		{
			if(itemList.Count > 1 && GetItemBottomPosY(0) >= 0 && GetItemBottomPosY(0) < gridPosY)
			{
				needRepos = RemoveFirst();
			}
			if(_itemList.Count > 0)
			{
				if(-_itemList[_itemList.Count - 1].rectTransform.anchoredPosition.y < gridPosY + _scrollHeight)
					AddLast();
			}
		}
		else
		{
			if(_itemList.Count > 1)
			{
				if(-_itemList[_itemList.Count - 1].rectTransform.anchoredPosition.y > gridPosY + _scrollHeight)
				{
					RemoveLast();
				}
			}
			if(GetItemBottomPosY(0) >= 0 && GetItemBottomPosY(0) > gridPosY)
			{
				needRepos = AddFirst();
			}
		}
		
		if(needRepos)
		{
			Repose();
		}
		_lastGridPosY = _gridRt.anchoredPosition.y;
	}
	
	float GetItemBottomPosY(int index)
	{
		T item = _itemList[index];
		return -item.rectTransform.anchoredPosition.y + item.GetHeight();
	}
	
	bool AddFirst()
	{
		if(_head <= 0)
			return false;
		head--;
		var item = GetItem();
		if(callback != null)
			callback(head, item);
		#if UNITY_EDITOR
		item.gameObject.name = head.ToString();
		#endif
		item.rectTransform.SetY(_itemList[0].rectTransform.anchoredPosition.y + item.GetHeight());
		_itemList.Insert(0, item);
		_tmpHeight += item.GetHeight();
		return true;
	}
	
	void AddLast()
	{
		if(_tail >= _maxCount)
			return;
		T lastItem;
		if(_itemList.Count == 0)
			lastItem = null;
		else
		{
			lastItem = _itemList[_itemList.Count - 1];
		}
		var item = GetItem();
		if(callback)
			callback(_tail, item);
		#if UNITY_EDITOR
		item.gameObject.name = _tail.ToString();
		#endif
		_itemList.Add(item);
		_totalHeight += item.GetHeight();
		_gridRt.SetHeight(totalHeight);
		
		if(lastItem != null)
		{
			item.rectTransform.SetY(0);
		}
		else
		{
			item.rectTransform.SetY(lastItem.rectTransform.anchoredPosition.y - lastItem.GetHeight());
		}
		_tail++;
	}
	
	void RemoveFirst()
	{
		if(_itemList.Count <= 0)
			return false;
		_tmpHeight -= _itemList[0].GetHeight();
		AddCache(0, true);
		if(_head < 0)
			_head = 0;
		_head ++;
		return;
	}	
	
	void RemoveLast()
	{
		if(_itemList.Count <= 0)
			return;
		var item = AddCache(_itemList.Count - 1, true);
		_tail--;
		totalHeight -= item.GetHeight();
	}
	
	T GetItem()
	{
		T item;
		if(_cacheList.Count > 0)
		{
			item = _cacheList[0];
			_cacheList.RemoveAt(0);
		}
		else
		{
			item = Activator.CreateInstance<T>();
			var go = UGUITools.AddChild(_gridRt.gameObject, itemGo);
			item.gameObject = go;
		}
		item.isActive = true;
		return item;
	}
	
	T AddCache(int index, bool moveOut)
	{
		var item = _itemList[index];
		item.Reset(moveOut);
		_itemList.Remove(item);
		_cacheList.Add(item);
		item.isActive = false;
		return item;
	}
	
	void Repose()
	{
		_totalHeight = 0;
		for(int i = 0; i < _itemList.Count; i++)
		{
			_itemList[i].rectTransform.SetY(-_totalHeight);
			_totalHeight += _itemList[i].GetHeight();
		}
		_gridRt.SetHeight(_totalHeight);
		var pos = _gridRt.anchoredPosition;
		pos.y += _tmpHeight;
		_gridRt.anchoredPosition = pos;
		if(_tmpHeight <= -0.001 |\ _tmpHeight > 0.001)
		{
			RestartDrag();
		}
	}
	
}