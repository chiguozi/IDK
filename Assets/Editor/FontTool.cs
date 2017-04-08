using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

public class FontTool
{
    [MenuItem("Tools/FontTool")]
    static void CreateFont()
    {
        if (Selection.objects == null)
            return;
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("没有选中fnt文件，或者图片文件");
            return;
        }
        //至少需要保证选中文件的目录下面有fnt文件，否则不会生成字体  
        Font m_myFont = null;
        TextAsset m_data = null;
        string filePath = "";
        Material mat = null;
        Texture2D tex = null;
        bool bln = false;
        //不管选中fnt、png、mat、fontsettings其中的任何一个，都可以创建字体  
        foreach (UnityEngine.Object o in Selection.objects)
        {
            if (o.GetType() == typeof(TextAsset))
            {
                m_data = o as TextAsset;
                bln = true;
            }
            else if (o.GetType() == typeof(Material))
            {
                mat = o as Material;
                bln = true;
            }
            else if (o.GetType() == typeof(Texture2D))
            {
                tex = o as Texture2D;
                bln = true;
            }
            else if (o.GetType() == typeof(Font))
            {
                m_myFont = o as Font;
                bln = true;
            }
            if (bln)
            {
                filePath = AssetDatabase.GetAssetPath(o);
                filePath = filePath.Substring(0, filePath.LastIndexOf('.'));
            }
        }
        //获取fnt文件，在这里加一次判断，为了可以直接选择图片也能导出字体  
        string dataPathName = filePath + ".fnt";
        if (m_data == null)
        {
            m_data = (TextAsset)AssetDatabase.LoadAssetAtPath(dataPathName, typeof(TextAsset));
        }
        if (m_data != null)
        {
            string matPathName = filePath + ".mat";
            string fontPathName = filePath + ".fontsettings";
            string texPathName = filePath + "_0.png";

            //获取图片，如果没有图片，不影响，可以生成之后，再手动设置  
            if (tex == null)
            {
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texPathName, typeof(Texture2D));
            }
            if (tex == null)
            {
                Debug.LogWarning("没找到图片，或者图片名称和fnt文件名称不匹配");
            }

            //获取材质，如果没有则创建对应名字的材质  
            if (mat == null)
            {
                mat = (Material)AssetDatabase.LoadAssetAtPath(matPathName, typeof(Material));
            }
            if (mat == null)
            {
                mat = new Material(Shader.Find("UI/Default Font"));
                AssetDatabase.CreateAsset(mat, matPathName);
            }
            else
            {
                mat.shader = Shader.Find("UI/Default Font");
            }
            mat.SetTexture("_MainTex", tex);

            //获取font文件，如果没有则创建对应名字的font文件  
            if (m_myFont == null)
            {
                m_myFont = (Font)AssetDatabase.LoadAssetAtPath(fontPathName, typeof(Font));
            }
            if (m_myFont == null)
            {
                m_myFont = new Font();
                AssetDatabase.CreateAsset(m_myFont, fontPathName);
            }
            m_myFont.material = mat;

            BMFont mbFont = new BMFont();
            BMFontReader.Load(mbFont, m_data.name, m_data.bytes);
            CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphs.Count];
            for (int i = 0; i < mbFont.glyphs.Count; i++)
            {
                BMGlyph bmInfo = mbFont.glyphs[i];
                CharacterInfo info = new CharacterInfo();
                //设置ascii码  
                info.index = bmInfo.index;
                //设置字符映射到材质上的坐标  
                info.uvBottomLeft = new Vector2((float)bmInfo.x / mbFont.texWidth, 1f - (float)( bmInfo.y + bmInfo.height ) / mbFont.texHeight);
                info.uvBottomRight = new Vector2((float)( bmInfo.x + bmInfo.width ) / mbFont.texWidth, 1f - (float)( bmInfo.y + bmInfo.height ) / mbFont.texHeight);
                info.uvTopLeft = new Vector2((float)bmInfo.x / mbFont.texWidth, 1f - (float)( bmInfo.y ) / mbFont.texHeight);
                info.uvTopRight = new Vector2((float)( bmInfo.x + bmInfo.width ) / mbFont.texWidth, 1f - (float)( bmInfo.y ) / mbFont.texHeight);
                //设置字符顶点的偏移位置和宽高  
                info.minX = bmInfo.offsetX;
                info.minY = -bmInfo.offsetY - bmInfo.height;
                info.maxX = bmInfo.offsetX + bmInfo.width;
                info.maxY = -bmInfo.offsetY;
                //设置字符的宽度  
                info.advance = bmInfo.advance;
                characterInfo[i] = info;
            }
            m_myFont.characterInfo = characterInfo;
            EditorUtility.SetDirty(m_myFont);//设置变更过的资源  
            EditorUtility.SetDirty(mat);//设置变更过的资源  
            AssetDatabase.SaveAssets();//保存变更的资源  
            AssetDatabase.Refresh();//刷新资源，貌似在Mac上不起作用  


            //必须保留  否则字体无效
            AssetDatabase.ExportPackage(fontPathName, "temp.unitypackage");
            AssetDatabase.DeleteAsset(fontPathName);
            AssetDatabase.ImportPackage("temp.unitypackage", true);
            AssetDatabase.Refresh();

            Debug.Log("创建字体成功");
        }
        else
        {
            Debug.LogWarning("没有找到fnt文件，或者文件名称和图片名称不匹配");
        }
    }

}
