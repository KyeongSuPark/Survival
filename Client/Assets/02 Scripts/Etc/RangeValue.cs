﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

/// <summary>
///   범위를 갖는 값을 표현하는 구조체
/// </summary>
public class RangeValue<T> where T : IComparable<T>
{
    public T m_Max;        ///< 최대 값
    public T m_Min;        ///< 최소 값
    //protected T m_Value;  
    public T m_Value;      ///< 현재 값 디버그용

    public T GetValue() { return m_Value; }
    public virtual void SetValue(T _value) { }
}
[Serializable]
public class RangeIntValue : RangeValue<int>
{
    /// <summary>
    ///   값 변경 delegate
    /// </summary>
    /// <param name="_value">this</param>
    /// <param name="_delta">변화량</param>
    public delegate void ChangeValueEventHandler(RangeIntValue _value, int _delta); ///< 값 변경 델리게이트
    public event ChangeValueEventHandler Changed;                                   ///< 값 변경 콜백 이벤트

    public override void SetValue(int _value)
    {
        int oldValue = m_Value;
        m_Value = Mathf.Clamp(_value, m_Min, m_Max);
        if (Changed != null && m_Value != oldValue)
            Changed(this, m_Value - oldValue);
    }

    public void Add(int _value)
    {
        SetValue(m_Value + _value);
    }

    public void Sub(int _value)
    {
        SetValue(m_Value - _value);
    }
}
[Serializable]
public class RangeFloatValue : RangeValue<float>
{
    public delegate void ChangeValueEventHandler(int _newValue);    ///< 값 변경 델리게이트
    public event ChangeValueEventHandler Changed;                   ///< 값 변경 콜백 이벤트

    public override void SetValue(float _value)
    {
        m_Value = Mathf.Clamp(_value, m_Min, m_Max);
    }

    public void Add(int _value)
    {
        SetValue(m_Value + _value);
    }

    public void Sub(int _value)
    {
        SetValue(m_Value - _value);
    }
}

/// <summary>
///   시간의 흐름에 따라 변경되는 값을 표현하는 구조체
/// </summary>
public class ProgressValue<T> : RangeValue<T> where T: IComparable<T>
{
    protected T m_Delta;       ///< 초당 변경되는 값
    protected float m_Time;     ///< 얼마 동안 (s)

    /// <summary>
    ///   타이머 설정
    /// </summary>
    public void SetTimer(float _totalTime)
    {
        m_Time = _totalTime;
    }

    /// <summary>
    ///   변화값 설정
    /// </summary>
    public void SetDelta(T _delta)
    {
        m_Delta = _delta;
    }

    /// <summary>
    ///   일정 주기마다 호출되는 갱신 함수
    /// </summary>
    public virtual void Update() { }
    
    /// <summary>
    ///   시간이 만료 되었나??
    /// </summary>
    public bool IsExpired()
    {
        if (m_Time <= 0)
            return true;
        return false;
    }
}

[Serializable]
public class ProgressIntValue : ProgressValue<int>
{
    public override void Update()
    {
        m_Time -= Time.deltaTime;
        if(IsExpired())
        {
            m_Time = 0.0f;
            return;
        }

        SetValue(m_Value + m_Delta);
    }
}
[Serializable]
public class ProgressFloatValue : ProgressValue<float>
{
    public override void Update()
    {
        m_Time -= Time.deltaTime;
        if (IsExpired())
        {
            m_Time = 0.0f;
            return;
        }

        SetValue(m_Value + m_Delta);
    }
}