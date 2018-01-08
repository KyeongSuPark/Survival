using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDataManager : MonoBehaviour {
    public static TableDataManager Instance = null;
    public eTableType[] m_StartUpTables;                                ///< 시작 할때 로드해야되는 테이블
    private Dictionary<eTableType, TableDataContainer> m_Containers;    ///< 테이블 컨테이너
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_Containers = new Dictionary<eTableType, TableDataContainer>();
        
        //. startup 테이블 로드
        LoadStartUp();
    }

    /// <summary>
    /// 컨테이너 생성 후 로드 한다.
    /// 컨테이너 추가는 이 함수로..
    /// </summary>
    public void Load(eTableType _type)
    {
        //. 이미 추가되어 있으면 Pass
        if (m_Containers.ContainsKey(_type))
            return;

        TableDataContainer container = new TableDataContainer(_type);
        container.Load();
        m_Containers.Add(_type, container);
        Log.Print(eLogFilter.Table, string.Format("table load type:{0}", _type));
    }

    /// <summary>
    /// T로부터 타입을 유추하여 테이블 아이템 Find 한다.
    /// </summary>
    private T InternalFind<T>(int _id) where T : TblBase
    {
        eTableType eType = GetType(typeof(T));
        if (m_Containers.ContainsKey(eType) == false)
            return null;

        TableDataContainer container = m_Containers[eType];
        TblBase data = container.Find(_id);
        if (data is T)
            return (T)data;

        return null;
    }

    public static T Find<T>(int _id) where T : TblBase
    {
        return Instance.InternalFind<T>(_id);
    }

    public static TableDataContainer FindContainer(eTableType _type)
    {
        if(Instance.m_Containers.ContainsKey(_type))
            return Instance.m_Containers[_type];

        return null;
    }

    /// <summary>
    /// type 가지고 Table type을 반환한다.
    /// </summary>
    /// <param name="_type">system type</param>
    /// <returns>테이블 타입</returns>
    private eTableType GetType(Type _type)
    {
        if (_type == typeof(TblItem))
            return eTableType.Item;
        else if (_type == typeof(TblItemEffect))
            return eTableType.ItemEffect;
        return eTableType.Max;
    }

    /// <summary>
    /// Startup 테이블로 설정된 컨테이너들을 로드한다.
    /// </summary>
    private void LoadStartUp()
    {
        if (m_StartUpTables == null)
            return;

        foreach(var type in m_StartUpTables)
            Load(type);
    }
}
