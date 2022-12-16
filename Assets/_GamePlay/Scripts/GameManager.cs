using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private static GameState gameState = GameState.MainMenu;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        //csv.OnInit();
        userData?.OnInitData();

        ChangeState(GameState.MainMenu);

    }

    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state)
    {
        return gameState == state;
    }
    private void Start()
    {
        //CheckOfflineEarning();
    }
    private void OnApplicationQuit()
    {
        userData.SetStringData(UserData.Key_Last_Time_Play, ref userData.lastTimePlay, System.DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }
    //private void CheckOfflineEarning()
    //{
    //    System.DateTime dtNow = System.DateTime.Now;
    //    System.DateTime dtLastTimePlay = System.DateTime.Parse(userData.lastTimePlay, CultureInfo.InvariantCulture);

    //    double offlineTime = (dtNow - dtLastTimePlay).TotalMinutes;
    //    offlineTime = Mathf.Clamp((float)offlineTime, 0, 240);
    //    if (offlineTime >= 30)
    //    {
    //        BigNumber coinGain = LevelManager.Instance.GetBaseCoinGainEndGame() * offlineTime / 12;
    //        UI_Game.Instance.OpenUI<UIOfflineEarning>(UIID.UICOfflineEarnings).Setup(coinGain);
    //    }
    //}
}
