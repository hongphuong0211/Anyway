using UnityEngine;
public class Effect : GameUnit {
    public float m_DefaultLifeTime = 2;
    protected float m_LifeTime = 3;
    public bool IsFollow = false;
    public Creature m_Owner;
    public virtual void Setup(float lifeTime) {
        m_LifeTime = lifeTime;
    }
    public void SetFollow(Creature owner) {
        m_Owner = owner;
        IsFollow = true;
    }
    public virtual void OnEnable() {
        m_LifeTime = m_DefaultLifeTime;
    }
    private void Update() {
        OnRunning();
    }
    public virtual void SetColor(Color c) {
    }
    public virtual void SetColor(int colorIndex) {
    }
    public virtual void OnRunning() {
        if (IsFollow) {
            Transform.position = m_Owner.Transform.position;
        }
        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime <= 0) {
            Deactive();
        }
    }
    public void Deactive() {
        m_Owner = null;
        IsFollow = false;
        if (gameObject.activeInHierarchy) {
            PrefabManager.Instance.DespawnPool(this);
        }
    }
}