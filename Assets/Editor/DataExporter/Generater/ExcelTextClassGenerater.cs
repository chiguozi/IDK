﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

//不写成静态类
//@todo  添加模板方式
public class ExcelTextClassGenerater : IExcelClassGenerater
{
    public void GenerateClass(string savePath, string className, ExcelGameData data)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        string fileName = className + ExcelExporterUtil.ClientClassExt;

        List<string> types = data.fieldTypeList;
        List<string> fields = data.fieldNameList;
        StringBuilder sb = new StringBuilder();
        ExcelExporterUtil.AddCommonSpaceToSb(sb);

        sb.AppendLine("public class " + className + " : ConfigTextBase");
        sb.AppendLine("{");

        //跳过ID 字段
        for (int i = 1; i < types.Count; i++)
        {
            var type = SupportTypeUtil.GetIType(types[i]);
            if(type != null)
                sb.AppendLine(string.Format("\tpublic {0} {1};", type.realName, fields[i]));
        }

        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine("\tpublic override void Write(int i, string value)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tswitch (i)");
        sb.AppendLine("\t\t{");

        for (int i = 0; i < types.Count; i++)
        {
            sb.AppendLine("\t\t\tcase " + i + ":");
            //默认第一个字段名称为ID  先临时处理
            sb.AppendLine("\t\t\t\t" + (i == 0 ? "ID" : fields[i]) + " = " + SupportTypeUtil.GetTypeParseFuncName(types[i]) + "(value);");
            sb.AppendLine("\t\t\t\tbreak;");
        }

        sb.AppendLine("\t\t\tdefault:");
        sb.AppendLine("\t\t\t\tUnityEngine.Debug.LogError(GetType().Name + \"src i:\" + i);");
        sb.AppendLine("\t\t\t\tbreak;");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        File.WriteAllText(savePath + fileName, sb.ToString());
    }

    public static void GenerateClientClassFactory(string dataPath, string savePath, bool empty)
    {
        var files = Directory.GetFiles(dataPath, "*.bytes");
        List<string> classNameList = new List<string>();
        for(int i = 0; i < files.Length; i++)
        {
            //@todo +宏
            string fileName = Path.GetFileNameWithoutExtension(files[i]);
            classNameList.Add(fileName);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace Config.TextConfig");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic class " + ExcelExporterUtil.ConfigFactoryName);
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tpublic static ConfigTextBase Get(string configName)");
        sb.AppendLine("\t\t{");
        if(!empty)
        {
            sb.AppendLine("\t\t\tswitch(configName)");
            sb.AppendLine("\t\t\t{");

            for (int i = 0; i < classNameList.Count; i++)
            {
                sb.AppendLine("\t\t\t\tcase \"" + classNameList[i] + "\":");
                sb.AppendLine("\t\t\t\t\treturn new " + ExcelExporterUtil.ClientClassPre + classNameList[i] + "();");
            }

            sb.AppendLine("\t\t\t}");
        }

        sb.AppendLine("\t\t\treturn null;");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        sb.AppendLine("}");
        File.WriteAllText(Path.Combine(savePath, ExcelExporterUtil.ConfigFactoryName) + ExcelExporterUtil.ClientClassExt, sb.ToString());
    }

}
