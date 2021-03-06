﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 테이블 데이터 Base
/// </summary>
public class TblBase
{
    public int Id;               ///< 테이블의 PrimaryKey
}

/// <summary>
/// 아이템 테이블 데이터
/// </summary>
public class TblItem : TblBase
{
    public string Name;          ///< 아이템 이름
    public eItemUseType UseType; ///< 사용 방법
    public string Icon;          ///< 아이콘 이름
    public string Prefab;        ///< 프리팹 리소스 경로
    public eItemGrade Grade;     ///< 아이템 등급
    public int ItemEffectId;	 ///< 아이템 효과 Id      
                                 
    public TblItem()
    {
        Name = "";
        Icon = "";
        Prefab = "";
    }
}

/// <summary>
/// 아이템 효과 테이블 데이터
/// </summary>
public class TblItemEffect : TblBase
{
    public string Name;              ///< 효과 이름
    public eItemEffect EffectType;   ///< 효과 종류
    public int Duration;             ///< 효과가 유지되는 시간
    public int Value;                ///< 종류별 사용할 값
    
    public TblItemEffect()
    {
        Name = "";
    }
}
