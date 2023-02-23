using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBind : Effect {
    public Vector3 m_Offset = new Vector3(0, 0, 0);
    protected Transform m_BindingPoint;
    public override void OnRunning() {
        if (m_Owner != null) {
            Transform.position = m_BindingPoint.position + m_Offset;
        }
    }
    public void SetOwner(Creature owner) {
        m_Owner = owner;
    }
    public void SetBindingPoint(Transform bindingPoint) {
        m_BindingPoint = bindingPoint;
    }
}
