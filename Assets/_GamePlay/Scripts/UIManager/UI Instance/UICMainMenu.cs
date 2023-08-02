using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace GamePlay
{
    public class UICMainMenu : UICanvas
    {
        private int m_CurrentMenu = 2;
        [SerializeField] private List<MenuItem> m_Menu;
        [SerializeField] private TextMeshProUGUI m_TextLevel;
        [SerializeField] private TextMeshProUGUI m_TextGold; 
        [SerializeField] private TextMeshProUGUI m_TextCrystal;
        [SerializeField] private Slider m_SliderExperience;
        public override void Setup()
        {
            base.Setup();
            ChangeView(2);
            m_TextLevel.SetText((ProfileManager.MyProfile.lv).ToString());
            m_TextGold.SetText(new BigNumber(GameManager.Instance.m_UserData.gold).ToString3());
            m_TextCrystal.SetText(new BigNumber(GameManager.Instance.m_UserData.crystal).ToString3());
            m_SliderExperience.value = (float)ProfileManager.MyProfile.exp / (float)AttributeManager.Instance.m_ExperienceData[Mathf.Min(ProfileManager.MyProfile.lv - 1, AttributeManager.Instance.m_ExperienceData.Count - 1)];
        }
        public void StartHost()
        {
            GameManager.Instance.StartHost();
        }
        public void FindRoom()
        {
            GameManager.Instance.FindRoom();
            UI_Game.Instance.OpenUI(UIID.UICMatch);
        }
        public void ChangeView(int index){
            if (index > -1 && index < m_Menu.Count){
                if (m_Menu[index].m_Object != null && m_Menu[index].m_State){
                    if (m_CurrentMenu > -1 && m_CurrentMenu < m_Menu.Count && m_Menu[m_CurrentMenu].m_Object != null)
                        m_Menu[m_CurrentMenu].m_Object.SetActive(false);
                    m_CurrentMenu = index;
                    m_Menu[m_CurrentMenu].m_Object.SetActive(true);
                }
            }
        }
    }
    [Serializable]
    public class MenuItem{
        public GameObject m_Object = null;
        public bool m_State = false;
    }
}