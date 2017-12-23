using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 통해 GameObject로 Instance된 객체
/// </summary>
public class ItemInstance : MonoBehaviour
{
    private int m_OwnerId;              ///< 아이템 사용한 주체 Id
    private TblItemEffect m_TblEffect;  ///< 아이템 발동 효과

    public int OwnerId { set { m_OwnerId = value; } }
    public TblItemEffect TblEffect { set { m_TblEffect = value; } }
	
    void OnTriggerEnter(Collider _other)
    {
        //. 플레이어
        if(_other.tag == R.String.TAG_PLAYER)
        {
            //. 효과 발동
        }
        //. 장애물
        else if(_other.tag == R.String.TAG_OBSTACLE)
        {
            //. 파티클 넣을까?
            //. 즉시 사라진다. 
            Destroy(this);
        }
    }

    public static GameObject CreateFromItem(Item _item)
    {
        if (_item == null || _item.TableData == null)
            return null;

        //. 발사 주체 찾기
        Player owner = ObjectManager.FindPlayer(_item.OwnerId);
        if (owner == null)
        {
            Log.Print(eLogFilter.Item, string.Format("couldn't create item - failed to find owner id:{0}", _item.OwnerId));
            return null;
        }

        TblItem tblItem = _item.TableData;

        //. 상태 이펙트 데이터 찾기
        TblItemEffect tblEffect = TableDataManager.Find<TblItemEffect>(tblItem.ItemEffectId);
        if (tblEffect == null)
        {
            Log.Print(eLogFilter.Item, string.Format("couldn't create item - failed to find tblEffect id:{0}", tblItem.ItemEffectId));
            return null;
        }

        //. 나중에 최적화시에 선별 로딩이 될수도..
        //. Resource에서 Prefab 불러온다.
        GameObject prefab = ResourceManager.Instance.FindOrLoadItemPrefab(tblItem.Prefab);
        if (prefab == null)
        {
            Log.Print(eLogFilter.Item, string.Format("couldn't create item - failed to find prefab name:{0}", tblItem.Prefab));
            return null;
        }

        //. 인스턴스 생성
        GameObject instance = Instantiate(prefab, owner.FirePos);
        ItemInstance itemInstance = instance.GetComponent<ItemInstance>();

        //. 속성 설정
        itemInstance.OwnerId = owner.Id;
        itemInstance.TblEffect = tblEffect;
        return instance;
    }
}
