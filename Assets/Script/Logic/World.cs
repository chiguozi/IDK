using UnityEngine;
using System.Collections.Generic;

public class World
{
	static Dictionary<uint, EntityBase> _entitesMap = new Dictionary<uint, EntityBase>();
	
	public static Dictionary<uint, EntityBase> entites {get {return _entitesMap;}}
	

}