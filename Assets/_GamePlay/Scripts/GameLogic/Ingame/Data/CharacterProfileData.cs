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
    public int exp = 0;
    /// <summary>
    /// Weapon Inventory ID
    /// </summary>
    public int wp = -1;
    /// <summary>
    /// Armor Inventory ID
    /// </summary>
    public int arm = -1;
    /// <summary>
    /// Helmet Inventory ID
    /// </summary>
    public int hlm = -1;
    /// <summary>
    /// Helmet Inventory ID
    /// </summary>
    public int wg = -1;

    public List<SkillData> sds = new List<SkillData>();

    private int m_PreviousEXP;
    private int m_PreviousLevel;
    //private int m_TargetEXP;

    public void Init(CharacterType id) {
        cid = id;
    }
    public void Load() {
        m_PreviousLevel = lv;
        m_PreviousEXP = exp;
        // m_TargetEXP = GameData.Instance.GetLevelRequireEXP(lv + 1);
        // while (exp >= m_TargetEXP) {
        //     ConsumeEXP(m_TargetEXP);
        //     LevelUp();
        // }
        AddSkillData(new SkillData(11, 0));
        AddSkillData(new SkillData(21, 0));
        AddSkillData(new SkillData(31, 0));
    }
    // public void EquipWeapon(int inventoryID) {
    //     UnEquip(EquipmentType.WEAPON);
    //     wp = inventoryID;
    // }
    // public void EquipHelmet(int inventoryID) {
    //     UnEquip(EquipmentType.HELMET);
    //     hlm = inventoryID;
    // }
    // public void EquipArmor(int inventoryID) {
    //     UnEquip(EquipmentType.ARMOR);
    //     arm = inventoryID;
    // }
    // public void EquipWing(int inventoryID) {
    //     UnEquip(EquipmentType.WING);
    //     wg = inventoryID;
    // }
    // public void UnEquip(EquipmentType et) {
    //     switch (et) {
    //         case EquipmentType.WEAPON: {
    //                 if (wp >= 0) {
    //                     WeaponData wd = ProfileManager.MyProfile.GetWeaponByInventoryID(wp);
    //                     wd.Use(false);
    //                 }
    //             }
    //             break;
    //         case EquipmentType.HELMET: {
    //                 if (hlm >= 0) {
    //                     HelmetData wd = ProfileManager.MyProfile.GetHelmetByInventoryID(hlm);
    //                     wd.Use(false);
    //                 }
    //             }
    //             break;
    //         case EquipmentType.ARMOR: {
    //                 if (arm >= 0) {
    //                     ArmorData wd = ProfileManager.MyProfile.GetArmorByInventoryID(arm);
    //                     wd.Use(false);
    //                 }
    //             }
    //             break;
    //         case EquipmentType.WING: {
    //                 if (wg >= 0) {
    //                     WingData wd = ProfileManager.MyProfile.GetWingByInventoryID(wg);
    //                     wd.Use(false);
    //                 }
    //             }
    //             break;
    //     }
    // }
    public int GetPreviousEXP() {
        return m_PreviousEXP;
    }
    public int GetPreviousLevel() {
        return m_PreviousLevel;
    }

    // /// <summary>
    // /// 
    // /// </summary>
    // /// <returns>Inventory ID</returns>
    // public int GetEquippedWeaponInventoryID() {
    //     return wp;
    // }
    // public int GetEquippedWeaponID() {
    //     return ProfileManager.MyProfile.GetWeaponByInventoryID(wp).did;
    // }
    // public WeaponData GetEquippedWeaponData() {
    //     return ProfileManager.MyProfile.GetWeaponByInventoryID(wp);
    // }
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <returns>Inventory ID</returns>
    // public int GetEquippedHelmetInventoryID() {
    //     return hlm;
    // }
    // public int GetEquippedHelmetID() {
    //     return ProfileManager.MyProfile.GetHelmetByInventoryID(hlm).did;
    // }
    // public HelmetData GetEquippedHelmetData() {
    //     return ProfileManager.MyProfile.GetHelmetByInventoryID(hlm);
    // }
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <returns>Inventory ID</returns>
    // public int GetEquippedArmorInventoryID() {
    //     return arm;
    // }
    // public int GetEquippedArmorID() {
    //     return ProfileManager.MyProfile.GetArmorByInventoryID(arm).did;
    // }
    // public ArmorData GetEquippedArmorData() {
    //     return ProfileManager.MyProfile.GetArmorByInventoryID(arm);
    // }
    // /// <summary>
    // /// 
    // /// </summary>
    // /// <returns>Inventory ID</returns>
    // public int GetEquippedWingInventoryID() {
    //     return wg;
    // }
    // public int GetEquippedWingID() {
    //     if(wg != -1) {
    //         return ProfileManager.MyProfile.GetWingByInventoryID(wg).did;
    //     }
    //     return -1;
    // }
    // public WingData GetEquippedWingData() {
    //     return ProfileManager.MyProfile.GetWingByInventoryID(wg);
    // }
    public CharacterType GetCharacterType() {
        return cid;
    }
    public int GetLevel() {
        return lv;
    }
    // public int GetEXP() {
    //     return exp;
    // }
    // public void AddEXP(int amount) {
    //     exp += amount;
    //     while(exp >= m_TargetEXP) {
    //         ConsumeEXP(m_TargetEXP);
    //         LevelUp();
    //     }
    // }
    // public void ConsumeEXP(int amount) {
    //     exp -= amount;
    // }
    // public void LevelUp() {
    //     lv++;
    //     AddSkillPoint();
    //     //m_TargetEXP = GameData.Instance.GetLevelRequireEXP(lv + 1);
    // }
    // public BigNumber GetTotalHPFromItem() {
    //     HelmetData helmetData = GetEquippedHelmetData();
    //     BigNumber helmetHP = helmetData.GetHP();

    //     ArmorData armorData = GetEquippedArmorData();
    //     BigNumber armorHP = armorData.GetHP();

    //     WingData wingData = GetEquippedWingData();
    //     BigNumber wingHP = 0;
    //     if (wingData != null) {
    //          wingHP = wingData.GetHP();
    //     }
    //     return helmetHP + armorHP + wingHP;
    // }
    // public BigNumber GetTotalAttackFromItem() {
    //     WeaponData weaponData = GetEquippedWeaponData();
    //     return weaponData.GetAttackDamage();
    // }
    public string GetCharacterName() {
        return GameData.Instance.GetCharacterName(cid);
    }
    // public float GetPreviousEXPProgress() {
    //     int previousRequirementEXP = GameData.Instance.GetLevelRequireEXP(m_PreviousLevel + 1);
    //     return (float)m_PreviousEXP / (float)previousRequirementEXP;
    // }
    // public float GetCurrentEXPProgress() {
    //     int currentRequirementEXP = GameData.Instance.GetLevelRequireEXP(GetLevel() + 1);
    //     return (float)GetEXP() / (float)currentRequirementEXP;
    // }
    // public string GetCurrentEXPProgressString() {
    //     int currentRequirementEXP = GameData.Instance.GetLevelRequireEXP(GetLevel() + 1);
    //     string s = GetEXP() + "/" + currentRequirementEXP;
    //     return s;
    // }
    // public string GetCurrentEXPProgressString2() {
    //     int currentRequirementEXP = GameData.Instance.GetLevelRequireEXP(GetLevel());
    //     string s = currentRequirementEXP + "/" + currentRequirementEXP;
    //     return s;
    // }
    public bool IsGoodToUnlockWingSlot() {
        return GetLevel() >= 10;
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
    // public float GetStatValueFromSkillData(StatType statType) {
    //     for(int i = 0; i < sds.Count; i++) {
    //         SkillData skillData = sds[i];
    //         SkillOnBoard skillOnBoard = GameData.Instance.GetCharacterSkill(skillData.id, GetCharacterType());
    //         SkillConfig skillConfig = skillOnBoard.SkillConfig;
    //         if(skillConfig.statType == statType) {
    //             return skillConfig.GetValue(skillData.p);
    //         }
    //     }
    //     return 0;
    // }
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