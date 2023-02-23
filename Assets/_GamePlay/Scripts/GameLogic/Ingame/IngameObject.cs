using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectMaterialType {
    BODY,
    METAL,
    STONE,
    OTHER
}
public enum LandType { GROUND = 0, FLYING = 1 }
public class IngameObject : GameUnit, ITakenDamage {
    public static int m_NextID = 0;
    protected int m_ID;
    protected BigNumber m_MaxHP;
    protected BigNumber m_IncreaseHP;
    public BigNumber m_HP;
    protected BigNumber m_HpSaved;
    [HideInInspector]
    public bool IsHideOnRadar = false;
    public bool IsInSideCamera = false;
    public IngameType m_IngameType;
    public LandType m_LandType;
    public ObjectMaterialType m_MaterialType = ObjectMaterialType.BODY;
    public float m_Distance;
    protected IngameObject m_Target;
    [HideInInspector]
    public bool m_IsSpawnedInstance;
    public virtual void Init() {
    }
    public int ID {
        get {
            return m_ID;
        }
        set {
            m_ID = value;
        }
    }
    public IngameObject Target {
        get {
            return m_Target;
        }
    }
    // public virtual void RegisterInScene() {
    //     while (IngameEntityManager.Instance.GetEntityFromID(m_NextID) != null) {
    //         m_NextID++;
    //     }
    //     m_ID = m_NextID;
    //     IngameEntityManager.Instance.RegisterEntity(this);
    // }
    public void UnregisterInScene() {
        IngameEntityManager.Instance.UnregisterEntity(this);
    }
    public virtual void BattleCry(float delayTime) { }
    public virtual void BattleCry() { }
    public virtual void Reset() {
    }
    public virtual bool IsDead() {
        return true;
    }
    public virtual void OnHit(BigNumber damage) { }
    public virtual void OnHit(BigNumber damage, List<StatConfig> stats) { }
    public virtual void OnHit(BigNumber damage, List<StatConfig> stats, DamageType damageType) { }
    
    public void OnHealing(BigNumber hp) {
        m_HP += hp;
        if(m_HP > m_MaxHP) {
            m_HP = m_MaxHP;
        }
    }
    public void OnHPBuff(BigNumber hp, Vector3 buffPos, string info = "") { }
    public virtual void OnDamaged(BigNumber damage, DamageType damageType) { }
    public virtual int ApplyBuff(Buff buff) {
        return 0;
    }
    public virtual void RemoveBuff(Buff buff) { }
    // Get Function
    public IngameType IngameType {
        get { return m_IngameType; }
        set { m_IngameType = value; }
    }
    public LandType GetLandType() {
        return m_LandType;
    }
    public BigNumber GetMaxHP() {
        return m_MaxHP;
    }
    public BigNumber GetHP() {
        return m_HP;
    }
    public void SetHP(BigNumber hp) {
        m_HP = hp;
    }
    public void SetMaxHP(BigNumber hp) {
        m_MaxHP = hp;
    }
    public virtual void AddHP(BigNumber hp) {
        m_HP += hp;
    }
    public void AddMaxHP(BigNumber hp) {
        m_MaxHP += hp;
    }
    public virtual BigNumber GetDamage() {
        return 0;
    }
    public virtual void AddMaxHPPer(float per) {
        m_MaxHP = m_MaxHP + m_MaxHP * per / 100f;
    }
    public virtual void AddHPPer(float per) {
        m_HP = m_HP + m_HP * per / 100f;
    }
    public float GetCurrentHpPercentage() {
        BigNumber b = m_HP * (100f / m_MaxHP);
        return b.ToFloat();
    }
    public float GetCurrentHPProgress() {
        float f = (m_HP / m_MaxHP).ToFloat();
        return f;
    }
    public virtual bool IsDeactive() {
        bool isActive = gameObject.activeInHierarchy;
        return !isActive;
    }
    public virtual CharacterDataConfig GetCharacterDataConfig() {
        return null;
    }
    public virtual bool IsOutCamera() {
        if (Camera.main.WorldToViewportPoint(transform.position).y >= 1f ||
                          Camera.main.WorldToViewportPoint(transform.position).y <= 0f ||
                           Camera.main.WorldToViewportPoint(transform.position).x >= 1f ||
                           Camera.main.WorldToViewportPoint(transform.position).x <= 0f) return true;
        return false;
    }
    public virtual int GetSubType() {
        return -1;
    }
    public virtual void OnPushBack(Vector3 direct, float force) { }

    
}
public interface ITakenDamage {
    LandType GetLandType();
    void OnHit(BigNumber damage);
    void OnHit(BigNumber damage, List<StatConfig> stats);
    void OnHit(BigNumber damage, List<StatConfig> stats, DamageType damageType);
    void OnPushBack(Vector3 direct, float force);
    void OnHealing(BigNumber hp);
    void OnHPBuff(BigNumber hp, Vector3 buffPos, string info = "");
    int ApplyBuff(Buff buff);
    int GetSubType();
}

public class StatCofig{

}