using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData> {
    public string GetEquipmentItemSpriteName(int id, EquipmentType et) {
        string s = "ico-" + et + "-" + id.ToString("00");
        s = s.ToLower();
        return s;
    }
    public string GetCharacterName(CharacterType characterType) {
        switch (characterType) {
            case CharacterType.SURVIVOR:
                return "Ada";
            case CharacterType.HUNTER:
                return "Larz";
        }
        return "";
    }
    public CharacterDataConfig GetCharacterDataConfig(int selectedCharacterID){
        return null;
    }
}