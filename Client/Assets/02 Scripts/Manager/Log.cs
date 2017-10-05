using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 로그 필터
/// </summary>
public enum eLogFilter : int
{
    Normal,         ///< 일반 로그
    State,          ///< 상태
    AnimTrigger,    ///< 애님 트리거
    Player,         ///< 플레이어
}   

/// <summary>
/// Log를 관리 하는 객체
/// 현재는 필터만 적용
/// </summary>
public class Log : MonoBehaviour {
    public static Log Instance = null;
    public bool[] Filters
    {
        get { return DevelopOptions.LogFilter.Filters; }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Debug.LogError("Log has two instances");
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    } 

    public static void PrintError(eLogFilter _filter, object _message)
    {
        if (Instance.Filters[(int)_filter] == false)
            return;

        Debug.LogError("[" + _filter.ToString() + "]" + _message);
    }

    public static void Print(eLogFilter _filter, object _message)
    {
        if (Instance.Filters[(int)_filter] == false)
            return;

        Debug.Log("[" + _filter.ToString() + "]" + _message);
    }

    public static void Print(object _message)
    {
        Print(eLogFilter.Normal, _message);
    }
}
