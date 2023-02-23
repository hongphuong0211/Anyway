using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SpawnType { INSTANCE = 0, TIMING, KILLED_COUNT, TYPE_TIMING, WAVE }

public delegate GameObject GetNextEnemy(Vector3 position);
public delegate bool IsOverEnemy();


public class SpawnPoint : MonoBehaviour {
    public SpawnType spawnType;
    public GameObject enemyPrefab;
    public GameObject markPoint;
    public int amount;
    public bool firstSpawn;
    public ParticleSystem SpawnEffect;
    public bool IsSpawned = false;
    public bool IsSpawning;

    protected GetNextEnemy m_GetNextEnemyMethod;
    protected IsOverEnemy m_IsOverEnemy;
    protected UnityAction<int> m_SpawnCallback;

    protected int currentSpawningAmount;

    public virtual void Spawn() {
    }
    
    public virtual void StartSpawn() {
        IsSpawning = true;
    }
    public virtual void StopSpawn() {
        IsSpawning = false;
    }
    public void RelayOnTriggerEnter() {
        StartSpawn();
    }
    public void SetGetEnemyMethod(GetNextEnemy method) {
        m_GetNextEnemyMethod = method;
    }
    public void SetSpawnCallback(UnityAction<int> method) {
        m_SpawnCallback = method;
    }
    public void SetIsOverEnemyMethod(IsOverEnemy method) {
        m_IsOverEnemy = method;
    }
    public virtual int GetEnemyCount() {
        return 0;
    }
    protected virtual GameObject GetNextEnemy() {
        if (m_GetNextEnemyMethod != null) {
            return m_GetNextEnemyMethod(transform.position);
        }
        return null;
    }
    public virtual bool IsOverEnemy() {
        if (currentSpawningAmount < amount) {
            return false;
        } else {
            return true;
        }
    }
}