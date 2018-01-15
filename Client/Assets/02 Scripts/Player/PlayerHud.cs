using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour {
    public GameObject[] m_Rps;              ///< Rps Sprite Object

    public Image m_CountryIcon;             ///< 국가 아이콘 Spirte
    public Text m_NickName;                 ///< 닉 네임 Text
    public HpBar m_HpBar;                   ///< HpBar
    public GameObject m_HudCanvas;          ///< Hud Canvas

    private Coroutine m_SuffleCoroutine;     ///< Suffle 코루틴
    private Coroutine m_FlickerCoroutine;   ///< 점멸 코루틴
    private Player m_Player;

	// Use this for initialization
	void Start () {
        m_SuffleCoroutine = null;
        m_FlickerCoroutine = null;
        m_Player = GetComponent<Player>();
        SetCountryIcon(m_Player.Country);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    ///   입력된 시간동안 Rps Sprite를 Suffle 시킨다.
    /// </summary>
    private IEnumerator ShuffleRps(float _duration)
    {
        eRps rpsIdx = eRps.Rock;
        float frequency = _duration / (float)R.Const.SHUFFLE_FREQUENCY;
        while(true)
        {
            //. UI 설정
            SetRpsSprite(rpsIdx);
            yield return new WaitForSeconds(frequency);

            //. count, index 갱신
            rpsIdx++;
            if (rpsIdx > eRps.Scissors)
                rpsIdx = eRps.Rock;
        }
    }

    /// <summary>
    ///   Rps Sprite 를 설정한다.
    /// </summary>
    private void SetRpsSprite(eRps _rps)
    {
        //. Rps UI 전부 deactive
        foreach (GameObject sprite in m_Rps)
            sprite.SetActive(false);

        //. 하나만 선택해서 active
        m_Rps[(int)_rps].SetActive(true);
    }

    /// <summary>
    ///   Rps Srpite를 점멸 시킨다.
    /// </summary>
    private IEnumerator FlickerRpsSprite(eRps _rps)
    {
        //. 몇 초 동안
        float frequency = 0.2f;

        //. 몇 번
        int count = 4;

        //. Active 된 Rps 찾기
        GameObject activeSprite = m_Rps[(int)_rps];
        while(true)
        {
            if (count <= 0)
                break;

            //. 주기 동안 hide
            activeSprite.SetActive(false);
            yield return new WaitForSeconds(frequency);

            //. 주기 동안 show
            activeSprite.SetActive(true);
            yield return new WaitForSeconds(frequency);
            count--;
        }
    }

    /// <summary>
    ///   셔플 UI 연출을 멈춤
    /// </summary>
    private void StopSuffleUI()
    {
        if (m_SuffleCoroutine != null)
            StopCoroutine(m_SuffleCoroutine);
        m_SuffleCoroutine = null;
    }

    /// <summary>
    ///   점멸 코루틴 멈춤
    /// </summary>
    private void StopFlickerUI()
    {
        if (m_FlickerCoroutine != null)
            StopCoroutine(m_FlickerCoroutine);
        m_FlickerCoroutine = null;
    }

    /// <summary>
    ///   Hp 변경 콜백
    ///   Hp 값 만큼 Hp bar guage를 변화 시킨다.
    /// </summary>
    public void OnChangedHp(RangeIntValue _Hp, int _delta)
    {
        float hpPercent = (float)_Hp.GetValue() / (float)_Hp.m_Max;
        m_HpBar.SetFillAmount(hpPercent);

        //. 데미지를 받은 거라면 UI 쉐이크
        if (_delta < 0)
        {
            //. 1초 동안 0.2만큼씩 Shake한다.
            Vector3 amp = new Vector3(0.2f, 0.2f, 0.0f);
            ShakePosition.StartShake(m_HudCanvas, amp, 1.0f, true);
        }
          
    }

    /// <summary>
    ///   Suffle이 시작 됬을 때 호출 
    /// </summary>
    public void OnStartSuffle(float _duration)
    {
        //. 진행중인 UI 연출들을 중단
        StopSuffleUI();
        StopFlickerUI();

        //. 입력된 기간동안 UI 연출 시작
        m_SuffleCoroutine = StartCoroutine(ShuffleRps(_duration));
    }

    /// <summary>
    ///   가위 바위 보가 결정됬을 때 호출
    /// </summary>
    public void OnDecisionRps(eRps _resultRps)
    {
        //. 진행중인 UI 연출들을 중단
        StopSuffleUI();
        StopFlickerUI();
        
        //. 결정된 Rps로 UI 설정
        SetRpsSprite(_resultRps);
        
        //. 점멸 시작
        m_FlickerCoroutine = StartCoroutine(FlickerRpsSprite(_resultRps));
    }

    /// <summary>
    ///   국가 아이콘을 셋팅 한다.
    ///   해당 국가 코드가 없으면 보이지 않는다.
    /// </summary>
    public void SetCountryIcon(eCountry _contry)
    {
        //. 국가 아이콘 셋팅
        Sprite icon = ResourceManager.Instance.FindOrLoadCountryIcon(_contry);
        if (icon == null)
        {
            m_CountryIcon.gameObject.SetActive(false);
            return;
        }
        m_CountryIcon.sprite = icon;
    }

    public void SetVisible(bool _visible)
    {
        m_HudCanvas.SetActive(_visible);
    }
}
