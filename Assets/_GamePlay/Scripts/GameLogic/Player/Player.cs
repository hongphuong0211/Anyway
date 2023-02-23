using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public int index;
    [SyncVar]
    public IngameType type;
    [SyncVar(hook = nameof(SetCharacterDir))]
    public Vector2 Direction;
    [SerializeField] Hunter hunterController;
    [SerializeField] Survivor survivorController;
    protected Character m_Character;
    public Character Character
    {
        get
        {
            return m_Character;
        }
    }
    public Joystick joystick;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private CameraFollower cameraFollow;
    private float zoom = 5;
    private void Start() {
        if (type == IngameType.SURVIVOR)
        {
            m_Character = survivorController;
            survivorController.gameObject.SetActive(true);
            hunterController.gameObject.SetActive(false);
        }
        else if (type == IngameType.HUNTER)
        {
            m_Character = hunterController;
            survivorController.gameObject.SetActive(false);
            hunterController.gameObject.SetActive(true);
        }
        m_Character.enabled = true;
        if (!isLocalPlayer)
        {
            m_Character.highlight.SetActive(false);
        }
        
    }
    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            IngameManager.Instance.Player = this;
            joystick = UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).joystick;
            cameraFollow = FindObjectOfType<CameraFollower>();
            cameraFollow.Setup(() => transform.position, () => zoom, true, true);
            fieldOfView = FindObjectOfType<FieldOfView>();
            UI_Game.Instance.CloseUI(UIID.UICRoom);
            UI_Game.Instance.OpenUI(UIID.UICGamePlay);
            IngameManager.Instance.StartGame(0, 1);
        }
    }
    public void InitCharacter(int selectedCharacterID, Vector3 spawnPosition)
    {
        if (isLocalPlayer)
        {
            FunctionUpdater.Create(() =>
            {
                if (m_Character.CurrentState != null && m_Character.CurrentState.Equals(ControlState.Instance))
                {   
                    if (joystick.Direction.sqrMagnitude > 0.0001f){
                        fieldOfView.SetAimDirection(joystick.Direction);
                    }
                    fieldOfView.SetOrigin(m_Character.Transform.position);
                }

            });
        }

    }
    public void SetCharacterPos(Vector3 pos)
    {
        m_Character.Transform.position = pos;
    }
    private void Update()
    {
        if (!isLocalPlayer) return;
        {
            if (IngameManager.Instance.Player == null || IngameManager.Instance.GetCharacter() == null)
            {
                IngameManager.Instance.Player = this;
            }
            if (IngameManager.Instance.GetCharacter().IsDead()) return;
            Vector2 inputDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
            if (inputDirection.sqrMagnitude > 0.0001f)
            {
                if (!m_Character.CurrentState.Equals(ControlState.Instance))
                {
                    m_Character.ChangeState(ControlState.Instance);
                }
                Vector2 direction = (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y)) ? Vector2.right * (inputDirection.x / Mathf.Abs(inputDirection.x)) : Vector2.up * (inputDirection.y / Mathf.Abs(inputDirection.y));
                SetDirection(direction);
            }
            SendInputToCharacter(inputDirection);
        }
    }
    public void SendInputToCharacter(Vector2 movement)
    {
        m_Character.SetInputMovement(movement);
    }
    [Command]
    private void SetDirection(Vector2 dir){
        Direction = dir;
    }
    private void SetCharacterDir(Vector2 oldVar, Vector2 newVar){
        m_Character.SetDirection(newVar);
    }
    [Command]
    public void CmdMagic(Player target, float damage)
    {
        target.Character.TakeDamage(damage);
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        TargetDoMagic(opponentIdentity.connectionToClient, damage);
    }

    [TargetRpc]
    public void TargetDoMagic(NetworkConnection target, float damage)
    {
        // This will appear on the opponent's client, not the attacking player's
        Debug.Log($"Magic Damage = {damage}");
        Player targetPlayer = target.identity.gameObject.GetComponent<Player>();
        targetPlayer.Character.TakeDamage(damage);
    }
    // [Command]
    // public void OnHitOfHunter(BigNumber damage){
    //     playerHP -= damage.ToFloat();
    //     NetworkIdentity 
    // }
    // public void SetCharacterHP(float oldVar, float newVar){
    //     Debug.Log(oldVar + " " + newVar);
    //     m_Character.TakeDamage(new BigNumber(oldVar - newVar));
    // }
}