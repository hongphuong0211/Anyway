using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour {
    public Animator m_GateAnimator;
    public GameObject m_PassGate;
    public GameObject m_GateCollider;
    public GameObject m_GateTriggerCollider;
    public GameObject m_BakedLightPrefab;
    public Transform m_MapTopTrans, m_MapBottomTrans, m_MapLeftTrans, m_MapRightTrans;
    public float m_CameraMaxZ;
    public float m_CameraMinZ;
    public void OpenGate(bool isOpen) {
        m_PassGate.SetActive(isOpen);
        m_GateCollider.SetActive(!isOpen);
        m_GateTriggerCollider.SetActive(true);
        if (m_GateAnimator != null && isOpen) {
            m_GateAnimator.SetTrigger("Open");
        }
    }
    public void HideGateCollider() {
        m_GateTriggerCollider.SetActive(false);
    }
}
