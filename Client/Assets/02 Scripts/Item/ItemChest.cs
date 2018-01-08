using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 보물 상자
/// </summary>
public class ItemChest : MonoBehaviour
{
    /// <summary>
    /// 점유 정보
    /// </summary>
    class Possesion
    {
        public int m_PlayerId; ///< 점유한 플레이어
        public float m_Time;    ///< 점유한 시간(s)
    }

    private TblItem m_TblItem;                     ///< 결정된 아이템
    private float m_Timer;                          ///< 몇 초 동안 점유해야 아이템을 획득할수 있나?
    private bool m_bOpen;                          ///< 상자가 열렸나?
    private List<Possesion> m_Possessions;         ///< 점유 정보(어떤 플레이어가 몇초동안 점유하고 있나?) 

    private Animator m_Animator;

    public HpBar m_HpBar;                           ///< 타이머 바
    public Canvas m_HudCanvas;                      ///< Hud Canvas

    public int m_TestId;

	// Use this for initialization
	void Start () {
        m_bOpen = false;
        m_Animator = GetComponent<Animator>();
        m_Possessions = new List<Possesion>();

        //. 아이템 테이블 컨테이너에서 랜덤으로 하나 픽한다.
        TableDataContainer container = TableDataManager.FindContainer(eTableType.Item);
        Dictionary<int, TblBase>.KeyCollection keys = container.GetKeys();
        int randomIdx = Random.Range(0, keys.Count);

        //. Todo Test 후 삭제
        randomIdx = m_TestId;

        int tempIdx = 0;
        foreach(var itemKey in keys)
        {
            if(randomIdx == tempIdx)
            {
                m_TblItem = container.Find(itemKey) as TblItem;
                break;
            }

            ++tempIdx;
        }

        Player.PlayerDied += OnPlayerDied;
        m_Timer = 3.0f;
	}

    void OnDestroy()
    {
        Player.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied(int _playerId)
    {
        RemovePossession(_playerId);
    }

    void Update()
    {
        if (m_bOpen)
            return;

        //. 점유 정보에서 시간을 추가해준다.
        foreach(var possesion in m_Possessions)
        {
            possesion.m_Time += Time.deltaTime;

            //. Local 이면 Hud 갱신
            if (possesion.m_PlayerId == Player.LocalPlayerId)
            {
                float fillAmount = 1.0f - possesion.m_Time / m_Timer; //. normalized 값이라 1 - (0~1) 을 해준다.
                m_HpBar.SetForceFillAmount(fillAmount);
            }

            //. 점유 시간이 타이머 이상이면 아이템 획득
            if (possesion.m_Time >= m_Timer)
            {
                Player player = ObjectManager.FindPlayer(possesion.m_PlayerId);
                if (player == null)
                    continue;

                player.OnPickupItem(m_TblItem.Id);                
                m_bOpen = true;
                break;
            }
        }

        //. 열었으면 Open Animation 시켜준다.
        if (m_bOpen)
        {
            SetVisibleHud(false);
            m_Animator.SetTrigger(R.AnimHash.OPEN);
        }
    }
	
	void OnTriggerEnter(Collider _col)
    {
        if (_col.tag != R.String.TAG_PLAYER)
            return;

        Player player = _col.GetComponent<Player>();
        if (player == null)
            return;

        //. 이미 있다?? 
        if (ContainsPossesion(player.Id))
            return;

        //. 점유 정보에 추가한다. 
        AddPossession(player.Id);

        //. Local 이다?? - Hud를 보여준다.
        if (player.IsLocal)
            SetVisibleHud(true);
    }

    void OnTriggerExit(Collider _col)
    {
        if (_col.tag != R.String.TAG_PLAYER)
            return;

        Player player = _col.GetComponent<Player>();
        if (player == null)
            return;

        //. 점유 정보가 없다??
        if (ContainsPossesion(player.Id) == false)
            return;

        //. 점유 정보에서 삭제 한다.
        RemovePossession(player.Id);

        //. Local 이다?? - Hud를 가린다.
        if (player.IsLocal)
            SetVisibleHud(false);
    }

    private void SetVisibleHud(bool _visible)
    {
        m_HudCanvas.gameObject.SetActive(_visible);
    }

    private void RemovePossession(int _playerId)
    {
        for(int i = 0; i < m_Possessions.Count; ++i)
        {
            if(m_Possessions[i].m_PlayerId == _playerId)
            {
                m_Possessions.RemoveAt(i);
                return;
            }
        }
    }

    private bool ContainsPossesion(int _playerId)
    {
        foreach(var possesion in m_Possessions)
        {
            if (possesion.m_PlayerId == _playerId)
                return true;
        }
        return false;
    }

    private void AddPossession(int _playerId)
    {
        Possesion possession = new Possesion();
        possession.m_PlayerId = _playerId;
        possession.m_Time = 0.0f;
        m_Possessions.Add(possession);
    }

    /// <summary>
    /// 상자 Open Animation End
    /// </summary>
    public void OnOpenAnimEnd()
    {
        DestroyObject(gameObject, 1.0f);
    }
}
