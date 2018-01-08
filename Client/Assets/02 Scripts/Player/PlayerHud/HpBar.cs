using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
    public Image m_Bar;                         ///< Hp Bar
    public Image m_BarBackground;               ///< HP Bar Background
    public float m_BarActionTime;                ///< Hp Bar 연출 시간

    public Color[] m_HpColorStep;               ///< Hp 양에 따라 변화되는 색상 ( idx 0부터 변화 )
    private Coroutine m_BarCoroutine;           ///< Hp Bar 연출 코루틴
    private Coroutine m_BackgroundCoroutine;    ///< Bar Background 연출 코루틴
    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
        m_BarCoroutine = null;
        m_BackgroundCoroutine = null;
	}

    /// <summary>
    ///   Hp bar를 채우거나 비우는 연출
    /// </summary>
    /// <param name="_fill">0-1 사이의 정규화된 값</param>
    private IEnumerator FillHpBar(float _fill)
    {    
        float curTime = 0.0f;
        AnimationCurve actionCurve = AnimationCurve.EaseInOut(curTime, m_Bar.fillAmount, m_BarActionTime, _fill); 
        while(true)
        {
            if (curTime >= m_BarActionTime)
            {
                //. 연출 시간이 끝나면 강제로 값을 할당
                m_Bar.fillAmount = _fill;
                break;
            }

            //. fill amount 변경
            m_Bar.fillAmount = actionCurve.Evaluate(curTime);

            //. fill amount 양에 따라 색상 변경
            if (m_HpColorStep.Length == 3)
            {
                if (m_Bar.fillAmount > 0.65f)
                    m_Bar.color = m_HpColorStep[0];
                else if (m_Bar.fillAmount > 0.3f)
                    m_Bar.color = m_HpColorStep[1];
                else
                    m_Bar.color = m_HpColorStep[2];
            }

            curTime += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    ///   Hp bar background를 채우거나 비우는 연출
    /// </summary>
    /// <param name="_fill">0-1 사이의 정규화된 값</param>
    private IEnumerator FillHpBarBackground(float _fill)
    {
        //. Bar 연출이 70퍼센트 진행되었을 때 부터 실행
        yield return new WaitForSeconds(m_BarActionTime * 0.7f);

        float curTime = 0.0f;
        //. Bar 연출의 절반 시간 만큼 진행
        float actionTime = m_BarActionTime * 0.5f;
        AnimationCurve actionCurve = AnimationCurve.EaseInOut(curTime, m_BarBackground.fillAmount, actionTime, _fill);
        while (true)
        {
            if (curTime >= actionTime)
            {
                m_BarBackground.fillAmount = _fill;
                break;
            }

            //. fill amount 변경
            m_BarBackground.fillAmount = actionCurve.Evaluate(curTime);
            curTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    ///   Hp Bar 연출 멈춤
    /// </summary>
    private void StopHpBarAction()
    {
        if (m_BarCoroutine != null)
            StopCoroutine(m_BarCoroutine);
        m_BarCoroutine = null;
    }

    private void StopHpBarBackgroundAction()
    {
        if (m_BackgroundCoroutine != null)
            StopCoroutine(m_BackgroundCoroutine);
        m_BackgroundCoroutine = null;
    }

    /// <summary>
    /// fill 값을 변경하는 연출을 재생한다.
    /// </summary>
    /// <param name="_fill">0-1 사이의 정규화된 값</param>
	public void SetFillAmount(float _fill)
    {
        if(Utils.NearlyEquals(m_Bar.fillAmount,_fill) == false)
        {
            StopHpBarAction();
            StopHpBarBackgroundAction();

            //. 연출 시작
            m_BarCoroutine = StartCoroutine(FillHpBar(_fill));
            m_BackgroundCoroutine = StartCoroutine(FillHpBarBackground(_fill));
        }
    }

    /// <summary>
    /// fill 값에 연출을 사용하지 않고 바로 변경한다.
    /// </summary>
    /// <param name="_fill">0-1 사이의 정규화된 값</param>
    public void SetForceFillAmount(float _fill)
    {
        m_Bar.fillAmount = _fill;
        m_BarBackground.fillAmount = _fill;
    }
}
