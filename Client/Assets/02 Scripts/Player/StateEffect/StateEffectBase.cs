using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEffectBase : MonoBehaviour {
    private static readonly string TIMER_INVOKE_FUNC = "OnTimerEnd";
    protected Player m_Owner;             ///< 상태효과가 적용되고 있는 플레이어
    protected PlayerStat m_Stat;          ///< Owner의 스탯
    protected TblItemEffect m_TblEffect;     ///< 발동될 이펙트 테이블

    protected virtual void Awake()
    {
        m_Owner = GetComponent<Player>();
        m_Stat = GetComponent<PlayerStat>();
    }

    void OnDestroy()
    {
        if (m_Owner)
            m_Owner.RemoveStateEffect(m_TblEffect.EffectType);
    }

    /// <summary>
    /// 지속 시간 완료되면 호출되는 함수
    /// 정리해야할 작업이 있다면 이 함수를 override 해서 사용하자. override 할때 base는 반드시 호출해야 한다.
    /// </summary>
    protected virtual void OnTimerEnd()
    {
        DestroyObject(this);
    }

    public static StateEffectBase Create(GameObject _owner, TblItemEffect _tblEffect)
    {
        StateEffectBase effect = null;
        //. Component 추가
        switch (_tblEffect.EffectType)
        {
            case eItemEffect.Fetter: effect = _owner.AddComponent<FetterEffect>();
                break;
            case eItemEffect.ChangeRps: effect = _owner.AddComponent<ChangeRpsEffect>();
                break;
            case eItemEffect.Hide: effect = _owner.AddComponent<HideEffect>();
                break;
            case eItemEffect.Heal: effect = _owner.AddComponent<HealEffect>();
                break;
            case eItemEffect.Shield: effect = _owner.AddComponent<ShieldEffect>();
                break;
            case eItemEffect.Transform: effect = _owner.AddComponent<TransformEffect>();
                break;
            case eItemEffect.ChangeSpeed: effect = _owner.AddComponent<ChangeSpeedEffect>();
                break;
            case eItemEffect.Analeptic: effect = _owner.AddComponent<AnalepticEffect>();
                break;
        }

        if(effect == null)
        {
            Log.PrintError(eLogFilter.Item, string.Format("couldn't create state effect >> invalid type {0}", _tblEffect.Id));
            return null;
        }

        //. 초기화
        effect.Init(_tblEffect);
        return effect;
    }

    /// <summary>
    /// 테이블 데이터로 부터 상태 이펙트를 초기화
    /// </summary>
    public virtual void Init( TblItemEffect _effect)
    {
        m_TblEffect = _effect;

        //. 타이머 설정
        if(m_TblEffect.Duration != 0)
        {
            float second = m_TblEffect.Duration * 0.001f;
            Invoke(TIMER_INVOKE_FUNC, second);
        }
        //. 타이머 만료됨을 바로 호출 해준다.
        else
        {
            OnTimerEnd();
        }
    }
    
    /// <summary>
    /// 타이머 Reset
    /// </summary>
    public void ResetTimer()
    {
        CancelInvoke(TIMER_INVOKE_FUNC);
        float second = m_TblEffect.Duration * 0.001f;
        Invoke(TIMER_INVOKE_FUNC, second);
    }
}
