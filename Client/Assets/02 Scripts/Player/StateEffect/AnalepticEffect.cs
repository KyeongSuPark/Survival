using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스태미너 회복 효과
/// </summary>
public class AnalepticEffect : StateEffectBase
{

	// Use this for initialization
	void Start () {
        m_Stat.AddStamina(m_TblEffect.Value);
	
        //. Todo 이펙트 효과
	}
}
