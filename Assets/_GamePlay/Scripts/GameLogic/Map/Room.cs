using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType { WAVE_ROOM = 0, NORMAL_ROOM = 1, BOSS_ROOM = 2 }
public enum Difficulty { EASY = 0, MID, HARD };

[ExecuteInEditMode]
public class Room : MonoBehaviour {
    public int m_ID;
    public RoomType m_RoomType;

    public Transform m_StartPoint;
    public Transform m_EndPoint;
    public BaseMap m_BaseMap;

    public float m_Time;

    public Transform m_SpawnGroup;
    public List<SpawnPoint> m_SpawnPoints = new List<SpawnPoint>();

    protected int m_RequiredKillAmount = 0;
    protected int m_CurrentKillAmount = 0;

    public virtual void Init() {
        m_RequiredKillAmount = 0;
        m_Time = 0;
        m_BaseMap.OpenGate(false);

        //IngameManager.Instance.SetMaxY(m_BaseMap.m_CameraMaxZ);
        //IngameManager.Instance.SetMinY(m_BaseMap.m_CameraMinZ);
        IngameManager.Instance.m_MapTopY = m_BaseMap.m_MapTopTrans.position.z;
        IngameManager.Instance.m_MapBottomY = m_BaseMap.m_MapBottomTrans.position.z;
        IngameManager.Instance.m_MapMinX = m_BaseMap.m_MapLeftTrans.position.x;
        IngameManager.Instance.m_MapMaxX = m_BaseMap.m_MapRightTrans.position.x;
    }
    private void Update() {
        if (Application.isPlaying) {
            if (IngameManager.Instance.IsPause) return;
            OnRunning();
        } else {
            if (m_SpawnGroup != null) {
                if (m_SpawnGroup.childCount != m_SpawnPoints.Count) {
                    m_SpawnPoints.Clear();
                    foreach (Transform t in m_SpawnGroup) {
                        SpawnPoint sp = t.GetComponent<SpawnPoint>();
                        m_SpawnPoints.Add(sp);
                    }
                }
            }
        }
    }
    protected virtual void OnRunning() {
        m_Time += Time.deltaTime;
    }
    public void OpenGate() {
        m_BaseMap.OpenGate(true);
    }
    public void HideGate() {
        m_BaseMap.HideGateCollider();
    }
    public Transform GetRoomGate() {
        return m_BaseMap.m_PassGate.transform;
    }
    public virtual bool IsGoodToPass() {
        return false;
    }
    public virtual bool IsOverEnemy() {
        return false;
    }
    public virtual bool IsAllSpawnDone() {
        for (int i = 0; i < m_SpawnPoints.Count; i++) {
            SpawnPoint sp = m_SpawnPoints[i];
            if (!sp.IsOverEnemy()) {
                return false;
            }
        }
        return true;
    }
    public Transform GetNearestUnSpawnedPoint(Transform character) {
        Transform t = null;
        float distance = 99999999;
        List<SpawnPoint> list = GetUnSpawnedPoint();
        for(int i = 0; i < list.Count; i++) {
            SpawnPoint sp = list[i];
            if(t == null) {
                t = sp.transform;
                distance = (t.position - character.position).magnitude;
            } else {
                float num = (sp.transform.position - character.position).magnitude;
                if (num < distance) {
                    t = sp.transform;
                    distance = num;
                }
            }
        }
        return t;
    }
    public List<SpawnPoint> GetUnSpawnedPoint() {
        List<SpawnPoint> list = new List<SpawnPoint>();
        for (int i = 0; i < m_SpawnPoints.Count; i++) {
            SpawnPoint sp = m_SpawnPoints[i];
            if (!sp.IsSpawned) {
                list.Add(sp);
            }
        }
        return list;
    }
    // public virtual void MarkKill(int amount = 1) {
    //     m_CurrentKillAmount += amount;
    //     if (m_CurrentKillAmount >= m_RequiredKillAmount && !IngameEntityManager.Instance.HasAnyEnemyLive() && IsAllSpawnDone()) {
    //         IngameManager.Instance.ClearRoom();
    //     }
    // }
    public Vector3 GetSpawnPos() {
        return m_StartPoint.position;
    }
    public GameObject GetBakedLightPrefab() {
        return m_BaseMap.m_BakedLightPrefab;
    }
}