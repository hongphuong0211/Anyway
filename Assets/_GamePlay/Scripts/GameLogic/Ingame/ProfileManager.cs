using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GamePlay;
using UnityEngine;
using UnityEngine.Events;

public class ProfileManager : Singleton<ProfileManager> {
    public static PlayerProfile MyProfile {
        get {
            return Instance.m_LocalProfile;
        }
    }
    private PlayerProfile m_LocalProfile;
    private UnityAction m_OnBuySuccess;

    private int m_SaveFailTime = 0;
    protected override void Awake() {
        base.Awake();
        InitProfile();
    }
    public void InitProfile() {
        CreateOrLoadLocalProfile();
        CloudCodeManager.Instance.AddScore(0);
    }
    private void CreateNewPlayer() {
        m_LocalProfile = new PlayerProfile();
        m_LocalProfile.CreateNewPlayer();
        SaveData(false);
    }
    private void LoadDataFromPref() {
        Debug.Log("Load Data");
        string dataText = PlayerPrefs.GetString("SuperFetch", "");
        Debug.Log("Data " + dataText);
        if (string.IsNullOrEmpty(dataText)) {
            // Dont have -> create new player and save;
            CreateNewPlayer();
        } else {
            // Have -> Load data
            LoadDataToPlayerProfile(dataText);
        }
    }
    private void LoadDataToPlayerProfile(string data) {
        m_LocalProfile = JsonMapper.ToObject<PlayerProfile>(data);
        m_LocalProfile.LoadLocalProfile();
    }
    private void Update() {
        if (m_LocalProfile != null) {
            m_LocalProfile.Update();
        }
    }
    #region Get Function
    private void CreateOrLoadLocalProfile() {
        Debug.Log("Create Or Load Data");
        LoadDataFromPref();
    }
    private string GetCacheFolderPath() {
        return Application.persistentDataPath + "/cache";
    }
    private string GetDataFolderPath() {
        return Application.persistentDataPath + "/atad";
    }
    public string GetPlayerCacheFile() {
        return Application.persistentDataPath + "/" + "pi.txt";
    }
    private string GetObfuscatePlayerFile() {
        return Application.persistentDataPath + "/atad/" + "mi.dat";
    }
    #endregion
    public void SaveData(bool isCheckCooldown = true) {
        m_LocalProfile.SaveDataToLocal(isCheckCooldown);
    }
    public void SaveDataText(string data) {
        if (!string.IsNullOrEmpty(data)) {
            //Debug.Log("Data " + data);
            PlayerPrefs.SetString("SuperFetch", data);
        }
    }
    
    
//     public void BuyIAP(string productID, UnityAction onBuySuccess = null) {
//         GameManager.Instance.ShowIAPLoading();
//         m_OnBuySuccess = onBuySuccess;
// #if UNITY_EDITOR
//         OnBuyIAPSuccess(productID);
// #else
//         Purchaser.Instance.BuyProduct(productID);
// #endif
//     }
//     public void OnBuyIAPFail(string storeSpecificId) {
//     }
//     public void OnApplicationPause(bool pause) {
//         if (pause) {
//             SaveData(false);
//         }
//     }
//     public void OnApplicationQuit() {
//         SaveData(false);
//     }
}
