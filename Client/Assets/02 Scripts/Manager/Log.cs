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
    Table,          ///< 테이블
    Item,           ///< 아이템
}   

/// <summary>
/// Log를 관리 하는 객체
/// 현재는 필터만 적용
/// </summary>
public class Log {
    private static Log m_Instance = null;
    public static Log Instance 
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new Log();
            return m_Instance;
        }
    }
    public bool[] Filters
    {
        get { return DevelopOptions.LogFilter.Filters; }
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
