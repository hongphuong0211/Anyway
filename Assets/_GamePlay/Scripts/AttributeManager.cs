using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class AttributeManager : Singleton<AttributeManager>
{
    public TextAsset m_SurvivorDataCSV;
    public TextAsset m_HunterDataCSV;
    public TextAsset m_LevelPowerCSV;
    public List<Sprite> m_ClassSprite;
    public List<Sprite> m_SurvivorSprite;
    public List<Sprite> m_HunterSprite;
    [ListDrawerSettings(DraggableItems = false)]
    public List<CharacterDataConfig> m_SurvivorDatas;
    [ListDrawerSettings(DraggableItems = false)]
    public List<CharacterDataConfig> m_HunterDatas;
    public List<Sprite> m_ResourceSprite;
    public List<Material> m_EffectMaterial;
    public List<int> m_ExperienceData = new List<int>(){100, 200, 500, 1000};
    
    [Button("Load Data")]
    protected void LoadData()
    {
        List<Dictionary<string, string>> res;
        m_SurvivorDatas = new List<CharacterDataConfig>();
        res = CSVReader.Read(m_SurvivorDataCSV);
        for (int i = 0; i < res.Count; i++)
        {
            CharacterDataConfig config = new CharacterDataConfig(System.Int32.Parse(res[i]["id"]), float.Parse(res[i]["detechRange"]), CharacterType.SURVIVOR, res[i].ContainsKey("class")?(CharacterClass) System.Int32.Parse(res[i]["class"]):CharacterClass.DEFAULT);
            config.InitMovement(float.Parse(res[i]["moveSpeed"]), float.Parse(res[i]["minSpeed"]), float.Parse(res[i]["maxSpeed"]), float.Parse(res[i]["obstacleSpeed"]));
            config.decodeSpeed = float.Parse(res[i]["decodeSpeed"]);
            config.healSpeed = float.Parse(res[i]["healSpeed"]);
            config.name = res[i]["name"];
            m_SurvivorDatas.Add(config);
        }

        m_HunterDatas = new List<CharacterDataConfig>();
        res = CSVReader.Read(m_HunterDataCSV);
        for (int i = 0; i < res.Count; i++)
        {
            CharacterDataConfig config = new CharacterDataConfig(System.Int32.Parse(res[i]["id"]), float.Parse(res[i]["detechRange"]), CharacterType.HUNTER);
            config.InitMovement(float.Parse(res[i]["moveSpeed"]), float.Parse(res[i]["minSpeed"]), float.Parse(res[i]["maxSpeed"]), float.Parse(res[i]["obstacleSpeed"]));
            config.attackSpeed = float.Parse(res[i]["attackSpeed"]);
            config.name = res[i]["name"];
            m_HunterDatas.Add(config);
        }
        
    }

}
public enum CharacterType
{
    SURVIVOR = 1,
    HUNTER = 2,
    ALL = -1,
}

public enum CharacterClass
{
    DEFAULT = 0,
    SUPPORT = 1,
    KITE = 2, 
    DECODE = 3,
    
}