using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 변신 가능한 목록 중 하나를 선택하여 변신한다.
/// </summary>
public class TransformEffect : StateEffectBase
{
    private SkinnedMeshRenderer m_Renderer; ///< Player의 renderer
    private Vector3 m_OriginScale;          ///< Player의 local scale
    private Mesh m_OriginMesh;              ///< Player의 mesh
    private Material m_OriginMaterial;      ///< Player의 material

    private MeshFilter m_TargetMeshFilter;  ///< 변신 대상 mesh filter
    private Material m_TargetMaterial;      ///< 변신 대상 material
    private Vector3 m_TargetScale;          ///< 변신 대상 local scale


    // Use this for initialization
    protected override void Awake()
    {
        //. 변신 가능한 오브젝트를 선택한다.
        GameObject target = ResourceManager.PickTransformableObject();
        if(target == null)
        {
            Log.PrintError(eLogFilter.Item, string.Format("TransformEffect.Start() >> target is null owner:{0}", m_Owner.Id));
            OnTimerEnd();
            return;
        }

        //. 타겟 정보
        MeshRenderer targetRenderer = target.GetComponent<MeshRenderer>();
        m_TargetMeshFilter = target.GetComponent<MeshFilter>();
        m_TargetMaterial = targetRenderer.material;
        m_TargetScale = target.transform.localScale;

        //. 원래 정보
        m_Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_OriginMaterial = m_Renderer.material;
        m_OriginMesh = m_Renderer.sharedMesh;
        m_OriginScale = transform.localScale;

        //. 타겟으로 변신
        TransformToTarget();
	}

    protected override void OnTimerEnd()
    {
        //. 원래대로 변신
        TransformToOrigin();
        base.OnTimerEnd();
    }

    /// <summary>
    /// 타겟으로 변신
    /// </summary>
    private void TransformToTarget()
    {
        m_Renderer.sharedMesh = Instantiate<Mesh>(m_TargetMeshFilter.mesh);
        m_Renderer.material = Instantiate<Material>(m_TargetMaterial);
        transform.localScale = m_TargetScale;
    }

    /// <summary>
    /// 다시 원래대로 변신
    /// </summary>
    private void TransformToOrigin()
    {
        m_Renderer.sharedMesh = m_OriginMesh;
        m_Renderer.material = m_OriginMaterial;
        transform.localScale = m_OriginScale;
    }
	
}
