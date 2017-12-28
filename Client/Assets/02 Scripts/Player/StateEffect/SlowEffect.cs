using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : StateEffectBase
{
    private float m_OriginMaxVelocity;      ///< 원래의 최고 속도
	// Use this for initialization
	void Start () {
		//. 최고 속도를 기억해두자
        m_OriginMaxVelocity = m_Stat.MaxVelocity;
	}
	
	void OnDestroy()
    {
        //. 기억해둔 값으로 복원
        m_Stat.MaxVelocity = m_OriginMaxVelocity;
    }
}
