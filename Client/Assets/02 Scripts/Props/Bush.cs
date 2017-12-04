using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {
    private int m_Id;                      ///< 고유 아이디
    private Renderer m_Renderer;            ///< 부쉬 렌더러

    public int Id { get { return m_Id; } }
           
    void Awake()
    {
        m_Id = R.Const.INVALID_BUSH_ID;
        m_Renderer = GetComponent<Renderer>();
    }

	// Use this for initialization
	void Start () {
        if (m_Id == R.Const.INVALID_BUSH_ID)
            m_Id = BushManager.Instance.GetNextBushId();
	}
	
    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag != R.String.TAG_PLAYER)
            return;

        Player player = _other.GetComponent<Player>();
        if (player == null)
            return;

        BushManager.Instance.OnEnterBush(player, this);
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag != R.String.TAG_PLAYER)
            return;

        Player player = _other.GetComponent<Player>();
        if (player == null)
            return;

        BushManager.Instance.OnExitBush(player, this);
    }

    /// <summary>
    /// 투명하게 만들어준다.
    /// </summary>
    /// <param name="_alpha"></param>
    public void SetTransparent(float _alpha)
    {
        Color originColor = m_Renderer.material.color;
        m_Renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, _alpha);
    }
}
