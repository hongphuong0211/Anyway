using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor4D.Common.Scripts.Common;
using UnityEngine;
using Mirror;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
namespace GamePlay
{
    public class MyRoomPlayer : NetworkRoomPlayer
    {
        public ItemSlot slot;
        public override void OnClientEnterRoom()
        {
            base.OnClientEnterRoom();
            UICRoom room = UI_Game.Instance.GetUI<UICRoom>(UIID.UICRoom);
            slot.transform.SetParent(room.roomView);
            slot.transform.localPosition -= Vector3.forward * slot.transform.localPosition.z;
            slot.transform.localScale = Vector3.one;
            slot.gameObject.SetActive(true);
            if (this.isLocalPlayer)
            {
                room.roomPlayer = this;
            }
        }
        public override void OnClientExitRoom()
        {
            base.OnClientExitRoom();
            UICRoom room = UI_Game.Instance.GetUI<UICRoom>(UIID.UICRoom);
            slot.transform.SetParent(null);
            slot.gameObject.SetActive(false);
            if (this.isLocalPlayer)
            {
                room.roomPlayer = this;
            }
        }
        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            base.ReadyStateChanged(oldReadyState, newReadyState);

        }
        public override void OnGUI()
        {
            //ItemSlot slot = Instantiate<ItemSlot>(slotPrefab, room.roomView);
            //NetworkServer.Spawn(slot.gameObject);

            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!NetworkManager.IsSceneActive(room.RoomScene))
                    return;

                // DrawPlayerReadyState();
                // DrawPlayerReadyButton();
            }
        }
        public override void OnStartClient()
        {
            Debug.Log($"OnStartClient {gameObject}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            Debug.Log($"IndexChanged {newIndex}");
        }
    }
}
