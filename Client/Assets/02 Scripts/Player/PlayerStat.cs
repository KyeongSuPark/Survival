﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour {
    private bool m_bRecoveryStamina;                ///< 체력 회복 중인가?
    private bool m_bInvincible;                     ///< 무적인가?

    public float m_Accel;                            ///< 가속 Offset
    
    private List<eHiddenReason> m_HiddenReasons;    ///< hidden 되는 이유들
    private bool m_Ghost;                           ///< 유령 상태 (공격 하지도 받지도 못하는 상태) - 상태로 빼자
    private int m_Life;                             ///< 생명력
    private eRps m_Rps;                             ///< 현재 가위 바위 보
    private eRps m_SpecialRps;                      ///< 내가 선택한 필살 가위 바위 보

    public float m_OriginMaxVelocity;                ///< 능력치상 원래 최대 속도
    public RangeFloatValue m_Velocity;              ///< 속도
    public RangeIntValue m_Stamina;                 ///< 체력
    
    public float Velocity {
        get { return m_Velocity.GetValue(); }
        set { m_Velocity.SetValue(value); }
    }

    public float MaxVelocity
    {
        get { return m_Velocity.m_Max; ; }
        set { m_Velocity.m_Max = value; }
    }

    public float OriginMaxVelocity
    {
        get { return m_OriginMaxVelocity; }
    }

    public int Stamina {
        get { return m_Stamina.GetValue(); }
    }

    public float Accel {
        get { return m_Accel; }
    }

    public eRps Rps {
        get { return m_Rps; }
    }

    public eRps SpeciaRps {
        get { return m_SpecialRps; }
    }

    public bool Invincible {
        get { return m_bInvincible; }
    }

	// Use this for initialization
	void Start () {
        m_bRecoveryStamina = false;
        m_bInvincible = false;
        StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}

    void OnTriggerEnter(Collider _other)
    {
        //. 다른 플레이어와 충돌했다면 
        PlayerStat otherStat = _other.GetComponent<PlayerStat>();
        if (otherStat != null && otherStat.Invincible == false)
        {
            //. 가위 바위 보 싸움
            eRpsCompareResult result = Utils.CompareRps(m_Rps, otherStat.Rps);

            //. 졌을 때만 처리
            if(result == eRpsCompareResult.Lose)
            {
                //. special 가위 바위 보 처리
                if(otherStat.Rps == otherStat.SpeciaRps)
                {
                    //. 상대편 special 가위 바위 보 연출
                    //. 생명력 감소
                    m_Life -= R.Const.SPECIAL_DAMAGE;
                }
                else
                {
                    //. 생명력 감소
                    m_Life -= R.Const.NORMAL_DAMAGE;
                }

                //. 사망 처리
                if(m_Life <= 0)
                {
                    Player player = GetComponent<Player>();
                    player.Die();
                    StopAllCoroutines();
                }
                else
                {
                    //. 3초 셔플
                    StopCoroutine(ShuffleRps(0.0f));
                    StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
                    //. 3초 무적
                    StartCoroutine(SetInvincivle(R.Const.RESET_TIME));
                }              
            }

            Log.Print(eLogFilter.Player, string.Format("compare rps me:{0} other:{1} result:{2}", m_Rps, otherStat.Rps, result));
        }
    }

    /// <summary>
    ///   3초간 Shuffle 후 Rps 결정
    /// </summary>
    private IEnumerator ShuffleRps(float _duration)
    {
        //. Todo UI 연출

        yield return new WaitForSeconds(_duration);
        //. Rps 결정
        m_Rps = Utils.DecisionRps();
        Log.Print(eLogFilter.Player, string.Format("ShuffleRps >> decision Rps {0}", m_Rps));

        //. reset cycle 시간 후에 다시 호출
        yield return new WaitForSeconds(R.Const.RESET_CYCLE);
        StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
    }

    /// <summary>
    ///   입력된 시간동안 무적
    /// </summary>
    /// <param name="_duration">몇 초 동안?</param>
    public IEnumerator SetInvincivle(float _duration)
    {
        m_bInvincible = true;
        yield return new WaitForSeconds(_duration);
        m_bInvincible = false;
    }

    /// <summary>
    ///   속도 증가
    /// </summary>
    public void IncreaseVelocity()
    {
        //Debug.Log(string.Format("inc {0:F3}", m_Velocity));
        Velocity = Velocity + m_Accel * Time.deltaTime;
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


