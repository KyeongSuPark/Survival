using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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

    public eCountry Country{ get { return m_eCountry; } }
    public bool IsLocal { get { return m_bLocal; } }
    public int Id { get { return m_TempId; } }
    public PlayerStat Stat { get { return m_Stat; } }
    public Transform FirePos { get { return m_FirePos; } }

    void Awake()
    {
        ObjectManager.Instance.OnCreatePlayer(this);
    }

    void OnDestroy()
    {
        ObjectManager.Instance.OnDestroyPlayer(this);
    }

	// Use this for initialization
	void Start () {
        m_StateCache = new Dictionary<ePlayerState, PlayerState>();
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

        //. Test
        if (Input.GetButtonDown(R.String.INPUT_JUMP))
        {
            OnPickupItem(1);
            UseItem();
        }
	}

    void LateUpdate()
    {
        if (m_State != null)
            m_State.PostUpdate();
        
        CheckAnyState();
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
    ///   State 제약 없이 변경되어야 하는 상태 체크
    /// </summary>
    private void CheckAnyState()
    {
        if(Input.GetButtonDown(R.String.INPUT_JUMP))
            ChangeState(ePlayerState.Roll);
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
    }

    /// <summary>
    /// 캐릭터 Visibile 설정
    /// </summary>
    /// <param name="_visible"></param>
    public void SetVisible(bool _visible)
    {
        //. Renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(var renderer in renderers)
            renderer.enabled = _visible;

        //. Canvas
        Canvas[] canvases = GetComponentsInChildren<Canvas>();
        foreach (var canvas in canvases)
            canvas.enabled = _visible;

        //. Projector
        Projector projector = GetComponentInChildren<Projector>();
        projector.enabled = _visible;
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

        m_Item = Item.Create(tblItem, Id);
        //. Todo UI 처리 
    }
}
