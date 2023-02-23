using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentConfigData {
    public int id;
    public CharacterType characterType;
    public EquipmentType equipmentType;
    public Rarity rarity;
    public string name;
    public List<StatConfig> stats = new List<StatConfig>();
    public void Init(int id, CharacterType characterID, EquipmentType deviceType, Rarity rarity) {
        this.id = id;
        this.characterType = characterID;
        this.equipmentType = deviceType;
        this.rarity = rarity;
    }
    public Rarity GetRarity() {
        return rarity;
    }
    public EquipmentType GetEquipmentType() {
        return equipmentType;
    }
    public CharacterType GetCharacterType() {
        return characterType;
    }
    public string GetSpriteName() {
        string s = GameData.Instance.GetEquipmentItemSpriteName(id, equipmentType);
        return s;
    }
    public string GetItemName() {
        return name;
    }
    public virtual StatConfig GetStatConfigByRank(int rank) {
        return stats[rank-1];
    }
}
public struct StatConfig {
    public StatType statType;
    public float value;
}
[System.Serializable]
public class StatSprite {
    public StatType statType;
    public Sprite sprite;
}
public enum EquipmentType { NONE = -1, WEAPON = 0, HELMET = 1, ARMOR = 2, WING = 3, DRONE = 4 }
public enum Rarity { COMMON = 1, RARE = 2, EPIC = 3, LEGEND = 4}
public enum StatType { 
    NONE = 0, 
    HP = 1,
    ATK = 2,
    MOVE_SPEED = 3, 
    RATE_OF_FIRE = 4,
    PUSH_BACK = 5,
    ICE = 6,
    FIRE = 7,
    DAMAGE_TO_BOSS = 8,
    PIERCING = 9,
    GROUND = 10,
    FLYING = 11,
    BURST_FIRE = 12,
    POISON = 13,
    DEATH_TOUCH = 14,
    EXPLOSIVE_ROUNDS = 15,
    RICOCHET_SHOT = 16,
    CRITICAL = 17,
    CRITICAL_DAMAGE = 18,
    ALL_DAMAGE_REDUCE = 19,
    IMPACT_DAMAGE_REDUCE = 20,
    BULLET_DAMAGE_REDUCE = 21,
    ALL_DAMAGE = 22,
    EVADE = 23,
    SHIELD = 24,
    EXTRA_LIFE = 25,
    DRONE_ATK = 26,
    DOUBLE_SHOT = 27,
    GOLD_EARN = 28,
    REGEN_SHIELD = 29,
    RICOCHET_DAMAGE = 30
}
public enum DamageType {
    NORMAL = 0,
    ICE = 1,
    FIRE = 2,
    POISON = 3,
    CRITICAL = 4
}
