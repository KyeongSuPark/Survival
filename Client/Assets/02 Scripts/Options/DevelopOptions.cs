using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif //. UNITY_EDITOR

/// <summary>
///   개발과 과련된 Option들 모음
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
#endif //. UNITY_EDITOR
public class DevelopOptions {
    public static LogFilterOption LogFilter = new LogFilterOption();
    static DevelopOptions()
    {
        LogFilter.Load();
    }

    public static bool Save()
    {
        bool result = true;
        //. log filter save
        if(LogFilter.Save() == false)
        {
            Debug.LogError("failed to save LogFilter");
            result = false;
        }
        return result;
    }
}

[Serializable]
public class LogFilterOption
{
    private string FilePath = R.Path.EDITOR_DATA + @"LogFilter.json";
    public bool[] Filters; 
    public LogFilterOption()
    {
    }

    public bool Save()
    {
#if UNITY_EDITOR
        if (Log.Instance == null)
            return false;

        Filters = Log.Instance.Filters;
        string str = JsonUtility.ToJson(this, true);
        File.WriteAllText(FilePath, str);
#endif //. UNITY_EDITOR
        return true;
    }
    public bool Load()
    {
#if UNITY_EDITOR
        if (Log.Instance == null)
            return false;

        if(File.Exists(FilePath))
        {
            string str = File.ReadAllText(FilePath);
            JsonUtility.FromJsonOverwrite(str, this);

            //. eLogFilter 갯수가 다르거나 없다면 다시 생성한다.
            if(Filters.Length == 0 ||
                Filters.Length != Enum.GetValues(typeof(eLogFilter)).Length)
                Filters = new bool[Enum.GetValues(typeof(eLogFilter)).Length];
        }
#endif //. UNITY_EDITOR
        return true;
    }
}