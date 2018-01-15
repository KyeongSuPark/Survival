using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다른 플레이어 암전 효과 
/// </summary>
public class BlackoutEffect : StateEffectBase
{
    private Vector3 m_SpotLightOffset = new Vector3(0.0f, 5.0f, 0.0f);  ///< Spot light 상대 좌표
    private GameObject m_SpotLight;

    protected override void Init(TblItemEffect _effect)
    {
        base.Init(_effect);
        //.  로컬 일때만
        if(m_Owner.IsLocal)
        {
            //. 다른 플레이어의 Hud를 가린다.
            foreach(var pair in ObjectManager.Instance.Players)
            {
                Player player = pair.Value;
                if (player.Id == m_Owner.Id)
                    continue;

                PlayerHud playerHud = player.GetComponent<PlayerHud>();
                playerHud.SetVisible(false);
            }

            //. SpotLight를 가져와 Attach 한다.
            m_SpotLight = Instantiate(ResourceManager.FindOrLoadPrefab(R.String.TOP_SPOT_LIGHT_PREFAB));
            m_SpotLight.transform.parent = m_Owner.transform;
            m_SpotLight.transform.localPosition = m_SpotLightOffset;

            //. Sun을 Diable 시켜준다.
            ResourceManager.Instance.Sun.enabled = false;
        }
    }

    protected override void OnTimerEnd()
    {
        //.  로컬 일때만
        if (m_Owner.IsLocal)
        {
            //. 다른 플레이어의 Hud를 보여준다.
            foreach (var pair in ObjectManager.Instance.Players)
            {
                Player player = pair.Value;
                if (player.Id == m_Owner.Id)
                    continue;

                PlayerHud playerHud = player.GetComponent<PlayerHud>();
                playerHud.SetVisible(true);
            }

            //. SpotLight를 가져와 dettach 한다.
            m_SpotLight.transform.parent = null;
            DestroyObject(m_SpotLight);

            //. Sun을 enable 시켜준다.
            ResourceManager.Instance.Sun.enabled = true;
        }

        base.OnTimerEnd();
    }
}
