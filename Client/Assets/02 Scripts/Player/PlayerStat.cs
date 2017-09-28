using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour {
    private bool m_bRecoveryStamina;                ///< 체력 회복 중인가?

    public float m_Accel;                            ///< 가속 Offset
    
    private List<eHiddenReason> m_HiddenReasons;    ///< hidden 되는 이유들
    private bool m_Ghost;                           ///< 유령 상태 (공격 하지도 받지도 못하는 상태) - 상태로 빼자
    private int m_Life;                             ///< 생명력
    private eRps m_Rps;                             ///< 현재 가위 바위 보
    private eRps m_SpecialRps;                      ///< 내가 선택한 필살 가위 바위 보

    public RangeFloatValue m_Velocity;              ///< 속도
    public RangeIntValue m_Stamina;                 ///< 체력

    public float Velocity {
        get { return m_Velocity.GetValue(); }
        set { m_Velocity.SetValue(value); }
    }

    public float MaxVelocity
    {
        get { return m_Velocity.m_Max; ; }
    }

    public int Stamina {
        get { return m_Stamina.GetValue(); }
    }

	// Use this for initialization
	void Start () {
        m_bRecoveryStamina = false;
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}

    /// <summary>
    ///   속도 증가
    /// </summary>
    public void IncreaseVelocity()
    {
        //Debug.Log(string.Format("inc {0:F3}", m_Velocity));
        m_Velocity.SetValue(m_Velocity.GetValue() + m_Accel * Time.deltaTime);
    }

    /// <summary>
    ///   지친 상태인가?
    /// </summary>
    public bool IsExhausted()
    {
        if (m_Stamina.GetValue() <= 0)
            return true;
        
        return false;
    }
}


