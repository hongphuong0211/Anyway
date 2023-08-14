using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class PlayerProfile
{
    public string namePlayer { get; set; }
    public int lv { get; set; }
    public int exp { get; set; }
    public List<CharacterProfileData> m_ListCharacter { get; set; }
    
    public void Update(){}
    public void CreateNewPlayer()
    {
        namePlayer = "User" + DateTime.Now.Ticks;
        lv = 1;
        exp = 0;
        m_ListCharacter = new List<CharacterProfileData>();
        for (int i = 0; i < AttributeManager.Instance.m_SurvivorDatas.Count; i++)
        {
            CharacterProfileData data = new CharacterProfileData();
            data.lv = i == 0 ? 1 : 0;
            data.exp = 0;
            data.Init(CharacterType.SURVIVOR);
            m_ListCharacter.Add(data);
        }

        for (int i = 0; i < AttributeManager.Instance.m_HunterDatas.Count; i++)
        {
            CharacterProfileData data = new CharacterProfileData();
            data.lv = i == 0 ? 1 : 0;
            data.exp = 0;
            data.Init(CharacterType.HUNTER);
            m_ListCharacter.Add(data);
        }
    }
    public void LoadLocalProfile(){
        
    }

    public void SaveDataToLocal(bool isCheckCooldown = false)
    {
        if (isCheckCooldown)
        {
            string dataUser = JsonMapper.ToJson(this);
            ProfileManager.Instance.SaveDataText(dataUser);
        }
        else
        {
            string dataUser = JsonMapper.ToJson(this);
            ProfileManager.Instance.SaveDataText(dataUser);
        }
    }
    public CharacterProfileData GetCharacterProfile(int selectedCharacterID){
        return selectedCharacterID > -1 && selectedCharacterID < m_ListCharacter.Count? m_ListCharacter[selectedCharacterID]:null;
    }
    
}
