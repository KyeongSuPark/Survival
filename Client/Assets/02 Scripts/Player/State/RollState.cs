using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerState {
    public RollState(Player _owner)
        : base(_owner)
    {
           
    }

    public override ePlayerState GetCode()
    {
        return ePlayerState.Roll;
    }

    public override void OnStateEnter(StateChangeEventArg _arg = null)
    {
        
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
