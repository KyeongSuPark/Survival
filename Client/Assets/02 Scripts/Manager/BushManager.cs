using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 부쉬들의 관리와 진입/탈출 이벤트를 관리하는 객체
/// </summary>
public class BushManager : MonoBehaviour {
    public static BushManager Instance = null;
    private int m_NextBushId;                               ///< 다음 발급될 부쉬 아이디
    private Dictionary<int, List<int>> m_EnteredPlayers;    ///< 부쉬에 들어간 플레이어 정보 <key-bushId, value-playerIds>
    void Awake()
    {
        m_EnteredPlayers = new Dictionary<int, List<int>>();
        m_NextBushId = R.Const.INVALID_BUSH_ID + 1;
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private List<int> FindOrAddPlayerIds(int _bushId)
    {
        List<int> playerIds = null;
        //. 있으면 가져오고
        if (m_EnteredPlayers.ContainsKey(_bushId))
        {
            playerIds = m_EnteredPlayers[_bushId];
        }
        //. 없으면 생성한다음에 추가한다.
        else
        {
            playerIds = new List<int>();
            m_EnteredPlayers.Add(_bushId, playerIds);
        }
        return playerIds;
    }

    /// <summary>
    /// 다음 유니크한 부쉬 아이디를 발급한다.
    /// </summary>
    public int GetNextBushId()
    {
        return m_NextBushId++;
    }

    /// <summary>
    /// _palyer가 _bush에 진입시 호출
    /// </summary>
    public void OnEnterBush(Player _player, Bush _bush)
    {
        //. 진입 정보에 추가
        List<int> enteredPlayerIds = FindOrAddPlayerIds(_bush.Id);
        enteredPlayerIds.Add(_player.Id);

        //. 내가 부쉬에 들어왔다?
        if (_player.IsLocal)
        {
            //. 부쉬를 반 투명하게 해준다.
            _bush.SetTransparent(R.Const.TRANSPARENT_OFFSET);

            //. 같은 부쉬에 있는 애들을 보여준다.
            foreach(var playerId in enteredPlayerIds)
            {
                Player player = ObjectManager.FindPlayer(playerId);
                if (player != null)
                    player.Stat.RemoveHiddenReason(eHiddenReason.Bush);
            }
        }
        //. 남이 부쉬에 들어왔다?
        else
        {
            //. 나랑 같은 부쉬인가??
            bool bSameBush = false;
            foreach (var playerId in enteredPlayerIds)
            {
                Player player = ObjectManager.FindPlayer(playerId);
                if (player != null && player.IsLocal)
                {
                    bSameBush = true;
                    break;
                }
            }

            //. 같은 부쉬가 아니면 남을 Hide 시켜준다.
            if (bSameBush == false)
                _player.Stat.AddHiddenReason(eHiddenReason.Bush);
        }
    }

    /// <summary>
    /// _palyer가 _bush에 탈출시 호출
    /// </summary>
    public void OnExitBush(Player _player, Bush _bush)
    {
        //. 진입 정보에 제거
        List<int> enteredPlayerIds = FindOrAddPlayerIds(_bush.Id);
        enteredPlayerIds.Remove(_player.Id);

        //. 내가 부쉬에 나갔다?
        if (_player.IsLocal)
        {
            //. 부쉬를 반 투명하게 해준다.
            _bush.SetTransparent(1.0f);

            //. 같은 부쉬에 있던 애들을 가려준다
            foreach (var playerId in enteredPlayerIds)
            {
                Player player = ObjectManager.FindPlayer(playerId);
                if (player != null)
                    player.Stat.AddHiddenReason(eHiddenReason.Bush);
            }
        }
        //. 남이 부쉬에 나갔다?
        else
        {
            //. 남을 Show 시켜준다.
            _player.Stat.RemoveHiddenReason(eHiddenReason.Bush);
        }
    }
}
