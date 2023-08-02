using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Mirror;
using UnityEngine;
namespace GamePlay
{
    public class Survivor : Character
    {
        private Decoder currentDecoder;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constant.TAG_DECODER) && IngameManager.Instance.Player.index == indexChar && currentDecoder == null)
            {
                Debug.Log("Trigger decoder");
                if (!IngameManager.Instance.m_Decoders.ContainsKey(other)){
                    IngameManager.Instance.m_Decoders.Add(other, other.gameObject.GetComponent<Decoder>());
                }
                currentDecoder = IngameManager.Instance.m_Decoders[other];
                if (currentDecoder != null && currentDecoder.status == 0)
                {
                    UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveDecode(true, StartDecode);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Constant.TAG_DECODER) && IngameManager.Instance.Player.index == indexChar && currentDecoder != null)
            {
                Debug.Log("Trigger exit decoder");
                UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveDecode(false);
                currentDecoder = null;
            }
        }
        private void StartDecode()
        {
            ChangeState(DecodeState.Instance);
        }
        #region StateMachine
        // Survivor Decode State
        public void OnDecodeStart()
        {
            if (currentDecoder == null)
            {
                ChangeState(IdleState.Instance);
            }
        }

        public void AnimDecode()
        {
            CharacterControl.AnimationManager.Slash(false);
        }
        public void OnDecodeExecute()
        {
            if (currentDecoder != null)
            {
                IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.Decode);
                currentDecoder.CmdDecodeMachine(IngameManager.Instance.Player.netIdentity, m_CharacterDataConfig.decodeSpeed);
            }
            else
            {
                ChangeState(IdleState.Instance);
            }
        }
        public virtual void OnDecodeExit()
        {
            currentDecoder = null;
        }
        #endregion
    }
}