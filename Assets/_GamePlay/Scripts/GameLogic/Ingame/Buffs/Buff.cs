using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType { 
    NONE = 0, 
    ICE = 1,
    FIRE = 2,
    POISON = 3
}
[System.Serializable]
public class Buff {
    private static int nextID;
    protected int id;
    protected int ownerID;
    protected bool isLinkedBuff = false;
    public BigNumber damageOverTime;
    protected Creature owner;

    public int ID {
        get { return id; }
    }
    private BuffType buffType;
    public BuffType BuffType {
        get { return buffType; }
        set { buffType = value; }
    }
    protected float value;
    public float Value {
        get { return value; }
        set { this.value = value; }
    }
    public bool IsPernamentBuff;
    public float m_AffectedTime;
    public virtual void Update() {
        if (m_AffectedTime > 0) {
            m_AffectedTime -= Time.deltaTime;
        }
    }
    public void Init(BuffType buffType, float value, bool isLinkedBuff) {
        Init(buffType, value, 0, true, isLinkedBuff);
    }
    public void Init(BuffType buffType, float value, float affectedTime, bool isLinkedBuff) {
        Init(buffType, value, affectedTime, false, isLinkedBuff);
    }
    public void Init(BuffType buffType, float value, float affectedTime, bool isPernament, bool isLinkedBuff) {
        this.id = nextID;
        this.buffType = buffType;
        this.value = value;
        this.m_AffectedTime = affectedTime;
        this.IsPernamentBuff = isPernament;
        this.isLinkedBuff = isLinkedBuff;
        nextID++;
    }
    public void ApplyToOwner(Creature owner) {
        SetOwner(owner);
    }
    public void SetTime(float time) {
        m_AffectedTime = time;
    }
    public float GetTime() {
        return m_AffectedTime;
    }
    public void SetOwner(Creature owner) {
        this.owner = owner;
    }
    public void SetOwnerID(int ownerID) {
        this.ownerID = ownerID;
    }
    public int GetOwnerID() {
        return ownerID;
    }
    public bool IsOverTime() {
        return m_AffectedTime <= 0;
    }
    public bool IsLinkedBuff() {
        return isLinkedBuff;
    }
    public bool IsOwnerDeactive() {
        return owner.IsDeactive();
    }
    public virtual void StartBuff() {
    }
    public virtual void EndBuff() {
    }
}
// public class SlowBuff : Buff{
//     private Effect effect;
//     public SlowBuff() {
//         BuffType = BuffType.ICE;
//     }
//     public override void StartBuff() {
//         base.StartBuff();
//         owner.AddSlowRate(Value);
//         GameObject go = IngameManager.Instance.PutEffect(0, owner.Transform.position);
//         EffectBind eb = go.GetComponent<EffectBind>();
//         eb.Setup(m_AffectedTime + 0.5f);
//         eb.SetOwner(owner);
//         eb.SetBindingPoint(owner.Transform);
//         eb.Setup(m_AffectedTime);
//         effect = eb;
//     }
//     public override void EndBuff() {
//         base.EndBuff();
//         owner.SubtractSlowRate(Value);
//         effect.Deactive();
//     }
// }
// public class FireBuff : Buff {
//     private float currentCountTime;
//     private float damagedRate = 1;
//     private Effect effect;
//     public FireBuff() {
//         BuffType = BuffType.FIRE;
//     }
//     public override void StartBuff() {
//         base.StartBuff();
//         GameObject go = IngameManager.Instance.PutEffect(1, owner.Transform.position);
//         EffectBind eb = go.GetComponent<EffectBind>();
//         eb.Setup(m_AffectedTime + 0.5f);
//         eb.SetOwner(owner);
//         eb.SetBindingPoint(owner.Transform);
//         eb.Setup(m_AffectedTime);
//         effect = eb;
//     }
//     public override void Update() {
//         base.Update();
//         currentCountTime -= Time.deltaTime;
//         if (currentCountTime <= 0) {
//             currentCountTime = damagedRate + currentCountTime;
//             DealDamage();
//         }
//     }
//     private void DealDamage() {
//         if(!owner.IsDead())
//             owner.OnHit(damageOverTime, null, DamageType.FIRE);
//     }
//     public override void EndBuff() {
//         base.EndBuff();
//         effect.Deactive();
//     }
// }
// public class PoisonBuff : Buff {
//     public PoisonBuff() {
//         BuffType = BuffType.POISON;
//     }
    
//     private float currentCountTime;
//     private float damagedRate = 1;
//     private Effect effect;
//     public override void StartBuff() {
//         base.StartBuff();
//         owner.AddBonusOutcomingDamage(Value);
//         GameObject go = IngameManager.Instance.PutEffect(2, owner.Transform.position);
//         EffectBind eb = go.GetComponent<EffectBind>();
//         eb.Setup(m_AffectedTime + 0.5f);
//         eb.SetOwner(owner);
//         eb.SetBindingPoint(owner.Transform);
//         eb.Setup(m_AffectedTime);
//         effect = eb;
//     }
//     public override void Update() {
//         base.Update();
//         currentCountTime -= Time.deltaTime;
//         if (currentCountTime <= 0) {
//             currentCountTime = damagedRate + currentCountTime;
//             DealDamage();
//         }
//     }
//     public override void EndBuff() {
//         base.EndBuff();
//         owner.SubtractBonusOutcomingDamage(Value);
//         effect.Deactive();
//     }
//     private void DealDamage() {
//         if(!owner.IsDead())
//             owner.OnHit(damageOverTime,null, DamageType.POISON);
//     }
// }