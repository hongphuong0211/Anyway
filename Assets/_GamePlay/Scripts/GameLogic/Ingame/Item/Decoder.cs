using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public enum StatusDecoder
{
    open = 0,
    close = 1,
    done = 2
}

public class Decoder : NetworkBehaviour
{
    public UnityAction actionComplete;
    private List<Color> color = new List<Color>() { new Color(255, 255, 255, 255), new Color(255, 0, 0, 255), new Color(44, 44, 44, 255) };
    [SerializeField] private SpriteRenderer view;
    [SyncVar(hook = nameof(SetView))]
    public StatusDecoder status;
    [SerializeField] private Slider progressBar;
    [SyncVar(hook = nameof(SetBar))]
    [SerializeField]private float currentProgress = 0;
    private float maxProgress = 100;
    private bool isDecoded = false;
    [SyncVar]
    private float efficiency;

    [SerializeField] private Gate _gate;

    private void Start()
    {
        SetStatus(status);
        if (status == StatusDecoder.close)
            IngameManager.Instance.RegisterStatue(this);
    }

    private void FixedUpdate()
    {
        //SetView(StatusDecoder.open, status);
        if (isServer)
        {
            if (status == StatusDecoder.open && isDecoded && efficiency > 0)
            {
                if (currentProgress < maxProgress)
                {
                    Debug.Log("Break Statue Progress");
                    currentProgress += Time.fixedDeltaTime * efficiency;
                }
                else
                {
                    SetStatus(StatusDecoder.done);
                    // MapManager.Instance.SetNewDecoderDone(this);
                    // Debug.Log("Count Decode: " + MapManager.Instance.decodersDone);
                }
                isDecoded = false;
                efficiency = 0;
            }
            else
            {
                isDecoded = false;
                efficiency = 0;
            }
        }

        if (isClient)
        {
            SetBar(0, currentProgress);
            if (currentProgress >= maxProgress && status != StatusDecoder.done)
            {
                SetStatus(StatusDecoder.done);
            }
        }
    }
    public override void OnStartServer()
    {
        //view = GetComponentInChildren<SpriteRenderer>();
        progressBar.maxValue = maxProgress;
    }
    [Command(requiresAuthority = false)]
    public void CmdDecodeMachine(NetworkIdentity user, float eff)
    {
        //user.AssignClientAuthority(connectionToClient);
        isDecoded = true;
        this.efficiency += eff;
    }

    // [ClientRpc]
    // public void ClientRpcDecodeMachine(float efficiency){
    //     DecodeMachine(efficiency);
    // }

    public int GetProgress()
    {
        return (int)(currentProgress / maxProgress * 100f);
    }
    public void SetBar(float oldVar, float newVar)
    {
        progressBar.value = Mathf.Min(newVar/maxProgress, 1.0f);
        if (newVar >= maxProgress)
        {
            SetStatus(StatusDecoder.done);
        }
    }
    public void SetStatus(StatusDecoder type)
    {
        if (type == StatusDecoder.open)
        {
            if (currentProgress >= maxProgress)
            {
                SetStatus(StatusDecoder.done);
                return;
            }
        }
        else if (type == StatusDecoder.done)
        {
            actionComplete?.Invoke();
            IngameManager.Instance.BreakStatue(this);
            if (_gate != null)
            {
                _gate.OpenGate();
            }
        }
        this.status = type;
        view.color = color[(int)type];
    }
    public void SetView(StatusDecoder oldVar, StatusDecoder newVar)
    {
        view.color = color[(int)newVar];
    }
}
