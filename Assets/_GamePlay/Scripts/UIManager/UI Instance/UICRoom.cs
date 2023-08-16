using UnityEngine.UI;
using UnityEngine;
using Mirror;
namespace GamePlay
{
    public class UICRoom : UICanvas
    {
        public NetworkRoomPlayer roomPlayer;
        private bool isFull = false;
        public void SetFullState(bool newState)
        {
            isFull = newState;
            readyButton.gameObject.SetActive(isFull);
            exitButton.gameObject.SetActive(!isFull);
        }
        public Transform roomView;
        public Button readyButton;
        public Button exitButton;
        public Text textHeader;
        private float timeCount = 60f;
        private bool isReady = false;

        public override void Open()
        {
            base.Open();
            timeCount = 60f;
            isFull = false;
            textHeader.text = "Waiting...";
            exitButton.gameObject.SetActive(true);
            readyButton.gameObject.SetActive(false);
        }

        public void Cancel()
        {
            if (!isFull)
            {
                roomPlayer.netIdentity.connectionToClient.Disconnect();
                MyNetworkManager.Instance.StopClient();
                if (roomPlayer.netIdentity.isServer)
                {
                    MyNetworkManager.Instance.StopServer();
                }
                UI_Game.Instance.CloseUI(UIID.UICRoom);
                UI_Game.Instance.OpenUI(UIID.UICMainMenu);
            }
            else if (timeCount > 0)
            {
                isReady = false;
                roomPlayer.ReadyStateChanged(true, false);
                roomPlayer.CmdChangeReadyState(false);
                exitButton.gameObject.SetActive(false);
                readyButton.gameObject.SetActive(true);
            }
        }
        public void Ready()
        {
            isReady = true;
            roomPlayer.ReadyStateChanged(false, true);
            roomPlayer.CmdChangeReadyState(true);
            exitButton.gameObject.SetActive(true);
            readyButton.gameObject.SetActive(false);
        }
        public void Update()
        {
            if (isFull)
            {
                if (MyNetworkManager.Instance.roomSlots.Count < MyNetworkManager.Instance.maxConnections)
                {
                    SetFullState(false);
                    return;
                }
                if (!isReady)
                {
                    if (timeCount > 0f)
                    {
                        timeCount -= Time.deltaTime;
                        int minute = (int)timeCount / 60;
                        int second = (int)(timeCount - minute * 60);
                        textHeader.text = minute.ToString("00") + ":" + second.ToString("00");
                    }
                    else if (!isReady)
                    {
                        Ready();
                        // if (roomPlayer.isServer)
                        // {
                        //     MyNetworkManager.Instance.allPlayersReady = true;
                        // }
                    }
                }
                // else if (roomPlayer.isServer)
                // {
                //     if (MyNetworkManager.Instance.allPlayersReady)
                //     {
                //         MyNetworkManager.Instance.ServerChangeScene(MyNetworkManager.Instance.GameplayScene);
                //     }
                // }
            }
            else if (MyNetworkManager.Instance.roomSlots.Count >= MyNetworkManager.Instance.maxConnections)
            {
                SetFullState(true);

            }
        }
    }
}