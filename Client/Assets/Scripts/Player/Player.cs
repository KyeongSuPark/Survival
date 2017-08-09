using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody m_Rigidbody = null;
    private Animator m_Animator = null;     ///< 애니메이터
    private PlayerState m_State = null;     ///< 현재 상태

    private Dictionary<ePlayerState, PlayerState> m_StateCache = null;  ///< 플레이어 상태 캐쉬

    public Texture m_Tex;
	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material.shader = Shader.Find("Toon/Lit Outline");
        renderer.material.SetTexture("_Ramp", m_Tex);
        renderer.material.SetFloat("_Outline", 0.05f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
