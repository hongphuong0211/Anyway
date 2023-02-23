using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PrefabManager : Singleton<PrefabManager> {
    private Dictionary<CharacterType, GameObject> m_CharacterPrefabDic = new Dictionary<CharacterType, GameObject>();
    public List<CharacterPrefab> m_CharacterPrefabs = new List<CharacterPrefab>();

    private Dictionary<string, GameObject> m_IngameObjectPrefabDic = new Dictionary<string, GameObject>();
    public GameObject[] m_IngameObjectPrefabs;


    protected override void Awake() {
        base.Awake();
        //InitPrefab();
    }
    // public void InitPrefab() {
    //     for (int i = 0; i < m_IngameObjectPrefabs.Length; i++) {
    //         GameObject iPrefab = m_IngameObjectPrefabs[i];
    //         if (iPrefab == null) continue;
    //         string iName = iPrefab.name;
    //         try {
    //             m_IngameObjectPrefabDic.Add(iName, iPrefab);
    //         } catch (System.Exception) {
    //             continue;
    //         }
    //     }
    //     for (int i = 0; i < m_CharacterPrefabs.Count; i++) {
    //         CharacterPrefab iPrefab = m_CharacterPrefabs[i];
    //         if (iPrefab == null) continue;
    //         CharacterType prefabID = iPrefab.id;
    //         GameObject prefab = iPrefab.prefab;
    //         m_CharacterPrefabDic.Add(prefabID, prefab);
    //     }
    //     for (int i = 0; i < m_EnemyPrefabs.Length; i++) {
    //         GameObject iPrefab = m_EnemyPrefabs[i];
    //         if (iPrefab == null) continue;
    //         string iName = iPrefab.name;
    //         try {
    //             m_EnemyPrefabDic.Add(iName, iPrefab);
    //         } catch (System.Exception) {
    //             continue;
    //         }
    //     }
    //     for (int i = 0; i < m_BulletPrefabs.Length; i++) {
    //         GameObject iPrefab = m_BulletPrefabs[i];
    //         if (iPrefab == null) continue;
    //         string iName = iPrefab.name;
    //         try {
    //             m_BulletPrefabDic.Add(iName, iPrefab);
    //         } catch (System.Exception) {
    //             continue;
    //         }
    //     }
    //     for (int i = 0; i < m_DronePrefabs.Count; i++) {
    //         GameObject iPrefab = m_DronePrefabs[i];
    //         if (iPrefab == null) continue;
    //         string iName = iPrefab.name;
    //         try {
    //             m_DronePrefabDic.Add(iName, iPrefab);
    //         } catch (System.Exception) {
    //             continue;
    //         }
    //     }
    //     for (int i = 0; i < m_CourierPrefab.Count; i++) {
    //         GameObject iPrefab = m_CourierPrefab[i];
    //         if (iPrefab == null) continue;
    //         try {
    //             m_CourierPrefabDic.Add(i, iPrefab);
    //         } catch (System.Exception) {
    //             continue;
    //         }
    //     }
    //     //string ui_toast = "UIToolTip";
    //     //CreatePool(ui_toast, GetPrefabByName(ui_toast), 1);
    // }
    public void InitIngamePrefab() {
        
    }
    public GameObject GetPrefabByName(string name) {
        GameObject rPrefab = null;
        if (m_IngameObjectPrefabDic.TryGetValue(name, out rPrefab)) {
            return rPrefab;
        }
        return null;
    }
    // public bool HasPool(int instanceID) {
    //     return SimplePool.IsHasPool(instanceID);
    // }
    // public void ClearPools() {
    //     SimplePool.Release();
    // }
    // public void ReleasePools(GameUnit name) {
    //     SimplePool.Release(name);
    // }
    // public void CreatePool(string name, GameUnit prefab, int amount) {
    //     SimplePool.Preload(prefab, amount, name);
    // }
    public GameObject SpawnPool(GameUnit name) {
        if (SimplePool.IsHasPool(name.gameObject.GetInstanceID())) {
            GameObject go = SimplePool.Spawn(name, Vector3.zero, Quaternion.identity).gameObject;
            return go;
        }
        // } else {
        //     GameObject prefab = GetPrefabByName(name);
        //     if (prefab != null) {
        //         SimplePool.Preload(prefab, 1, name);
        //         GameObject go = SpawnPool(name);
        //         return go;
        //     }
        // }
        return null;
    }
    public GameObject SpawnPool(GameUnit name, Vector3 pos) {
        if (SimplePool.IsHasPool(name.gameObject.GetInstanceID())) {
            GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity).gameObject;
            return go;
        }
        return null;
    }
    // public GameObject SpawnEnemyPool(string name) {
    //     if (SimplePool.IsHasPool(name)) {
    //         GameObject go = SimplePool.Spawn(name, Vector3.zero, Quaternion.identity);
    //         return go;
    //     } else {
    //         GameObject prefab = GetEnemyPrefabByName(name);
    //         if (prefab != null) {
    //             SimplePool.Preload(prefab, 1, name);
    //             GameObject go = SpawnPool(name);
    //             return go;
    //         }
    //     }
    //     return null;
    // }
    // public GameObject SpawnEnemyPool(string name, Vector3 pos) {
    //     if (SimplePool.IsHasPool(name)) {
    //         GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
    //         return go;
    //     } else {
    //         GameObject prefab = GetEnemyPrefabByName(name);
    //         if (prefab != null) {
    //             SimplePool.Preload(prefab, 1, name);
    //             GameObject go = SpawnPool(name, pos);
    //             return go;
    //         }
    //     }
    //     return null;
    // }
    // public GameObject SpawnBulletPool(string name, Vector3 pos) {
    //     if (SimplePool.IsHasPool(name)) {
    //         GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
    //         return go;
    //     } else {
    //         GameObject prefab = GetBulletPrefabByName(name);
    //         if (prefab != null) {
    //             SimplePool.Preload(prefab, 1, name);
    //             GameObject go = SpawnPool(name, pos);
    //             return go;
    //         }
    //     }
    //     return null;
    // }
    // public GameObject SpawnBloodPool(Vector3 pos) {
    //     if (GameManager.Instance.GetBloodType() == BloodLifeType.NONE) return null;
    //     string name = "BloodSprite";
    //     if (SimplePool.IsHasPool(name)) {
    //         GameObject go = SimplePool.Spawn(name, pos, Quaternion.identity);
    //         go.transform.rotation = Quaternion.Euler(90, 0, 0);
    //         go.GetComponent<Blood>().Init();
    //         m_BloodSprite.Add(go);
    //         return go;
    //     } else {
    //         GameObject prefab = GetPrefabByName(name);
    //         if (prefab != null) {
    //             SimplePool.Preload(prefab, 1, name);
    //             GameObject go = SpawnPool(name, pos);
    //             go.transform.rotation = Quaternion.Euler(-90, 0, 0);
    //             go.GetComponent<Blood>().Init();
    //             m_BloodSprite.Add(go);
    //             return go;
    //         }
    //     }
    //     return null;
    // }
    // public void DespawnBlood() {
    //     for (int i = m_BloodSprite.Count - 1; i >= 0; i--) {
    //         GameObject go = m_BloodSprite[i];
    //         DespawnPool(m_BloodSprite[i]);
    //         m_BloodSprite.Remove(go);
    //     }
    // }
    public void DespawnPool(GameUnit go) {
        SimplePool.Despawn(go);
    }
    public void DespawnPool(GameUnit go, float delay) {
        StartCoroutine(WaitDespawn(go, delay));
    }
    IEnumerator WaitDespawn(GameUnit go, float delay) {
        yield return new WaitForSeconds(delay);
        SimplePool.Despawn(go);
    }
    public GameObject GetCharacterPrefabByID(CharacterType ct) {
        if (m_CharacterPrefabDic.ContainsKey(ct)) {
            return m_CharacterPrefabDic[ct];
        }
        return null;
    }    
}
[System.Serializable]
public class PrefabByID {
    public int id;
    public GameObject prefab;
}

[System.Serializable]
public class CharacterPrefab {
    public CharacterType id;
    public GameObject prefab;
}

[System.Serializable]
public class WeaponPrefabConfig {
    public int id;
    public string prefabName;
    public GameObject weapon;
}
