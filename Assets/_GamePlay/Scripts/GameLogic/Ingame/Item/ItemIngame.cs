using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngameItemType
{
    Health,
    Shield,
    Energy,
    Damage,
    Speed,
    Attack,
    Power,
    Gold,
    Misc,
    Recipe,
    Equipment,
}
public class IngameItem : IngameObject {
    public IngameItemType m_ItemType;
    public int m_Value;
    public int m_Value2;
    public string m_OwnerName;
    private bool walkCouriercanPick = true;

    public bool WalkCouriercanPick {
        get {
            return walkCouriercanPick;
        }

        set {
            walkCouriercanPick = value;
        }
    }

    public IngameItemType GetItemType() {
        return m_ItemType;
    }
    public void SetOwnerName(string ownerName) {
        m_OwnerName = ownerName;
    }
    public void SetValue(int value) {
        m_Value = value;
    }
    public int GetValue() {
        return m_Value;
    }
    public void SetValue2(int value2) {
        m_Value2 = value2;
    }
    public int GetValue2() {
        return m_Value2;
    }
    public virtual BigNumber GetAmount() {
        return 0;
    }
    public override void BattleCry() {
        base.BattleCry();
        //RegisterInScene();
    }
    public void Deactive() {
        //IngameEntityManager.Instance.RemoveEntity(this);
        PrefabManager.Instance.DespawnPool(this);
    }
    public virtual void Absorb(Transform t) {

    }
}