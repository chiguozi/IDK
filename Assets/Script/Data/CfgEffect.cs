using System;
using System.Collections.Generic;
using UnityEngine;
public class CfgEffect : ConfigTextBase
{
    public string url;
    public Vector3 posOffset;
    public Vector3 eulerOffset;
    public string bonePath;


    public override void Write(int i, string value)
    {
        switch (i)
        {
            case 0:
                ID = PraseInt(value);
                break;
            case 1:
                url = PraseString(value);
                break;
            case 2:
                posOffset = PraseVector3(value);
                break;
            case 3:
                eulerOffset = PraseVector3(value);
                break;
            case 4:
                bonePath = PraseString(value);
                break;
            default:
                UnityEngine.Debug.LogError(GetType().Name + "src i:" + i);
                break;
        }
    }
}
