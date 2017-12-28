using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 정보
/// </summary>
public class Item
{
    private int m_OwnerId;      ///< 사용자 Id
    private TblItem m_TblItem;  ///< 테이블 데이터

    public TblItem TableData
    {
        get { return m_TblItem; }
    }
    public int OwnerId
    {
        get { return m_OwnerId; }
    }

    protected Item(TblItem _tblItem, int _ownerId)
    {
        m_TblItem = _tblItem;
        m_OwnerId = _ownerId;
    }

    public static Item Create(TblItem _tblItem, int _ownerId)
    {
        switch (_tblItem.UseType)
        {
            case eItemUseType.Use:
            case eItemUseType.Trap:
            case eItemUseType.Projectile:
                return new Item(_tblItem, _ownerId);
        }

        Log.PrintError(eLogFilter.Normal, "invalid parameter (ItemBase.Create)");
        return null;
    }

    /// <summary>
    /// 아이템 사용
    /// </summary>
    public void Use()
    {
        //. 인스턴스화가 필요한 아이템
        if (IsInstantiable())
        {
            ItemInstance.CreateFromItem(this);
        }
        //. 즉시 나한테 효과 발동
        else
        {
            TblItemEffect tblEffect = TableDataManager.Find<TblItemEffect>(m_TblItem.ItemEffectId);
            Player owner = ObjectManager.FindPlayer(m_OwnerId);
            if (owner != null && tblEffect != null)
                owner.AddStateEffect(tblEffect);
        }
    }

    /// <summary>
    /// 인스턴스화가 가능한가?
    /// </summary>
    private bool IsInstantiable()
    {
        switch (m_TblItem.UseType)
        {
            case eItemUseType.Trap:
            case eItemUseType.Projectile:
                return true;
        }
        return false;
    }
}
