using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주어진 신간 만큼 은신 효과
/// </summary>
public class HideEffect : StateEffectBase
{
	// Use this for initialization
	void Start () {
        m_Stat.AddHiddenReason(eHiddenReason.Item);
	}
	
	void OnDestroy()
    {
        m_Stat.RemoveHiddenReason(eHiddenReason.Item);
    }
}
