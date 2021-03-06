﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static event PlayerDiedEventHandler PlayerDied;      ///< 플레이어 사망 이벤트
    public static event PlayerAwakedEventHandler PlayerAwaked;  ///< 플레이어 Awake 이벤트

    public static int LocalPlayerId = 0;    ///< LocalPlayer Id 

    private Rigidbody m_Rigidbody = null;
    private Animator m_Animator = null;     ///< 애니메이터
    private PlayerState m_State = null;     ///< 현재 상태
    private eCountry m_eCountry;            ///< 국가 코드
    private PlayerStat m_Stat;              ///< 스탯
    private Item m_Item;                    ///< 습득한 아이템 데이터
    [SerializeField]                                 
    private Transform m_FirePos;             ///< 발사체 위치
    public bool m_bLocal;
    public int m_TempId;
    //private bool m_bLocal;                  ///< Local Player 유무

    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬
    private Dictionary<eItemEffect, StateEffectBase> m_StateEffects = null;
    private List<ePlayerState> m_RestrictionStates = null;              ///< 제약 상태 (상태 변화가 일어나지 않도록 한다)

    public eCountry Country{ get { return m_eCountry; } }
    public bool IsLocal { get { return m_bLocal; } }
    public int Id { get { return m_TempId; } }
    public PlayerStat Stat { get { return m_Stat; } }
    public Transform FirePos { get { return m_FirePos; } }

    void Awake()
    {
        m_StateCache = new Dictionary<ePlayerState, PlayerState>();
        m_StateEffects = new Dictionary<eItemEffect, StateEffectBase>();
        m_RestrictionStates = new List<ePlayerState>();

        //. Notify
        PlayerAwaked(this);

        //. Local이면 Local Id를 저장한다.
        if (IsLocal)
            LocalPlayerId = Id;
    }

	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Stat = GetComponent<PlayerStat>();

        //. Toon Rendering 잘 나오게 Ramp 설정
        SetRampTexture();

        //. 초기 스테이트 설정
        ChangeState(ePlayerState.Idle);

        //. 테스트 값
        m_eCountry = eCountry.Korea;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_State != null)
            m_State.Update();

        if (IsLocal)
            LocalUpdate();
    }

    void LateUpdate()
    {
        if (m_State != null)
            m_State.PostUpdate();
    }

    private void LocalUpdate()
    {
        if (InputManager.GetButtonDown(R.Button.ITEM_USE))
            UseItem();

        if (InputManager.GetButtonDown(R.Button.JUMP))
            ChangeState(ePlayerState.Roll);
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

    /// <summary>
    /// 투명도 설정
    /// </summary>
    /// <param name="_alpha"> 0 ~ 1 사이 투명도 값</param>
    private void SetTransparent(float _alpha)
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material.shader = Shader.Find("Toon/BasicTransparent");
        Color originColor = renderer.material.color;
        renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, _alpha);
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

        //. 변경될 상태가 제약 상태에 있다면 상태 변경 시키지 않는다.
        if (m_RestrictionStates.Contains(_newState))
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

        Log.Print(eLogFilter.State, string.Format("change state [{1}] -> [{0}] ", _newState, m_State));
        m_State = newUnitState;
        m_State.OnStateEnter(_arg);
    }

    /// <summary>
    ///   사망
    /// </summary>
    public void Die()
    {
        Log.Print(eLogFilter.Player, string.Format("player die")); //. Todo 몇번 플레이어

        //. 사망 Animation
        m_Animator.SetTrigger(R.AnimHash.DIE);

        //. Collider Disable
        List<Collider> colliders = new List<Collider>();
        GetComponentsInChildren<Collider>(colliders);
        foreach(Collider col in colliders)
            col.enabled = false;

        //. UI 가린다.

        //. 사망 UI 

        //. Notify
        PlayerDied(Id);
    }

    /// <summary>
    /// 캐릭터 Visibile 설정
    /// </summary>
    /// <param name="_visible"></param>
    public void SetVisible(bool _visible)
    {
        //. local 이면 투명도를 준다.
        if(IsLocal)
        {
            //. 투명도 쉐이더 적용
            float alpha = _visible ? 1.0f : R.Const.TRANSPARENT_OFFSET;
            SetTransparent(alpha);

            //. 다시 원복
            if (_visible)
                SetRampTexture();
        }
        //. 아니면 전부 visible 설정
        else 
        {
            //. Renderer
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
                renderer.enabled = _visible;

            //. Canvas
            Canvas[] canvases = GetComponentsInChildren<Canvas>();
            foreach (var canvas in canvases)
                canvas.enabled = _visible;

            //. Projector
            Projector projector = GetComponentInChildren<Projector>();
            projector.enabled = _visible;
        }       
    }

    /// <summary>
    /// Hidden 상태를 갱신한다.
    /// </summary>
    public void UpdateHiddenState()
    {
        //. hidden 해야 할 이유가 하나라도 있으면 hide
        if (m_Stat.HasAnyHiddenReason())
            SetVisible(false);
        else
            SetVisible(true);
    }

    /// <summary>
    /// 현재 습득한 아이템을 사용한다.
    /// </summary>
    public void UseItem()
    {
        if (m_Item == null)
            return;

        Log.Print(eLogFilter.Item, string.Format("Player.UseItem >> player:{0} item:{1}", Id, m_Item.TableData.Id));
        m_Item.Use();
        //. Todo .UI 처리
        m_Item = null;  //. 사용했으면 지워준다.
    }

    /// <summary>
    /// 아이템을 줏었을 때 호출
    /// </summary>
    public void OnPickupItem(int _itemId)
    {
        TblItem tblItem = TableDataManager.Find<TblItem>(_itemId);
        if (tblItem == null)
            return;

        Log.Print(eLogFilter.Item, string.Format("Player.OnPickupItem >> player:{0} item:{1}", Id, _itemId));
        m_Item = Item.Create(tblItem, Id);
        //. Todo UI 처리 
    }

    /// <summary>
    /// 상태 효과 추가
    /// </summary>
    public void AddStateEffect(TblItemEffect _tblEffect)
    {
        eItemEffect effect = _tblEffect.EffectType;
        //. 가지고 있나??
        if(HasStateEffect(effect))
        {
            //. 시간만 리셋한다.
            m_StateEffects[effect].ResetTimer();
        }
        //. 없으면 새로 추가한다.
        else
        {
            StateEffectBase newEffect = StateEffectBase.Create(gameObject, _tblEffect);
            m_StateEffects.Add(effect, newEffect);
        }
    }

    /// <summary>
    /// 상태 효과 제거
    /// </summary>
    public void RemoveStateEffect(eItemEffect _effect)
    {
        m_StateEffects.Remove(_effect);
    }

    /// <summary>
    /// 상태효과를 가지고 있나??
    /// </summary>
    public bool HasStateEffect(eItemEffect _effect)
    {
        if (m_StateEffects.ContainsKey(_effect))
            return true;
        return false;
    }

    /// <summary>
    /// 제약 상태 추가
    /// </summary>
    /// <param name="_state">제약 될 상태</param>
    public void AddRestrictionState(ePlayerState _state)
    {
        if(m_RestrictionStates.Contains(_state) == false)
            m_RestrictionStates.Add(_state);
    }

    /// <summary>
    /// 제약 상태 제거
    /// </summary>
    /// <param name="_state">제약 해제할 상태</param>
    public void RemoveRestrictionState(ePlayerState _state)
    {
        m_RestrictionStates.Remove(_state);
    }

}
