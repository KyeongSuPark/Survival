using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가위바위보 변경
/// </summary>
public class ChangeRpsEffect : StateEffectBase
{
    protected override void ApplyRightAway()
    {
        m_Stat.ChangeRps();	
    }
}
