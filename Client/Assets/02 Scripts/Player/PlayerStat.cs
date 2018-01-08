using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour {
    /// <summary>
    ///  Rps 대결 결과가 결정되었을 때 호출되는 이벤트
    /// </summary>
    public delegate void DecisionRpsCompareEventHandler(PlayerStat _winner, PlayerStat _loser);    
    public event DecisionRpsCompareEventHandler DecidedRpsCompare;                                           

    public float m_Accel;                            ///< 가속 Offset
    public float m_OriginMaxVelocity;                ///< 능력치상 원래 최대 속도
    public RangeFloatValue m_Velocity;              ///< 속도
    public RangeIntValue m_Stamina;                 ///< 체력
    public RangeIntValue m_Hp;                      ///< 생명력

    private bool m_bRecoveryStamina;                ///< 체력 회복 중인가?
    private bool m_bInvincible;                     ///< 무적인가?
    private bool m_bShield;                         ///< 쉴드중인가? (공격 받았을 때 무효화 처리)

    private List<eHiddenReason> m_HiddenReasons;    ///< hidden 되는 이유들
    private bool m_Ghost;                           ///< 유령 상태 (공격 하지도 받지도 못하는 상태) - 상태로 빼자
    private eRps m_Rps;                             ///< 현재 가위 바위 보
    private eRps m_SpecialRps;                      ///< 내가 선택한 필살 가위 바위 보

    private Player m_Owner;                         ///< Player
    private PlayerHud m_PlayerHud;                  ///< PlayerHud
    private Coroutine m_SuffleCoroutine;             ///< Shuffle 코루틴

    public eRps TestDesicionRps;   ///< 테스트 하기 편하게

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

    public bool Shield
    {
        get { return m_bShield; }
        set { m_bShield = value; }
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
        m_Owner = GetComponent<Player>();
        m_PlayerHud = GetComponent<PlayerHud>();
        m_HiddenReasons = new List<eHiddenReason>();
        m_Hp.Changed += new RangeIntValue.ChangeValueEventHandler(m_PlayerHud.OnChangedHp);
    }

	// Use this for initialization
	void Start () {
        m_bRecoveryStamina = false;
        m_bInvincible = false;
        m_bShield = false;
        m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));

        //. Test Value
        m_Hp.SetValue(5);
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
        //. 플레이어끼리 충돌 아니면 Return
        if (_other.tag != R.String.TAG_PLAYER)
            return;

        //. 다른 플레이어와 충돌했다면 
        PlayerStat otherStat = _other.GetComponent<PlayerStat>();
        if (otherStat != null)
        {
            //. 충돌 로그
            Log.Print(eLogFilter.Player, string.Format("On trigger player me:{0} other:{1}", Id, otherStat.Id));
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
                //. 콜백 이벤트
                if (DecidedRpsCompare != null)
                    DecidedRpsCompare.Invoke(otherStat, this);

                //. 실드가 있으면 처리 하지 않는다.
                if (m_bShield)
                    return;

                //. special 가위 바위 보 처리
                if(otherStat.Rps == otherStat.SpeciaRps)
                {
                    //. Todo 상대편 special 가위 바위 보 연출
                    //. 생명력 감소
                    m_Hp.Sub(R.Const.SPECIAL_DAMAGE);
                }
                else
                {
                    //. 생명력 감소
                    m_Hp.Sub(R.Const.NORMAL_DAMAGE);
                }

                //. 사망 처리
                if(m_Hp.GetValue() <= 0)
                {
                    Player player = GetComponent<Player>();
                    player.Die();
                    StopAllCoroutines();
                }
                else
                {
                    //. 가위바위보 변경(3초 셔플 3초 무적)
                    ChangeRps();
                }              
            }

            Log.Print(eLogFilter.Player, string.Format("compare rps me:{0} other:{1} result:{2} life:{3}", m_Rps, otherStat.Rps, result, m_Hp));
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
        // m_Rps = Utils.DecisionRps();
        m_Rps = TestDesicionRps;

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

    /// <summary>
    /// 가위바위보를 변경한다.
    /// 변경하는 동안은 무적 처리 한다.
    /// </summary>
    public void ChangeRps()
    {
        //. 3초 셔플
        StopSuffleCoroutine();
        m_SuffleCoroutine = StartCoroutine(ShuffleRps(R.Const.RESET_TIME));
        //. 3초 무적
        StartCoroutine(SetInvincivle(R.Const.RESET_TIME));
    }

    /// <summary>
    /// Hp를 회복 시킨다.
    /// </summary>
    public void AddHp(int _value)
    {
        m_Hp.Add(_value);
    }

    /// <summary>
    /// Stamina를 회복 시킨다.
    /// </summary>   
    public void AddStamina(int _value)
    {
        m_Stamina.Add(_value);
    }

    /// <summary>
    /// hidden reason 추가
    /// </summary>
    public void AddHiddenReason(eHiddenReason _reason)
    {
        //. 이미 있나?
        if (m_HiddenReasons.Contains(_reason))
            return;

        //. 없으면 추가 후 hidden 상태 갱신
        m_HiddenReasons.Add(_reason);
        m_Owner.UpdateHiddenState();
    }

    public void RemoveHiddenReason(eHiddenReason _reason)
    {
        //. 없으면 Nothing
        if (m_HiddenReasons.Contains(_reason) == false)
            return;

        //. 있으면 제거 후 hidden 상태 갱신
        m_HiddenReasons.Remove(_reason);
        m_Owner.UpdateHiddenState();
    }

    /// <summary>
    /// hidden reason이 하나라도 있나??
    /// </summary>
    public bool HasAnyHiddenReason()
    {
        if (m_HiddenReasons.Count > 0)
            return true;
        return false;
    }
}


