using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 속박 효과
/// </summary>
public class FetterEffect : StateEffectBase
{
	// Use this for initialization
	void Start () {
        m_Owner.ChangeState(ePlayerState.Abnormal);

        //. Todo 상태 이상 이펙트 넣으면 더 좋겠다.
	}
	
	protected override void OnTimerEnd()
    {
        m_Owner.ChangeState(ePlayerState.Idle);

        //. Todo 이펙트 제거

        base.OnTimerEnd();
    }
}
