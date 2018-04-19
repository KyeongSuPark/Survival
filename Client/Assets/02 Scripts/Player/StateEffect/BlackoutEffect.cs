using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다른 플레이어 암전 효과 
/// </summary>
public class BlackoutEffect : StateEffectBase
{
    private Vector3 m_SpotLightOffset = new Vector3(0.0f, 5.0f, 0.0f);  ///< Spot light 상대 좌표
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

            //. UI 암전 효과를 준다.
            GameHud.SetBlackout(true);
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

            //. UI 암전 효과를 끈다.
            GameHud.SetBlackout(false);
        }

        base.OnTimerEnd();
    }
}
