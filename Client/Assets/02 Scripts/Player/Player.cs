using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody m_Rigidbody = null;
    private Animator m_Animator = null;     ///< 애니메이터
    private PlayerState m_State = null;     ///< 현재 상태

    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬

	// Use this for initialization
	void Start () {
        m_StateCache = new Dictionary<ePlayerState, PlayerState>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        //. Toon Rendering 잘 나오게 Ramp 설정
        SetRampTexture();

        //. 초기 스테이트 설정
        ChangeState(ePlayerState.Idle);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_State != null)
            m_State.Update();
	}

    void PostUpdate()
    {
        if (m_State != null)
            m_State.PostUpdate();
    }

    /// <summary>
    ///   Ramp Texture 설정
    /// </summary>
    private void SetRampTexture()
    {
        Texture litRampTexture = (Texture)Resources.Load(R.Path.RESOURCE_TOON_LIT_RAMP_TEX);
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material.shader = Shader.Find("Toon/Lit Outline");
        renderer.material.SetTexture("_Ramp", litRampTexture);
        renderer.material.SetFloat("_Outline", 0.05f);
    }

    private void OnAnimationEvent(eAnimationEvent _event)
    {
        if (m_State != null)
        {
            Debug.Log("anim event " + _event.ToString());
            m_State.OnAnimationEvent(_event);
        }
    }

    /// <summary>
    ///   플레이어 상태 변경
    /// </summary>
    public void ChangeState(ePlayerState _newState, StateChangeEventArg _arg = null)
    {
        if (m_State != null && m_State.GetCode() == _newState)
            return;

        //. 캐쉬 된게 있는지 검사
        PlayerState newUnitState = null;
        if (m_StateCache.ContainsKey(_newState))
        {
            newUnitState = m_StateCache[_newState];
        }
        else
        {
            //. 없으면 새로 만들고 캐쉬
            newUnitState = PlayerState.Create(this, _newState);
            m_StateCache.Add(newUnitState.GetCode(), newUnitState);
        }

        if (m_State != null)
            m_State.OnStateExit();

        Log.Print(eLogFilter.State, string.Format("change state to [{0}] from [{1}]", _newState, m_State));
        m_State = newUnitState;
        m_State.OnStateEnter(_arg);
    }
}
