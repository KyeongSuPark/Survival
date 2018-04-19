using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
/// <summary>
/// 게임 Hud 
/// </summary>
public class GameHud : MonoBehaviour {
    public static GameHud Instance = null;

    [SerializeField]
    private GameObject m_Blackout = null;       ///< 암전 효과 img 를 갖는 오브제트
    void Awake()
    {
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

    /// <summary>
    /// 암전 효과 적용
    /// </summary>
    /// <param name="bVisible">적용 유무</param>
    public static void SetBlackout(bool bVisible)
    {
        Instance.m_Blackout.SetActive(bVisible);
    }
}
