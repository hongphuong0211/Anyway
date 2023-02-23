using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using Mirror.Discovery;
using Mirror;
[RequireComponent(typeof(NetworkDiscovery))]
public class GameManager : Singleton<GameManager>
{
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    [SerializeField]private NetworkDiscovery networkDiscovery;
    #if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
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
        UI_Game.Instance.OpenUI(UIID.UICMainMenu);
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
    // private void Start()
    // {
    //     //CheckOfflineEarning();
    // }
    // private void OnApplicationQuit()
    // {
        // userData.SetStringData(UserData.Key_Last_Time_Play, ref userData.lastTimePlay, System.DateTime.Now.ToString(CultureInfo.InvariantCulture));
    //}
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
    #region Connect
    bool isFinding = false;
    public void StartHost()
    {
        isFinding = false;
        discoveredServers.Clear();
        MyNetworkManager.Instance.StartHost();
        networkDiscovery.AdvertiseServer();
    }
    public void FindRoom()
    {
        isFinding = true;
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }
    public void CancelConnect(){
        isFinding = false;
        discoveredServers.Clear();
        networkDiscovery.StopDiscovery();
    }
    void Connect(ServerResponse info)
    {
        networkDiscovery.StopDiscovery();
        MyNetworkManager.Instance.StartClient(info.uri);
    }
    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
        Debug.Log(info.serverId);
    }
    private void Update() {
        if(isFinding){
            foreach (ServerResponse info in discoveredServers.Values){
                isFinding = false;
                UI_Game.Instance.GetUI<UICMainMenu>(UIID.UICMainMenu).setFindState(false);
                Connect(info);
            }
        }
    }
    #endregion
}
