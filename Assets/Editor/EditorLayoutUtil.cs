using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;

public class EditorLayoutUtil
{
    public const int DefaultBtnWidth = 200;
    public const int DefaultFieldWidth = 300;
    public static void ShowFieldsInColumns(List<FieldInfo> fields, object value, int column = 2)
    {
        int count = column;
        bool needBegin = true;
        for(int i = 0; i < fields.Count; i++)
        {
            if (needBegin)
            {
                EditorGUILayout.BeginHorizontal();
                needBegin = false;
            }
            if (ShowFieldByFieldInfo(fields[i], value))
            {
                count--;
            }
            else
            {
                Debug.LogError(fields[i].FieldType.Name);
            }
            if (count == 0)
            {
                needBegin = true;
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);
                count = 2;
            }
            else
            {
                GUILayout.Space(80);
            }
        }
        if(count > 0)
        {
            EditorGUILayout.EndHorizontal();
        }
    }

    public static bool ShowFieldByFieldInfo(FieldInfo field, object value)
    {
        bool find = true;
        switch(field.FieldType.Name.ToLower())
        {
            case "string":
                ShowStringField(field, value);
                break;
            case "int32":
                ShowIntField(field, value);
                break;
            case "single":
            case "float":
                ShowFloatField(field, value);
                break;
            case "double":
                ShowDoubleField(field, value);
                break;
            case "vector3":
                ShowVector3Field(field, value);
                break;
            case "vector2":
                ShowVector2Field(field, value);
                break;
            case "Vector4":
                ShowVector4Field(field, value);
                break;
            case "quaternion":
                ShowQuaternionField(field, value);
                break;
            case "bool":
                ShowBoolField(field, value);
                break;
            case "enum":
                ShowEnumField(field, value);
                break;
            default:
                //@todo  有没有更好的方法？
                if(field.FieldType.BaseType.Name.ToLower() == "enum")
                    ShowEnumField(field, value);
                else
                    find = false;
                break;
        }
        return find;
    }
    #region internal

    static void ShowEnumField(FieldInfo field, object value)
    {
        var objEnum = Convert.ChangeType(field.GetValue(value), field.FieldType);
        var e = objEnum;
        e = EditorGUILayout.EnumPopup(field.Name,objEnum as Enum, GUILayout.Width(DefaultFieldWidth));
        if(e != objEnum)
        {
            field.SetValue(value, e);
        }
    }
    static void ShowIntField(FieldInfo field, object value)
    {
        int objectNum = (int)field.GetValue(value);
        int num = EditorGUILayout.IntField(field.Name, objectNum,GUILayout.Width(DefaultFieldWidth));
        if(num != objectNum)
        {
            field.SetValue(value, num);
        }
    }

    static void ShowDoubleField(FieldInfo field, object value)
    {
        double objectNum = (double)field.GetValue(value);
        double num = EditorGUILayout.DoubleField(field.Name, objectNum, GUILayout.Width(DefaultFieldWidth));
        if (num != objectNum)
        {
            field.SetValue(value, num);
        }
    }

    static void ShowFloatField(FieldInfo field, object value)
    {
        float objectNum = (float)field.GetValue(value);
        float num = EditorGUILayout.FloatField(field.Name, objectNum, GUILayout.Width(DefaultFieldWidth));
        if (num != objectNum)
        {
            field.SetValue(value, num);
        }
    }

    static void ShowStringField(FieldInfo field, object value)
    {
        string objectStr = (string)field.GetValue(value);
        string str = EditorGUILayout.TextField(field.Name, objectStr, GUILayout.Width(DefaultFieldWidth));
        if (str != objectStr)
        {
            field.SetValue(value, str);
        }
    }

    static void ShowVector3Field(FieldInfo field, object value)
    {
        Vector3 objectVec = (Vector3)field.GetValue(value);
        var vec = EditorGUILayout.Vector3Field(field.Name, objectVec, GUILayout.Width(DefaultFieldWidth));
        if(objectVec != vec)
        {
            field.SetValue(value, vec);
        }
    }

    static void ShowVector2Field(FieldInfo field, object value)
    {
        Vector2 objectVec = (Vector2)field.GetValue(value);
        var vec = EditorGUILayout.Vector2Field(field.Name, objectVec, GUILayout.Width(DefaultFieldWidth));
        if (objectVec != vec)
        {
            field.SetValue(value, vec);
        }
    }

    static void ShowVector4Field(FieldInfo field, object value)
    {
        Vector4 objectVec = (Vector4)field.GetValue(value);
        var vec = EditorGUILayout.Vector4Field(field.Name, objectVec, GUILayout.Width(DefaultFieldWidth));
        if (objectVec != vec)
        {
            field.SetValue(value, vec);
        }
    }

    static void ShowQuaternionField(FieldInfo field, object value)
    {
        Quaternion objectQ = (Quaternion)field.GetValue(value);
        Vector4 objectV4 = new Vector4(objectQ.x, objectQ.y, objectQ.z, objectQ.w);
        var vec = EditorGUILayout.Vector4Field(field.Name, objectV4, GUILayout.Width(DefaultFieldWidth));
        if (objectV4 != vec)
        {
            objectQ.x = vec.x;
            objectQ.y = vec.y;
            objectQ.z = vec.z;
            objectQ.w = vec.w;
            field.SetValue(value, objectQ);
        }
    }

    static void ShowBoolField(FieldInfo field, object value)
    {
        bool objB = (bool)field.GetValue(value);
        var b = EditorGUILayout.Toggle(field.Name, objB);
        if(b != objB)
        {
            field.SetValue(value, b);
        }
    }

    #endregion

}
