using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState {
    public IdleState(Player _owner)
        : base(_owner)
    {
           
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Idle;
    }

    public override void OnStateEnter(StateChangeEventArg _arg = null)
    {
        m_Animator.SetInteger(R.AnimHash.STATE, (int)GetCode());
        m_Stat.Velocity = 0.0f;
    }

    public override void OnStateExit()
    {
        
    }

    public override void Update()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //. 입력이 있으면 Move 상태로
        if (moveDir.Equals(Vector3.zero) == false)
            m_Owner.ChangeState(ePlayerState.Move);
    }

    public override void PostUpdate()
    {
        
    }

    public override void FixedUpdate()
    {
    }

    public override void OnAnimationEvent(eAnimationEvent _eAnimEvent)
    {
    }
}
