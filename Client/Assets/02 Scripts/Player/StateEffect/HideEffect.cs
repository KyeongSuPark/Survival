using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주어진 신간 만큼 은신 효과
/// </summary>
public class HideEffect : StateEffectBase
{
    // Use this for initialization
    public override void Init(TblItemEffect _effect)
    {
        base.Init(_effect);
        m_Stat.AddHiddenReason(eHiddenReason.Item);
	}
	
    protected override void OnTimerEnd()
    {
        m_Stat.RemoveHiddenReason(eHiddenReason.Item);
        base.OnTimerEnd();
    }
}
