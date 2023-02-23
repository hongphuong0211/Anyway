using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Character
{
    private Decoder currentDecoder;

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (currentState.Equals(DecodeState.Instance) && other.GetComponent<Decoder>() != null){
    //         currentDecoder = other.GetComponent<Decoder>();
    //         UI_Game.Instance.OpenUI(UIID.UICDecode);
    //     }
    // }
    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.GetComponent<Decoder>() != null){
    //         UI_Game.Instance.CloseUI(UIID.UICDecode);
    //     }
    // }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IngameManager.Instance.GetCharacter() == this && !CurrentState.Equals(DecodeState.Instance))
        {
            currentDecoder = other.gameObject.GetComponent<Decoder>();
            if (currentDecoder != null && currentDecoder.status == 0)
            {
                UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveDecode(true, StartDecode);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (IngameManager.Instance.GetCharacter() == this && currentDecoder != null)
        {
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
    public virtual void OnDecodeStart()
    {
        if (currentDecoder == null)
        {
            ChangeState(IdleState.Instance);
        }
    }
    public virtual void OnDecodeExecute()
    {
        if (currentDecoder != null)
        {
            currentDecoder.CmdDecodeMachine(Time.deltaTime * 100f);
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
