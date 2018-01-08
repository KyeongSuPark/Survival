using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주어진 값만큼 생명력 회복
/// </summary>
public class HealEffect : StateEffectBase
{
	// Use this for initialization
	void Start () {
        m_Stat.AddHp(m_TblEffect.Value);

        //. Todo 생명력 회복 이펙트
	}
}
