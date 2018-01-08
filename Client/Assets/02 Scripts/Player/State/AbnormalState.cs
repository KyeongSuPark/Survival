using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalState : PlayerState {
    public AbnormalState(Player _owner)
        : base(_owner)
    {
           
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Abnormal;
    }

    public override void OnStateEnter(StateChangeEventArg _arg = null)
    {
        m_Animator.SetInteger(R.AnimHash.STATE, (int)GetCode());
        m_Animator.SetTrigger(R.AnimHash.ABNORMAL);
        m_Stat.Velocity = 0.0f;
    }

    public override void OnStateExit()
    {
        
    }

    public override void Update()
    {
        
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
