using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapManager : NetworkBehaviour
{
    private static MapManager instance;
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MapManager>();
            }
            return instance;
        }
    }
    public int decodersDone
    {
        get
        {
            int count = 0;
            for (int i = 0; i < decoders.Count; i++)
            {
                count += (decoders[i].status == StatusDecoder.done) ? 1 : 0;
            }
            return count;
        }
    }
    public readonly SyncList<Decoder> decoders = new SyncList<Decoder>();
    public readonly SyncList<Gate> gates = new SyncList<Gate>();
    public void SetNewDecoderDone(Decoder item){
        Debug.Log("Count: " + decoders.Count);
        decoders.Add(item);
        if (decoders.Count == 5){
            Decoder[] decoderAll = FindObjectsOfType<Decoder>();
            foreach(Decoder dc in decoderAll){
                if (dc.status == StatusDecoder.close){
                    dc.status = StatusDecoder.open;
                }else if (dc.status == StatusDecoder.open){
                    dc.status = StatusDecoder.done;
                }
            }
        }
    }
}
