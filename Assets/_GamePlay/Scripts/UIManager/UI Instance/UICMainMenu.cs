using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UICMainMenu : UICanvas
{
    private bool finding = false;
    [SerializeField] Transform matchPanel;
    [SerializeField] TextMeshProUGUI textTime;
    private float timeWait = 0;
    [SerializeField] GameObject viewPlayer;

    public void StartHost()
    {
        finding = false;
        GameManager.Instance.StartHost();
    }
    public void FindRoom()
    {
        viewPlayer.SetActive(false);
        GameManager.Instance.FindRoom();
        finding = true;
        matchPanel.gameObject.SetActive(true);
        timeWait = 0;
    }
    public void CancelFind(){
        viewPlayer.SetActive(true);
        GameManager.Instance.CancelConnect();
        finding = false;
        matchPanel.gameObject.SetActive(false);
    }

    
    private void Update() {
        if (finding){
            timeWait += Time.deltaTime;
            int minute = (int)timeWait/60;
            int second = (int) (timeWait - minute * 60);
            textTime.SetText(minute.ToString("00") + ":" + second.ToString("00"));
        }
    }
    public void setFindState(bool newState){
        finding = newState;
    }
    public override void Open()
    {
        base.Open();
        viewPlayer.SetActive(true);
    }
}