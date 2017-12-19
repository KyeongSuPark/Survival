using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 테이블 데이터 Base
/// </summary>
public class TblBase
{
    private int m_Id;               ///< 테이블의 PrimaryKey

    public int Id { get { return m_Id; } }
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
    public int ItemEffectId;	 ///< 아이템 효과 Id                                
}

/// <summary>
/// 아이템 효과 테이블 데이터
/// </summary>
public class TblItemEffect : TblBase
{
    public string Name;              ///< 효과 이름
    public eItemEffect EffectType;   ///< 효과 종류
    public int Value;                ///< 종류별 사용할 값
}
