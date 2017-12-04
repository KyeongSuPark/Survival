using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    public static ObjectManager Instance = null;
    Dictionary<int, Player> m_Players;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_Players = new Dictionary<int, Player>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCreatePlayer(Player _player)
    {
        m_Players.Add(_player.Id, _player);
    }

    public void OnDestroyPlayer(Player _player)
    {
        m_Players.Remove(_player.Id);
    }

    public Player FindPlayer(int _playerId) 
    {
        if (m_Players.ContainsKey(_playerId) == false)
            return null;

        return m_Players[_playerId];
    }
}
