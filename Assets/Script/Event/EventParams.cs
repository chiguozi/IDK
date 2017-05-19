using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//暂时用来传递参数，object[]会有拆箱封箱问题 
//@todo 使用模板或者其它方式修改
public class EventParams
{
    public int iParam1;
    public int iParam2;
    public int iParam3;

    public float fParam1;
    public float fParam2;
    public float fParam3;

    public string sParam1;
    public string sParam2;
    public string sParam3;

    public bool bParam1;
    public bool bParam2;
    public bool bParam3;
}
