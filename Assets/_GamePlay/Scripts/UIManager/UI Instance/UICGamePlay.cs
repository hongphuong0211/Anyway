using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICGamePlay : UICanvas
{
    public UnityAction attackEvent;
    public UnityAction decodeEvent;
    public Joystick joystick;
    [SerializeField] private Button buttonDecode;
    [SerializeField] private Button buttonAttack;
    public override void Open()
    {
        base.Open();
        buttonDecode.gameObject.SetActive(false);
        buttonAttack.gameObject.SetActive(false);
    }
    public void ActiveDecode(bool status = false, UnityAction action = null){
        buttonDecode.gameObject.SetActive(status);
        decodeEvent = action;
    }
    public void Decode(){
        buttonDecode.gameObject.SetActive(false);
        decodeEvent?.Invoke();
    }
    public void ActiveAttack(bool status = false, UnityAction action = null){
        buttonAttack.gameObject.SetActive(status);
        attackEvent = action;
    }
    public void Attack(){
        attackEvent?.Invoke();
    }
}
