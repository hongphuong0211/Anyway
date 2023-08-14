using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class MyNetworkManager : NetworkRoomManager
    {
        public static MyNetworkManager Instance
        {
            get
            {
                return (MyNetworkManager)singleton;
            }
        }
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // call base functionality (actually destroys the player)
            if (Utils.IsSceneActive(GameplayScene) && !UI_Game.Instance.IsOpened(UIID.UICEndGame))
            {
                UI_Game.Instance.CloseUI(UIID.UICGamePlay);
                UICResult resultCanvas = UI_Game.Instance.OpenUI<UICResult>(UIID.UICEndGame);
                resultCanvas.SetUp(-1, new int[4] { 20, 300, 1, -400 },
                    new string[5] { "Hells", "kaks", "sid", "sd", "aoos" },
                    new int[5] { 3000, 1200, 1023, 1332, 4122 });
                CloudCodeManager.Instance.AddScore(-400);
            }

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
            UI_Game.Instance.CloseUI(UIID.UICMatch);
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
            if (playerController.index == 0)
            {
                playerController.type = IngameType.HUNTER;
                playerController.characterData = AttributeManager.Instance.m_HunterDatas[0];
            }
            else
            {
                playerController.type = IngameType.SURVIVOR;
                playerController.characterData = AttributeManager.Instance.m_SurvivorDatas[playerController.index];
            }
            return true;
        }

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
            int indexHunter = Random.Range(0, roomSlots.Count);
            Debug.Log("Hunter: " + indexHunter);
            roomSlots[indexHunter].index = 0;
            roomSlots[0].index = indexHunter;
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
}