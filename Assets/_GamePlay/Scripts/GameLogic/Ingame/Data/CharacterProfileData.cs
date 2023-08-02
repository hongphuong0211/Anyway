using System.Collections.Generic;
using UnityEngine;
public class CharacterProfileData {
    /// <summary>
    /// Character ID
    /// </summary>
    public CharacterType cid = CharacterType.SURVIVOR;
    /// <summary>
    /// Character Level
    /// </summary>
    public int lv = 1;
    /// <summary>
    /// Skill Point
    /// </summary>
    public int sp = 0;
    /// <summary>
    /// Character EXP
    /// </summary>
    public int exp = -1;

    public List<SkillData> sds = new List<SkillData>();

    private int m_PreviousEXP;
    private int m_PreviousLevel;
    private int m_TargetEXP;
    public void Init(CharacterType id) {
        cid = id;
    }
    public void Load() {
        m_PreviousLevel = lv;
        m_PreviousEXP = exp;
        m_TargetEXP = GameData.Instance.GetLevelRequireEXP(lv + 1);
        while (exp >= m_TargetEXP) {
            ConsumeEXP(m_TargetEXP);
            LevelUp();
        }
        AddSkillData(new SkillData(11, 0));
        AddSkillData(new SkillData(21, 0));
        AddSkillData(new SkillData(31, 0));
    }
    
    public int GetPreviousEXP() {
        return m_PreviousEXP;
    }
    public int GetPreviousLevel() {
        return m_PreviousLevel;
    }
    public CharacterType GetCharacterType() {
        return cid;
    }
    public int GetLevel() {
        return lv;
    }
    public int GetEXP() {
        return exp;
    }
    public void AddEXP(int amount) {
        exp += amount;
        while(exp >= m_TargetEXP) {
            ConsumeEXP(m_TargetEXP);
            LevelUp();
        }
    }
    public void ConsumeEXP(int amount) {
        exp -= amount;
    }
    public void LevelUp() {
        lv++;
        AddSkillPoint();
        m_TargetEXP = GameData.Instance.GetLevelRequireEXP(lv + 1);
    }
    public string GetCharacterName() {
        return GameData.Instance.GetCharacterName(cid);
    }
    public void AddSkillData(SkillData data) {
        int id = -1;
        for (int i = 0; i < sds.Count; i++) {
            if (data.id == sds[i].id) {
                id = data.id;
            }
        }
        if (id == -1) {
            sds.Add(data);
        }
    }
    public SkillData GetSkillDataByID(int id) {
        SkillData sd = null;
        for (int i = 0; i < sds.Count; i++) {
            if (id == sds[i].id) {
                sd = sds[i];
            }
        }
        if (sd == null) {
            Debug.Log("Skill data null with id " + id);
            return null;
        } else {
            return sd;
        }
    }
    public List<SkillData> GetTotalSkillData() {
        return sds;
    }
    public int UpgradeSkillDataByID(int id) {
        int num = 0;
        for (int i = 0; i < sds.Count; i++) {
            if (id == sds[i].id && sds[i].p < 10) {
                sds[i].p++;
                num = sds[i].p;
                if(sds[i].p >= 10) {
                    if (sds[i].id == 11) {
                        AddSkillData(new SkillData(12, 0));
                    }
                    if (sds[i].id == 21) {
                        AddSkillData(new SkillData(22, 0));
                    }
                    if (sds[i].id == 31) {
                        AddSkillData(new SkillData(32, 0));
                    }
                    if (sds[i].id == 12) {
                        AddSkillData(new SkillData(13, 0));
                    }
                    if (sds[i].id == 22) {
                        AddSkillData(new SkillData(23, 0));
                    }
                    if (sds[i].id == 32) {
                        AddSkillData(new SkillData(33, 0));
                    }
                    if (sds[i].id == 13) {
                        AddSkillData(new SkillData(0, 0));
                    }
                    if (sds[i].id == 23) {
                        AddSkillData(new SkillData(0, 0));
                    }
                    if (sds[i].id == 33) {
                        AddSkillData(new SkillData(0, 0));
                    }
                }
            }
        }
        return num;
    }
    public void ResetSkillData() {
        sds.Clear();
    }
    public bool IsSkillPointAvailable() {
        return sp > 0;
    }
    public void SpendSkillPoint() {
        if (sp > 0) {
            sp--;
        }
    }
    public void AddSkillPoint(int amount = 1) {
        sp += amount;
    }
}
public class SkillData {
    public int id;
    public int p;
    public SkillData() { }
    public SkillData(int id, int p) {
        this.id = id;
        this.p = p;
    }
}