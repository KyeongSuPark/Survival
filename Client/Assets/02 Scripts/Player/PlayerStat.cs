using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour {
    public float m_Accel;                            ///< 가속 Offset
    public float m_OriginMaxVelocity;                ///< 능력치상 원래 최대 속도
    public RangeFloatValue m_Velocity;              ///< 속도
    public RangeIntValue m_Stamina;                 ///< 체력

    private bool m_bRecoveryStamina;                ///< 체력 회복 중인가?
    private bool m_bInvincible;                     ///< 무적인가?

    private List<eHiddenReason> m_HiddenReasons;    ///< hidden 되는 이유들
    private bool m_Ghost;                           ///< 유령 상태 (공격 하지도 받지도 못하는 상태) - 상태로 빼자
    private int m_Life;                             ///< 생명력
    private eRps m_Rps;                             ///< 현재 가위 바위 보
    private eRps m_SpecialRps;                      ///< 내가 선택한 필살 가위 바위 보

    private PlayerHud m_PlayerHud;                  ///< PlayerHud
    private Coroutine m_SuffleCoroutine;             ///< Shuffle 코루틴

    public int m_TempId;                            ///< 임시 아이디
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

    public int Id {
        get { return m_TempId; }
    }

    void Awake() {
        m_PlayerHud = GetComponent<PlayerHud>();
    }

	// Use this for initialization
	void Start () {
        m_bRecoveryStamina = false;
        m_bInvincible = false;
        m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));

        //. Test Value
        m_Life = 3;
	}
	
	// Update is called once per frame
	void Update () {
        //if(Input.GetButtonDown(R.String.INPUT_JUMP))
        //{
        //    Debug.Log("force collision");
        //    //. 3초 셔플
        //    StopSuffleCoroutine();
        //    m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
        //    //. 3초 무적
        //    StartCoroutine(SetInvincivle(R.Const.RESET_TIME));
        //}
	}

    void OnDestroy() 
    {
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider _other)
    {
        //. 다른 플레이어와 충돌했다면 
        PlayerStat otherStat = _other.GetComponent<PlayerStat>();
        if (otherStat != null)
        {
            //. 충돌 로그
            Log.Print(eLogFilter.Player, string.Format("On trigger me:{1} other:{2}", Id, otherStat.Id));
            //. 무적 로그
            if (otherStat.Invincible == true || Invincible == true)
            {
                string someOne = (Invincible == true) ? "me" : "other";
                Log.Print(eLogFilter.Player, string.Format("{0} is invincible", someOne));
                return;
            }

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
                    StopSuffleCoroutine();
                    m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
                    //. 3초 무적
                    StartCoroutine(SetInvincivle(R.Const.RESET_TIME));
                }              
            }

            Log.Print(eLogFilter.Player, string.Format("compare rps me:{0} other:{1} result:{2} life:{3}", m_Rps, otherStat.Rps, result, m_Life));
        }
    }

    /// <summary>
    ///   Suffle Coroutine을 멈춤
    /// </summary>
    private void StopSuffleCoroutine()
    {
        if (m_SuffleCoroutine != null)
        {
            StopCoroutine(m_SuffleCoroutine);
            Debug.Log(string.Format("코루틴 중단 {0}", coroutineId));
        }
        m_SuffleCoroutine = null;
    }

    private int coroutineId = 1;
    /// <summary>
    ///   입력된 시간동안 Shuffle 후 Rps 결정
    /// </summary>
    private IEnumerator ShuffleRps(float _duration)
    {
        Debug.Log(string.Format("코루틴 시작 {0}", ++coroutineId));

        //. Hud에 셔플 시작 알림
        m_PlayerHud.OnStartSuffle(_duration);
        yield return new WaitForSeconds(_duration);
     
        //. Rps 결정
        m_Rps = Utils.DecisionRps();

        //. Hud에 Rps 결정을 알림
        m_PlayerHud.OnDecisionRps(m_Rps);
        
        Log.Print(eLogFilter.Player, string.Format("ShuffleRps >> decision Rps {0}", m_Rps));

        //. reset cycle 시간 후에 다시 호출
        yield return new WaitForSeconds(R.Const.RESET_CYCLE);
        m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
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


