using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour {
    public GameObject[] m_Rps;              ///< Rps Sprite Object
    private Coroutine m_SuffleCoroutine;     ///< Suffle 코루틴
    private Coroutine m_FlickerCoroutine;   ///< 점멸 코루틴

	// Use this for initialization
	void Start () {
        m_SuffleCoroutine = null;
        m_FlickerCoroutine = null;
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
}
