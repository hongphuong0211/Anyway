using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
namespace GamePlay
{
    public class UICGamePlay : UICanvas
    {
        
        public UnityAction attackEvent;
        public UnityAction decodeEvent;
        public Joystick joystick;
        [SerializeField] private Button buttonDecode;
        [SerializeField] private Button buttonAttack;
        [SerializeField] private StateUser[] m_StateUser;
        [SerializeField] private TextMeshProUGUI textStatue;
        public override void Open()
        {
            base.Open();
            buttonDecode.gameObject.SetActive(false);
            buttonAttack.gameObject.SetActive(false);
            for(int i = 0; i < m_StateUser.Length; i++){
                m_StateUser[i].ChangeState(-100);
            }
        }
        public void ActiveDecode(bool status = false, UnityAction action = null)
        {
            buttonDecode.gameObject.SetActive(status);
            decodeEvent = action;
        }
        public void Decode()
        {
            buttonDecode.gameObject.SetActive(false);
            decodeEvent?.Invoke();
        }
        public void ActiveAttack(bool status = false, UnityAction action = null)
        {
            buttonAttack.gameObject.SetActive(status);
            attackEvent = action;
        }
        public void Attack()
        {
            attackEvent?.Invoke();
        }

        public void OnSetupUser(int index, string name)
        {
            if (index > 0 && index <= m_StateUser.Length ){
                m_StateUser[index - 1].SetUp(name);
                m_StateUser[index - 1].ChangeState(100);
            }
        }
        public void OnChangeStatus(int index, float hp){
            if (index > 0 && index <= m_StateUser.Length)
            {
                m_StateUser[index - 1].ChangeState(hp);
            }
        }

        public void SetCountStatue(int count)
        {
            textStatue.SetText(("Statue: " + count));
        }
    }
}
