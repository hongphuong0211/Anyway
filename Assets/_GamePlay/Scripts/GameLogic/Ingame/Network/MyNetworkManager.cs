using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Assets.HeroEditor4D.FantasyInventory.Scripts.Interface.Elements;

public class MyNetworkManager : NetworkRoomManager
{
    private static MyNetworkManager m_Instance;
    public static MyNetworkManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<MyNetworkManager>();
            }

            // Create new instance if one doesn't already exist.
            if (m_Instance == null)
            {
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                m_Instance = singletonObject.AddComponent<MyNetworkManager>();
                singletonObject.name = typeof(MyNetworkManager).ToString() + " (Singleton)";

                // Make instance persistent.
                //DontDestroyOnLoad(singletonObject);
            }
            return m_Instance;
        }
    }
    private int indexHunter = 0;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }
    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
        // spawn the initial batch of Rewards
        if (sceneName == GameplayScene)
        {
            //UI_Game.Instance.CloseUI(UIID.UICMainMenu);
            UI_Game.Instance.CloseUI(UIID.UICRoom);
            UI_Game.Instance.OpenUI(UIID.UICGamePlay);
            IngameManager.Instance.InitLevel(0, 1);
        }
    }
    public override void OnRoomClientSceneChanged()
    {
        base.OnRoomClientSceneChanged();
    }
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
    }
    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();
        UI_Game.Instance.CloseUI(UIID.UICMainMenu);
        UI_Game.Instance.OpenUI(UIID.UICRoom);
    }
    /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            Player playerController = gamePlayer.GetComponent<Player>();
            playerController.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
            if (playerController.index == indexHunter){
                playerController.type = IngameType.HUNTER;
            }else{
                playerController.type = IngameType.SURVIVOR;
            }
            return true;
        }

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
#endif
    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (allPlayersReady && showStartButton)
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
}
