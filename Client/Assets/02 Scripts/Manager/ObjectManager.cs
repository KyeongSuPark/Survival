using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    public static ObjectManager Instance = null;
    private Dictionary<int, Player> m_Players;

    public Dictionary<int, Player> Players { get { return m_Players; } } 

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_Players = new Dictionary<int, Player>();
        Player.PlayerDied += OnPlayerDied;
        Player.PlayerAwaked += OnPlayerAwaked;
    }

    void OnDestroy()
    {
        Player.PlayerDied -= OnPlayerDied;
        Player.PlayerAwaked -= OnPlayerAwaked;
    }

    private void OnPlayerAwaked(Player _player)
    {
        m_Players.Add(_player.Id, _player);
    }

    private void OnPlayerDied(int _playerId)
    {
        m_Players.Remove(_playerId);
    }	

    private Player InternalFindPlayer(int _playerId) 
    {
        if (m_Players.ContainsKey(_playerId) == false)
            return null;

        return m_Players[_playerId];
    }

    public static Player FindPlayer(int _playerId)
    {
        return Instance.InternalFindPlayer(_playerId);
    }
}
