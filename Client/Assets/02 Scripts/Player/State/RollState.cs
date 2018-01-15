using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerState {
    private Vector3 m_Dir;              ///< 상태 진입 했을 때(입력 시) 방향

    public RollState(Player _owner)
        : base(_owner)
    {
        m_Dir = Vector3.zero;
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Roll;
    }

    public override void OnStateEnter(StateChangeEventArg _arg = null)
    {
        //. 상태 진입시 방향
        m_Dir = InputManager.GetMoveDir();
        //. 방향으로 바로 돌게 한다.
        m_Transform.LookAt(m_Transform.position + m_Dir);

        //. 구르기 시작 하자마자 최대 속도
        m_Stat.MaxVelocity = m_Stat.OriginMaxVelocity * m_Option.m_RollAccelOffset;
        m_Stat.Velocity = m_Stat.MaxVelocity;
        
        //. Animator Trigger 발동
        m_Animator.SetInteger(R.AnimHash.STATE, (int)GetCode());
        m_Animator.SetTrigger(R.AnimHash.ROLL);
    }

    public override void OnStateExit()
    {
        m_Dir = Vector3.zero;
    }
    public override void Update()
    {
        //. 입력 값으로 이동
        m_Transform.position += m_Dir * m_Stat.Velocity * Time.deltaTime;

        //. 점차 감소한다.
        m_Stat.MaxVelocity = m_Stat.OriginMaxVelocity * m_Option.m_RollAccelOffset * GetDecelOffset();
        m_Stat.Velocity = m_Stat.MaxVelocity;

        //AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(R.AnimLayer.BASE);
        //float offset = GetDecelOffset();
        //Debug.Log(string.Format("nTime:{0} offset:{1} velocity:{2}", stateInfo.normalizedTime, offset, m_Stat.Velocity));
    }

    public override void PostUpdate()
    {
        
    }

    public override void FixedUpdate()
    {
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
        if (_eAnimEvent == eAnimationEvent.AnimationEnd)
            m_Owner.ChangeState(ePlayerState.Idle);
    }

    /// <summary>
    ///   감속 offset 값
    /// </summary>
    private float GetDecelOffset()
    {
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(R.AnimLayer.BASE);
        if (stateInfo.IsName(R.String.ANIM_STATE_ROLL))
            return m_Option.m_RollDecelCurve.Evaluate(stateInfo.normalizedTime);
        return 1.0f;
    }
}
