using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePosition : MonoBehaviour
{
    private Vector3 m_OriginOffset; ///< Parent와의 원래 차이
    private Vector3 m_DecayFactor;  ///< 감쇠 Factor

    private Vector3 m_Amp;           ///< 얼마나 멀리 움직일건지 (진폭 - 양수로만 설정할것)
    private float m_Duration;         ///< 진동 시간
    private bool m_bDecay;           ///< 감쇠 유무 ( true면 진동 시간이 다될때 정지한다 )

    void OnDestroy()
    {
        StopAllCoroutines();
        transform.localPosition = m_OriginOffset;
    }

    /// <summary>
    ///   Shake Position 초기화 함수
    /// </summary>
    public void StartShake(Vector3 _amp, float _duration, bool _decay)
    {
        m_OriginOffset = transform.localPosition;
        m_Amp = _amp;
        m_Duration = _duration;
        m_bDecay = _decay;

        if(m_bDecay)
        {
            m_DecayFactor.x = m_Amp.x / _duration;
            m_DecayFactor.y = m_Amp.y / _duration;
            m_DecayFactor.z = m_Amp.z / _duration;
        }

        StartCoroutine(Shake(_amp, m_Duration, _decay));
    }

    /// <summary>
    ///   Shake 시작
    /// </summary>
    private IEnumerator Shake(Vector3 _amp, float _duraion, bool _decay)
    {
        while(true)
        {
            Vector3 offset;
            offset.x = Random.Range(-1.0f, 1.0f) * m_Amp.x;
            offset.y = Random.Range(-1.0f, 1.0f) * m_Amp.y;
            offset.z = Random.Range(-1.0f, 1.0f) * m_Amp.z;
            transform.localPosition = m_OriginOffset + offset;

            //. 진폭 감쇠 설정
            if (m_bDecay)
                m_Amp -= m_DecayFactor * Time.deltaTime;

            //. 시간 체크
            if (m_Duration <= 0)
                break;

            m_Duration -= Time.deltaTime;
            yield return null;
        }
        
        Destroy(this);
    }

    public static void StartShake(GameObject _target, Vector3 _amp, float _duraion, bool _decay)
    {
        ShakePosition instance = _target.AddComponent<ShakePosition>();
        instance.StartShake(_amp, _duraion, _decay);
    }

    public static void StartShake(GameObject _target, float _amp, float _duraion, bool _decay)
    {
        ShakePosition instance = _target.AddComponent<ShakePosition>();
        instance.StartShake(new Vector3(_amp, _amp, _amp), _duraion, _decay);
    }
}
