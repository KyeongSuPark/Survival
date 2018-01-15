using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager Instance = null;
    private bool m_bReverse;                        ///< 반전 입력 여부

    public static bool Reverse {
        get { return Instance.m_bReverse; }
        set { Instance.m_bReverse = value; }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_bReverse = false;
    }
	
    /// <summary>
    /// 이동 방향 반환
    /// </summary>
	public static Vector3 GetMoveDir()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //. 방향 반전
        if (Reverse)
            dir *= -1.0f;

        return dir;
    }
}
