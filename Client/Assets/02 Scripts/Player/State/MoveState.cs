using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState {
    private eMoveState m_eMoveState;    ///< 이동 상태
    private Vector3 m_TargetPos;        ///< 움직여야할 위치
    private Quaternion m_TargetRot;     ///< 움직여야할 회전값

    public MoveState(Player _owner)
            : base(_owner)
    {
        m_eMoveState = eMoveState.Run;
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Move;
    }

    public override void OnStateEnter(StateChangeEventArg _arg = null)
    {
        CheckMoveState();
        m_Animator.SetInteger(R.AnimHash.STATE, (int)GetCode());
        m_Animator.SetInteger(R.AnimHash.MOVE_STATE, (int)GetMoveState());
    }

    public override void OnStateExit()
    {
        
    }

    public override void Update()
    {
        //. 입력 값으로 이동
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_Transform.position += moveDir * m_Stat.Velocity * Time.deltaTime;

        //. 이동 시작, 입력 방향으로 선회
        if (moveDir.Equals(Vector3.zero) == false)
        {
            m_TargetRot = Quaternion.LookRotation(moveDir);
            m_Stat.IncreaseVelocity();
        }
        //. 입력이 없으면 Idle 상태로 넘어간다.
        else
        {
            m_Owner.ChangeState(ePlayerState.Idle);
        }         

        if (m_TargetRot.Equals(m_Transform.rotation) == false)
            m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, m_TargetRot, Time.deltaTime * m_Option.m_RotateLerpOffset);
    }

    public override void PostUpdate()
    {
        //. Input에 따라 MoveState 변경
    }

    public override void FixedUpdate()
    {
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
    }

    /// <summary>
    ///   입력으로 부터 이동 상태 반환
    /// </summary>
    private eMoveState GetMoveState()
    {
        return m_eMoveState;   
    }

    /// <summary>
    ///   이동 상태 검사 - 입력값에 따라 Move 상태도 변경한다.
    /// </summary>
    private void CheckMoveState()
    {
        if (Input.GetButtonDown(R.String.INPUT_RUN))
            ChangeMoveState(eMoveState.Run);
        else if (Input.GetButtonDown(R.String.INPUT_WALK))
            ChangeMoveState(eMoveState.Walk);
        else if (Input.GetButtonDown(R.String.INPUT_SNEAK))
            ChangeMoveState(eMoveState.Sneak);
    }

    private void ChangeMoveState(eMoveState _state)
    {
        if(m_eMoveState != _state)
        {
            Log.Print(eLogFilter.State, string.Format("change move state to [{0}] from [{1}]", _state, m_eMoveState));
            m_eMoveState = _state;
            OnChangedMoveState();
        }
    }

    /// <summary>
    ///   이동 상태 변경 이벤트
    /// </summary>
    private void OnChangedMoveState()
    {
        switch(GetMoveState())
        {
            case eMoveState.Run: m_Stat.MaxVelocity = m_Stat.OriginMaxVelocity;
                break;
            case eMoveState.Walk: m_Stat.MaxVelocity = m_Stat.OriginMaxVelocity * m_Option.m_WalkVelocityOffset;
                break;
            case eMoveState.Sneak: m_Stat.MaxVelocity = m_Stat.OriginMaxVelocity * m_Option.m_SneakVelocityOffset;
                break;
        }
    }
}
