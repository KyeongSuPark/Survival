using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 변속 효과
/// </summary>
public class ChangeSpeedEffect : StateEffectBase
{
    private Animator m_Animator;            ///< Player의 animator
    private float m_OriginAnimSpeed;         ///< 원래 anim 재생 속도
    private float m_OriginMaxVelocity;       ///< 원래의 최고 속도
	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();

		//. 최고 속도를 기억해두자
        m_OriginMaxVelocity = m_Stat.MaxVelocity;

        //. 테이블값으로 제한
        float offset = m_TblEffect.Value * 0.01f; //. 100분율을 0 ~ 1 offset으로 변경
        m_Stat.MaxVelocity *= offset;

        //. anim speed 조절
        m_OriginAnimSpeed = m_Animator.speed;
        m_Animator.speed *= offset;
	}
	
    protected override void OnTimerEnd()
    {
        //. 기억해둔 값으로 복원
        m_Animator.speed = m_OriginAnimSpeed;
        m_Stat.MaxVelocity = m_OriginMaxVelocity;
        base.OnTimerEnd();
    }
}
