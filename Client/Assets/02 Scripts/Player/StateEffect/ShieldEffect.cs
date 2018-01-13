using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : StateEffectBase
{
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        m_Stat.Shield = true;
        m_Stat.DecidedRpsCompare += OnDecidedRpsCompare;
	}

    protected override void OnDestroy()
    {
        m_Stat.Shield = false;
        m_Stat.DecidedRpsCompare -= OnDecidedRpsCompare;
        base.OnDestroy();
    }

    /// <summary>
    /// 가위바위보 결판이 났을 때 들어오는 함수
    /// </summary>
    private void OnDecidedRpsCompare(PlayerStat _winner, PlayerStat _loser)
    {
        //. Todo 실드 깨지는 이펙트

        //. 다음 Loop에 삭제시킨다.
        OnTimerEnd();
    }
}
