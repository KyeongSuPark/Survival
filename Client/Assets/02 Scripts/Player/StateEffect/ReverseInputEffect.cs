using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다른 플레이어 입력 반전 효과
/// </summary>
public class ReverseInputEffect : StateEffectBase
{
    protected override void Init(TblItemEffect _effect)
    {
        base.Init(_effect);
        if(m_Owner.IsLocal)
            InputManager.Reverse = true;
    }

    protected override void OnTimerEnd()
    {
        if (m_Owner.IsLocal)
            InputManager.Reverse = false;
        base.OnTimerEnd();
    }
}
