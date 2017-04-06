using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class SVNUtil
{
    public static void SVNCommand(string command, string path)
    {
        string c = "/c TortoiseProc.exe /command:{0} /path:\"{1}\" ";
        c = string.Format(c, command, path);
        ProcessStartInfo info = new ProcessStartInfo("cmd.exe", c);
        info.WindowStyle = ProcessWindowStyle.Hidden;
        Process.Start(info);
    }

    [MenuItem("Assets/SVN/UpdateAll", false, 4)]
    public static void SVNUpdateAll()
    {
        SVNCommand("update", Application.dataPath);
    }

    [MenuItem("Assets/SVN/CommitAll", false, 3)]
    public static void SVNCommitAll()
    {
        SVNCommand("commit", Application.dataPath);
    }

    [MenuItem("Assets/SVN/Commit", false, 1)]
    public static void SVNCommit()
    {
        SVNCommand("commit", GetSelectObjectPath());
    }

    [MenuItem("Assets/SVN/Update", false, 2)]
    public static void SVNUpdate()
    {
        SVNCommand("update", GetSelectObjectPath());
    }

    [MenuItem("Assets/SVN/Log", false, 5)]
    public static void SVNLog()
    {
        SVNCommand("log", GetSelectObjectPath());
    }

    [MenuItem("Assets/SVN/Revert", false, 7)]
    public static void SVNRevert()
    {
        SVNCommand("revert", GetSelectObjectPath());
    }

    [MenuItem("Assets/SVN/CleanUp", false, 6)]
    public static void SVNCleanUp()
    {
        SVNCommand("cleanup", GetSelectObjectPath());
    }

    static string GetSelectObjectPath()
    {
        string path = string.Empty;
        for (int i = 0; i < Selection.objects.Length; i++) 
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.objects[i]);
            path += AssetsPathToFilePath(assetPath);
            path += "*";
            if(!Directory.Exists(assetPath)) 
            {
                path += AssetsPathToFilePath(assetPath) + ".meta";
                path += "*";
            }
        }
        return path;
    }

    static string AssetsPathToFilePath(string path)
    {
        string m_path = Application.dataPath;
        m_path = m_path.Substring(0, m_path.Length - 6);
        m_path += path;
        return m_path;
    }
}
