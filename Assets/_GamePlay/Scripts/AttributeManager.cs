using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class AttributeManager : Singleton<AttributeManager>
{
    public TextAsset m_SurvivorDataCSV;
    public TextAsset m_HunterDataCSV;
    public TextAsset m_LevelPowerCSV;
    public List<Sprite> m_SurvivorSprite;
    public List<Sprite> m_HunterSprite;
    [ListDrawerSettings(DraggableItems = false)]
    public List<CharacterData> m_SurvivorDatas;
    [ListDrawerSettings(DraggableItems = false)]
    public List<CharacterData> m_HunterDatas;
    public List<Vector3> m_LevelPowerValues;

    [Button("Load Data")]
    protected void LoadData()
    {
        List<Dictionary<string, string>> res;
        m_SurvivorDatas = new List<CharacterData>();
        res = CSVReader.Read(m_SurvivorDataCSV);
        for (int i = 0; i < res.Count; i++)
        {
            m_SurvivorDatas.Add(new CharacterData(CharacterType.SURVIVOR, System.Int32.Parse(res[i]["Level"]), res[i]["Damage"], res[i]["HitPoint"], res[i]["CoinGain"]));
        }

        m_HunterDatas = new List<CharacterData>();
        res = CSVReader.Read(m_HunterDataCSV);
        for (int i = 0; i < res.Count; i++)
        {
            m_HunterDatas.Add(new CharacterData(CharacterType.HUNTER, System.Int32.Parse(res[i]["Level"]), res[i]["Damage"], res[i]["HitPoint"], res[i]["CoinGain"]));
        }

        m_LevelPowerValues = new List<Vector3>();
        res = CSVReader.Read(m_LevelPowerCSV);
        for (int i = 0; i < res.Count; i++)
        {
            m_LevelPowerValues.Add(new Vector3(float.Parse(res[i]["a"]), float.Parse(res[i]["b"]), float.Parse(res[i]["c"])));
        }
    }

    public CharacterData GetAttribute(CharacterType type, int level)
    {
        if (level > m_SurvivorDatas.Count) return null;

        switch (type)
        {
            case CharacterType.SURVIVOR:
                return m_SurvivorDatas[level - 1];
            case CharacterType.HUNTER:
                return m_HunterDatas[level - 1];
        }
        return null;
    }
    public BigNumber GetPower(CharacterType type, int level)
    {
        if (level > m_SurvivorDatas.Count) return new BigNumber(0);

        switch (type)
        {
            case CharacterType.SURVIVOR:
                return m_SurvivorDatas[level - 1].Power;
            case CharacterType.HUNTER:
                return m_HunterDatas[level - 1].Power;
        }
        return new BigNumber(0);
    }
    public BigNumber GetPowerLevel(int level)
    {
        BigNumber a = new BigNumber(m_LevelPowerValues[level - 1].x);
        BigNumber b = new BigNumber(m_LevelPowerValues[level - 1].y);
        BigNumber c = new BigNumber(m_LevelPowerValues[level - 1].z);

        return a + level / b + c;
    }
    public BigNumber GetCoinGain(CharacterType type, int level)
    {
        if (level > m_SurvivorDatas.Count) return new BigNumber(0);

        switch (type)
        {
            case CharacterType.SURVIVOR:
                return m_SurvivorDatas[level - 1].CoinGain;
            case CharacterType.HUNTER:
                return m_HunterDatas[level - 1].CoinGain;
        }
        return new BigNumber(0);
    }
}
public enum CharacterType
{
    SURVIVOR = 1,
    HUNTER = 2,
    ALL = -1,
}
[System.Serializable]
public class CharacterData
{
    public CharacterType m_Type;
    public int m_Number;
    public string m_Damage;
    public string m_HitPoint;
    public string m_Power;
    public string m_CoinGain;
    public BigNumber Damage { get => new BigNumber(m_Damage); set => m_Damage = value.ToString(); }
    public BigNumber HitPoint { get => new BigNumber(m_HitPoint); set => m_HitPoint = value.ToString(); }
    public BigNumber Power { get => new BigNumber(m_Power); set => m_Power = value.ToString(); }
    public BigNumber CoinGain { get => new BigNumber(m_CoinGain); set => m_CoinGain = value.ToString(); }

    public CharacterData(CharacterType type, int level, string damage, string hitPoint, string coinGain)
    {
        m_Type = type;
        m_Number = level;
        m_Damage = damage;
        m_HitPoint = hitPoint;
        m_CoinGain = coinGain;
        //m_Power = power;
        m_Power = (HitPoint / (type == CharacterType.SURVIVOR ? 6.9f : 2) + Damage).ToString();

    }
}