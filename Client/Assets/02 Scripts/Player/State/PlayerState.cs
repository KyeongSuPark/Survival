using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerState
{
    protected Player m_Owner = null;            ///< 플레이어
    protected Rigidbody m_Rigidbody = null;     ///< 플레이어 강체
    protected Animator m_Animator = null;       ///< 애니메이터
    protected Transform m_Transform = null;     ///< Transform
    protected PlayerStat m_Stat = null;         ///< 플레이어 스탯
    protected PlayerOption m_Option = null;     ///< 플레이어 설정

    public PlayerState(Player _owner)
    {
        m_Owner = _owner;
        m_Animator = _owner.GetComponent<Animator>();
        m_Rigidbody = _owner.GetComponent<Rigidbody>();
        m_Stat = _owner.GetComponent<PlayerStat>();
        m_Option = _owner.GetComponent<PlayerOption>();
        m_Transform = _owner.gameObject.transform;
        if (m_Animator == null || m_Rigidbody == null || m_Stat == null || 
            m_Transform == null || m_Option == null)
        {
            Log.PrintError(eLogFilter.Normal, "failed to init player state");
            Debug.Break();
        }
    }

    /// <summary>
    /// 상태 생성 코드 
    /// 자식 클래스에서 스테이트 추가될게 있다면 여기서 변경
    /// </summary>
    public static PlayerState Create(Player _owner, ePlayerState _eUnitState)
    {
        switch (_eUnitState)
        {
            case ePlayerState.Idle: return new IdleState(_owner);
            case ePlayerState.Move: return new MoveState(_owner);
            case ePlayerState.Roll: return new RollState(_owner);
            case ePlayerState.Abnormal: return new AbnormalState(_owner);
        }

        Log.PrintError(eLogFilter.Normal, "invalid parameter (CreateUnitState)");
        return null;
    }

    public abstract ePlayerState GetCode();
    public virtual void OnStateEnter(StateChangeEventArg _arg = null) { }
    public virtual void OnStateExit() { }
    public virtual void Update() { }
    //public virtual void PreUpdate() { }
    public virtual void PostUpdate() { }

    public virtual void FixedUpdate() { }
    public virtual void OnAnimationEvent(eAnimationEvent _eAnimEvent) { }
}
