using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : Singleton<IngameManager>
{
    private CameraFollower m_CameraFollower;
    private Player m_Player;
    // public Room m_CurrentRoom;
    public int m_CurrentRoomID;
    public GameObject m_DecoderPrefab;
    public bool IsClearRoom = false;
    public bool IsPause = false;
    public bool IsEndGame = false;

    private float m_PlayTime = 0;
    private int m_CurrentLevel = 1;
    private int m_TotalDroppedEquip = 0;
    private float m_NextPowerBoosterTime = 30f;
    private float m_DeltaTime;
    private List<float> m_FPSCountings = new List<float>();
    private List<float> m_FPSAverage10s = new List<float>();
    
    private Vector2 uiOffset;

    [HideInInspector]
    public float m_MapTopY, m_MapBottomY, m_MapMinX, m_MapMaxX;
    public int m_CharLevel;
    private void Update() {
        if (!IsPause) {
            float dt = Time.deltaTime;
            m_PlayTime += dt;
            m_NextPowerBoosterTime -= dt;
            FPSCounting();
        }
    }
    private void ResetCountingFPS() {
        m_FPSAverage10s.Clear();
        m_FPSCountings.Clear();
    }
    private void FPSCounting() {
        m_DeltaTime += (Time.unscaledDeltaTime - m_DeltaTime) * 0.1f;
        float fps = 1.0f / m_DeltaTime;
        
        m_FPSCountings.Add(fps);
        if (m_FPSCountings.Count >= 300) {
            float num = GetAverageFPS();
            m_FPSAverage10s.Add(num);
            m_FPSCountings.Clear();
        }
    }
    private float GetAverageFPS() {
        float num = 0;
        for(int i = 0; i < m_FPSCountings.Count; i++) {
            float f = m_FPSCountings[i];
            num += f;
        }
        float num1 = num / m_FPSCountings.Count;
        return num1;
    }
    private float GetAverageFPS10() {
        float num = 0;
        for (int i = 0; i < m_FPSAverage10s.Count; i++) {
            float f = m_FPSAverage10s[i];
            num += f;
        }
        float num1 = num / m_FPSAverage10s.Count;
        return num1;
    }
    public void StartGame(int level, int world) {
        ResetCountingFPS();
        IsEndGame = false;
        m_CurrentRoomID = 0;
        int selectedCharID = 0;
        Vector3 spawnPos = new Vector3(0,0.05f,0);
        m_Player.InitCharacter(selectedCharID,
                                       spawnPos);
    }
    public void InitLevel(int level, int world) {
        int type = 1;
    }
    public Player Player {
        get {
            return m_Player;
        }
        set{
            m_Player = value;
        }
    }
    public Character GetCharacter() {
        return m_Player.Character;
    }
   
    public void Pause() {
        IsPause = true;
    }
    public void Resume() {
        IsPause = false;
    }
    public bool IsPlayerDead() {
        return m_Player.Character.IsDead();
    }
}
