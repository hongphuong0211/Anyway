public class GameData : Singleton<GameData> {
    public string GetEquipmentItemSpriteName(int id, EquipmentType et) {
        string s = "ico-" + et + "-" + id.ToString("00");
        s = s.ToLower();
        return s;
    }
    public int GetLevelRequireEXP(int level)
    {
        return level < AttributeManager.Instance.m_ExperienceData.Count?AttributeManager.Instance.m_ExperienceData[level]: level * 1000;
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
        return selectedCharacterID >= 0 && selectedCharacterID < (AttributeManager.Instance.m_SurvivorDatas.Count)? AttributeManager.Instance.m_SurvivorDatas[selectedCharacterID]: (selectedCharacterID >= AttributeManager.Instance.m_SurvivorDatas.Count && selectedCharacterID < (AttributeManager.Instance.m_SurvivorDatas.Count + AttributeManager.Instance.m_SurvivorDatas.Count))?AttributeManager.Instance.m_HunterDatas[selectedCharacterID - AttributeManager.Instance.m_SurvivorDatas.Count]:null;
    }
}