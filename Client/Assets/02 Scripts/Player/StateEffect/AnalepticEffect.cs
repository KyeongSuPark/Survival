using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스태미너 회복 효과
/// </summary>
public class AnalepticEffect : StateEffectBase
{
    protected override void ApplyRightAway()
    {
        m_Stat.AddStamina(m_TblEffect.Value);

        //. Todo 이펙트 효과
    }
}
