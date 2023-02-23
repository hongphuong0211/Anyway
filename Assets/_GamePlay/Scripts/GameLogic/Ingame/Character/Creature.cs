using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : IngameObject {
    public Rigidbody2D Rigidbody { 
        get { 
            if(m_Rigidbody == null) {
                m_Rigidbody = GetComponent<Rigidbody2D>();
            }
            return m_Rigidbody; 
        }
    }

    protected static readonly int animatorIsRunningHash = Animator.StringToHash("IsRun");
    protected static readonly int animatorDie = Animator.StringToHash("Die");
    protected static readonly int animatorFire = Animator.StringToHash("Fire");
    protected static readonly int animatorIsInCombatHash = Animator.StringToHash("IsInCombat");
    protected static readonly int animatorSpeedHash = Animator.StringToHash("Speed");

    public Rigidbody2D m_Rigidbody;
    public Collider2D m_Collider;
    public Animator m_Animator;
    public List<SkinnedMeshRenderer> m_MeshRenderers = new List<SkinnedMeshRenderer>();
    public Shader m_ShaderNormal;
    public Color m_ShaderDamaged, m_ShaderOvercharge, m_ShaderShield;
    public bool IsPushBack;

    protected Transform m_Transform;
    protected NavMeshAgent m_AI;
    protected List<Buff> m_AddedBuffes = new List<Buff>();
    protected ChanceTable<int> m_CritChanceTable = new ChanceTable<int>();
    protected BigNumber m_PreviousHP;
    public Movement m_Movement;
    protected float m_BonusInDamage;
    protected float m_BonusOutDamage;
    protected float m_ReduceInDamage;
    protected float m_ReduceOutDamage;
    private float bonusBossDamage;
    private float bonusEvade;
    private float bonusShield;
    private float bonusCritical;
    private float bonusCriticalDamage;
    private float bonusDoubleShot;
    private float bonusExtraLife;
    
    protected float m_BonusSpeed;
    protected float m_BonusMaxHP;
    protected float m_BonusMaxShield;
    protected bool IsHoldingAttack;
    protected float m_TakeDamageEffectDuration = 0.1f;
    protected float m_TakeDamageImmunityDuration = 1;
    protected bool m_IsShowEffect = false;

    public virtual Movement Movement {
        get {
            return m_Movement;
        }
    }
    public float BonusBossDamage { get => bonusBossDamage; set => bonusBossDamage = value; }
    public float BonusEvade { get => bonusEvade; set => bonusEvade = value; }
    public float BonusShield { get => bonusShield; set => bonusShield = value; }
    public float BonusCritical { get => bonusCritical; set => bonusCritical = value; }
    public float BonusCriticalDamage { get => bonusCriticalDamage; set => bonusCriticalDamage = value; }
    public float BonusDoubleShot { get => bonusDoubleShot; set => bonusDoubleShot = value; }
    public float BonusExtraLife { get => bonusExtraLife; set => bonusExtraLife = value; }

    private void Awake() {
        m_Transform = transform;
        m_Collider = GetComponent<Collider2D>();
        Init();
    }
    public override void OnHit(BigNumber damage) {
        base.OnHit(damage);
        OnHit(damage, null);
    }
    public override void OnHit(BigNumber damage, List<StatConfig> stats) {
        OnHit(damage, stats, DamageType.NORMAL);
    }
    public override void OnHit(BigNumber damage, List<StatConfig> stats, DamageType damageType) {
        base.OnHit(damage, stats);
        if (BonusEvade > 0) {
            if (Random.Range(0, 100) < BonusEvade) {
                // Miss
                //IngameManager.Instance.PutHitStatusEffect(0, DamageType.NORMAL, Transform.position);
                return;
            }
        }
        OnShowEffect();
        if (IsDeactive()) return;
        damage = ExecuteDamage(damage, stats, ref damageType);
        OnDamaged(damage, damageType);
    }
    bool isCrit = false;
    public virtual BigNumber ExecuteDamage(BigNumber inputDamage, List<StatConfig> stats, ref DamageType damageType) {
        float bonusPer = 0;
        float critDamage = 0;
        isCrit = false;
        if(stats!= null) {
            for (int i = 0; i < stats.Count; i++) {
                StatConfig sc = stats[i];
                switch (sc.statType) {
                    case StatType.GROUND: {
                            if (m_LandType == LandType.GROUND) {
                                bonusPer += sc.value;
                            }
                        }
                        break;
                    case StatType.FLYING: {
                            if (m_LandType == LandType.FLYING) {
                                bonusPer += sc.value;
                            }
                        }
                        break;
                    case StatType.CRITICAL: {
                            int chance = (int)sc.value;
                            m_CritChanceTable.AddItem(1, chance);
                            m_CritChanceTable.AddItem(0, 100 - chance);
                            int roll = m_CritChanceTable.GetRandomItem();
                            if (roll == 1) {
                                isCrit = true;
                                damageType = DamageType.CRITICAL;
                            }
                        }
                        break;
                    case StatType.CRITICAL_DAMAGE: {
                            critDamage += sc.value;
                        }
                        break;
                    case StatType.POISON: {
                            damageType = DamageType.POISON;
                        }
                        break;
                    case StatType.FIRE: {
                            damageType = DamageType.FIRE;
                        }
                        break;
                    case StatType.ICE: {
                            damageType = DamageType.ICE;
                        }
                        break;
                }
            }
        }
        if (bonusPer > 0) {
            inputDamage = inputDamage + inputDamage *(bonusPer/ 100f);
        }
        if (isCrit) {
            critDamage += 50;
            inputDamage = inputDamage + inputDamage * (critDamage / 100f);
        }
        return inputDamage;
    }
    public override void OnDamaged(BigNumber damage, DamageType damageType) {
        base.OnDamaged(damage, damageType);
        //IngameManager.Instance.PutHitStatusEffect(damage, damageType, Transform.position);
        m_HP -= damage;
        if (m_HP <= 0) {
            Dead();
        }
    }
    public override bool IsDead() {
        return IsDeactive() || m_HP<=0;
    }
    public virtual void OnRunning() {
        m_TakeDamageEffectDuration -= Time.deltaTime;
        if(m_TakeDamageEffectDuration <= 0 && m_IsShowEffect) {
            OnDisableEffect();
        }
    }
    public virtual void OnBuffRunning() {
        int num = 0;
        while (num < m_AddedBuffes.Count) {
            Buff buff = m_AddedBuffes[num];
            if (!buff.IsLinkedBuff()) {
                buff.Update();
                if (buff.IsOverTime()) {
                    RemoveBuff(buff);
                    continue;
                }
            } else {
                if (buff.IsOwnerDeactive()) {
                    RemoveBuff(buff);
                    continue;
                }
            }
            num++;
        }
    }
    public void RefreshAllStats() {
        SetSlowRate(0);
        SetBonusIncomingDamage(0);
        SetBonusOutcomingDamage(0);
        SetReduceIncomingDamage(0);
        SetReduceOutcomingDamage(0);
        BonusBossDamage = 0;
        BonusEvade = 0;
        BonusShield = 0;
        BonusExtraLife = 0;
    }
    public override void RemoveBuff(Buff buff) {
        if (m_AddedBuffes.Contains(buff)){
            buff.EndBuff();
            m_AddedBuffes.Remove(buff);
        }
    }
    // public override int ApplyBuff(Buff buff) {
    //     int len = m_AddedBuffes.Count;
    //     for (int i = 0; i < len; i++) {
    //         Buff iB = m_AddedBuffes[i];
    //         if (iB.GetOwnerID() == buff.GetOwnerID() && iB.BuffType == buff.BuffType) {
    //             if (!iB.IsPernamentBuff) {
    //                 iB.SetTime(buff.m_AffectedTime);
    //             }
    //             return iB.ID;
    //         }
    //     }
    //     Buff newBuff;
    //     if (!buff.IsLinkedBuff()) {
    //         newBuff = GameManager.Instance.GetNewBuff(buff);
    //         newBuff.ApplyToOwner(this);
    //         m_AddedBuffes.Add(newBuff);
    //     } else {
    //         newBuff = buff;
    //         m_AddedBuffes.Add(buff);
    //     }
    //     newBuff.StartBuff();
    //     return newBuff.ID;
    // }
    public void ClearBuff() {
        while (m_AddedBuffes.Count > 0) {
            Buff buff = m_AddedBuffes[0];
            RemoveBuff(buff);
        }
    }
    public void AddSlowRate(float amount) {
        Movement.AddSlowRate(amount);
        float speedRate = 1 - Movement.slowRate / 100f;
        UpdateSpeedToAnimation(speedRate);
    }
    public void SubtractSlowRate(float amount) {
        Movement.SubtractSlowRate(amount);
        float speedRate = 1 - Movement.slowRate / 100f;
        UpdateSpeedToAnimation(speedRate);
    }
    public void SetSlowRate(float amount) {
        Movement.SetSlowRate(amount);
        float speedRate = 1 - Movement.slowRate / 100f;
        UpdateSpeedToAnimation(speedRate);
    }
    public void UpdateSpeedToAnimation(float rate) {
        if (m_Animator != null) {
            m_Animator.SetFloat(animatorSpeedHash, rate);
        }
    }
    public void AddBonusIncomingDamage(float amount) {
        m_BonusInDamage += amount;
    }
    public void SubtractBonusIncomingDamage(float amount) {
        m_BonusInDamage -= amount;
    }
    public void SetBonusIncomingDamage(float amount) {
        m_BonusInDamage = amount;
    }
    public virtual float GetBonusIncomingDamage() {
        return m_BonusInDamage;
    }
    public void AddBonusOutcomingDamage(float amount) {
        m_BonusOutDamage += amount;
    }
    public void SubtractBonusOutcomingDamage(float amount) {
        m_BonusOutDamage -= amount;
    }
    public void SetBonusOutcomingDamage(float amount) {
        m_BonusOutDamage = amount;
    }
    public virtual float GetBonusOutcomingDamage() {
        return m_BonusOutDamage;
    }
    public void AddReduceOutcomingDamage(float amount) {
        m_ReduceOutDamage += amount;
    }
    public void SubtractReduceOutcomingDamage(float amount) {
        m_ReduceOutDamage -= amount;
    }
    public void SetReduceOutcomingDamage(float amount) {
        m_ReduceOutDamage = amount;
    }
    public virtual float GetReduceOutcomingDamage() {
        return m_ReduceOutDamage;
    }
    public void AddReduceIncomingDamage(float amount) {
        m_ReduceInDamage += amount;
    }
    public void SubtractReduceIncomingDamage(float amount) {
        m_ReduceInDamage -= amount;
    }
    public void SetReduceIncomingDamage(float amount) {
        m_ReduceInDamage = amount;
    }
    public virtual float GetReduceIncomingDamage() {
        return m_ReduceInDamage;
    }
    public void AddBonusMaxHP(float amount) {
        m_BonusMaxHP += amount;
    }
    public void SubtractBonusMaxHP(float amount) {
        m_BonusMaxHP -= amount;
    }
    public void SetBonusMaxHP(float amount) {
        m_BonusMaxHP = amount;
    }
    public virtual float GetBonusMaxHP() {
        return m_BonusMaxHP;
    }
    public void AddBonusMoveSpeed(float amount) {
        m_Movement.AddBonusSpeed(amount);
    }
    public void SubtractBonusSpeed(float amount) {
        m_Movement.SubtractBonusSpeed(amount);
    }
    public void SetBonusSpeed(float amount) {
        m_Movement.SetBonusSpeed(amount);
    }
    public void SetImmunityTime(float time) {
        m_TakeDamageImmunityDuration = time;
    }
    public virtual void Dead() {
        ClearBuff();
    }
    public virtual void OnShowEffect() {
        m_IsShowEffect = true;
        m_TakeDamageEffectDuration = 0.25f;
        for(int i = 0;i< m_MeshRenderers.Count; i++) {
            SkinnedMeshRenderer sm = m_MeshRenderers[i];
            sm.material.SetColor("Shader_Color", m_ShaderDamaged);
            sm.material.SetFloat("FresnelOn", 1);
        }
    }
    public virtual void OnDisableEffect() {
        m_IsShowEffect = false;
        for (int i = 0; i < m_MeshRenderers.Count; i++) {
            SkinnedMeshRenderer sm = m_MeshRenderers[i];
            sm.material.SetFloat("FresnelOn", 0);
        }
    }
}