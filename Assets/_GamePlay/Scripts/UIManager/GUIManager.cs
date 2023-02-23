using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager> {
    internal class GUIMap : Dictionary<int, UICanvas> {
        public static int m_NextID = 0;
        public int m_ID = 0;
        public GUIMap() {
            //Debug.Log("Create new GUI MAP " + m_NextID);
            m_ID = m_NextID;
            m_NextID++;
        }
    }
    private GUIMap m_GUIMap;

    private UICanvas m_PreviousPopup = null;
    private UICanvas m_PreviousPanel = null;
    private bool IsHoldBackkey = false;
    private bool IsLockNotify = false;
    private bool IsFirstOpenPanelLevel = true;
    private bool IsFirstOpenPanelWorld = true;
    private float m_OffsetTop = 0;
    private List<UICanvas> m_CurrentOpenedPopup = new List<UICanvas>();

    public List<UICanvas> CurrentOpenedPopup {
        get { return m_CurrentOpenedPopup; }
        set { m_CurrentOpenedPopup = value; }
    }
    private List<UICanvas> m_CurrentOpenPanel = new List<UICanvas>();

    private Vector3 m_CenterPos;
    //public RectTransform m_CenterPosition;
    //public RectTransform m_LeftPosition;
    //public RectTransform m_RightPosition;
    //public RectTransform m_TopPosition;
    //public RectTransform m_BottonPosition;
    //public GameObject m_PanelMainButton;

    public GameObject m_MainCanvas;
    public GameObject m_SubCanvas;
    public UICanvas m_PopupLoading;

    public bool IsLongDevice = false;

    public bool IsShowFPS = false;

    public GameObject m_ItemNotification;
    public GameObject m_TutorialPanel;

    public GameObject JoyStick;

    protected override void Awake() {
        base.Awake();
        this.m_GUIMap = new GUIMap();
            if (m_SubCanvas != null) {
                DontDestroyOnLoad(m_SubCanvas);
            }
            float ratio = (float)Screen.height / (float)Screen.width;
            if (ratio > 2.1f) {
                m_OffsetTop = -50f;
            }
            if (ratio > 1.8f) {
                IsLongDevice = true;
            }
            m_CenterPos = Vector3.zero + new Vector3(0, m_OffsetTop);
    }
    
    void Start() {
        int maxScreenHeight = 1080;
        //#if UNITY_IPHONE
        //        if((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") > -1){
        //            maxScreenHeight = 1440;
        //        }
        //        if((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPhone5") > -1 || 
        //            (UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPhone5S") > -1 ||
        //            (UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPhone5C") > -1 ||
        //            (UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPhone6") > -1) {
        //            maxScreenHeight = 720;
        //        }
        //#endif
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight) {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
        LoadInitScene();
    }

    AsyncOperation async;
    bool isLoadInitScene = false;
    public void LoadInitScene() {
        StartCoroutine(LoadScreen());
    }
    IEnumerator LoadScreen() {
        Debug.Log("Start Load");
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        async = SceneManager.LoadSceneAsync("InitScene", LoadSceneMode.Single);
        async.allowSceneActivation = false;
        //float _loadProgress = 0;
        //while(_loadProgress <= 0.3f) {
        //    _loadProgress += 0.02f;
        //    yield return new WaitForSeconds(Time.deltaTime);
        //}

        while (async.progress < 0.9f) {
            yield return null;
        }

        async.allowSceneActivation = true;
    }
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1) {
        if (!isLoadInitScene) {
            isLoadInitScene = true;
            //GameManager.Instance.ChangeToStartMenu();
        }
    }

    private float m_LoadingTime = 0;
    private void Update() {
        m_LoadingTime += Time.deltaTime;
        // if (Input.GetKeyDown(KeyCode.Escape) && !IsHoldBackkey) {
        //     if (GameManager.Instance.GetTutorialMode()) return;
        //     UICanvas _UICanvas = GUIManager.Instance.GetCurrentPopup();
        //     if (_UICanvas != null && !_UICanvas.IsAvoidBackKey) {
        //         _UICanvas.OnBack();
        //     } else {
        //         //if (GameManager.Instance.GetScenename().Contains("MainMenu")) {
        //         //    PopupQuitGame pmd = GUIManager.Instance.GetUICanvasByID(UIID.QuitGame) as PopupQuitGame;
        //         //    //print(pmd);
        //         //    GUIManager.Instance.ShowUIPopup(pmd, false);
        //         //}
        //     }
        // }
// #if UNITY_EDITOR
//         if (Input.GetKeyDown(KeyCode.Space)) {
//             PopupWinLevel peg = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN_LEVEL) as PopupWinLevel;
//             peg.Setup(1, Random.Range(30, 200), Random.Range(10, 100), Random.Range(0, 1000));
//             GUIManager.Instance.ShowUIPopup(peg, false);
//         }
// #endif
        //if (Input.GetKeyDown(KeyCode.Delete)) {
        //    Achievement achie = new Achievement(1, 1, "test tí", MissionTargetID.ARMOR_RANK);
        //    List<DescriptionInfo> achievementInfoes = GameData.Instance.m_AchievementInfos;
        //    achie.achievementInfo = achievementInfoes[0];
        //    achie.description = Random.Range(0, 10000).ToString();
        //    GameManager.Instance.m_CompletedAchievements.Add(achie);
        //    CheckNotifyAchievement();
        //}
    }
   public void SetJoyStick(GameObject go) {
        JoyStick = go;
    }
    public GameObject GetJoyStick() {
        return JoyStick;
    }
    public void ActiveJoyStick(bool active) {
        JoyStick.SetActive(active);
    }
}
