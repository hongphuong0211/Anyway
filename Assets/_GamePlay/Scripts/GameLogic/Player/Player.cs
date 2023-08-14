using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;
using Mirror;
namespace GamePlay
{
    public enum AnimationPlayer
    {
        Idle,
        Hit, Die, Run,
        Slash1H,
        Slash2H,
        CrossbowShot,
        Fire, SecondaryShot,
        Slash
    }
    public class Player : NetworkBehaviour
    {
        [SyncVar]
        public int index;
        [SyncVar]
        public IngameType type;
        [SyncVar(hook = nameof(SetCharacterDir))]
        public Vector2 Direction;
        [SyncVar]
        public CharacterDataConfig characterData;

        [SyncVar(hook = nameof(SetCharacterAnim))] private int AnimationPlayer;
        [SerializeField] Hunter hunterController;
        [SerializeField] Survivor survivorController;
        [SerializeField] AudioSource footstep;
        protected Character m_Character;
        
        public Character Character
        {
            get
            {
                return m_Character;
            }
        }
        public Joystick joystick;
        //[SerializeField] private FieldOfView fieldOfView;
        [SerializeField] private CameraFollower cameraFollow;
        private float zoom = 5;
        private void Start()
        {
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
            m_Character.indexChar = index;
            m_Character.enabled = true;
            if (!isLocalPlayer)
            {
                m_Character.highlight.SetActive(false);
            }
            m_Character.InitCharacter(characterData);
            UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).OnSetupUser(index, characterData.name);
        }
        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                IngameManager.Instance.Player = this;
                joystick = UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).joystick;
                cameraFollow = FindObjectOfType<CameraFollower>();
                cameraFollow.Setup(() => transform.position, () => zoom, true, true);
                //fieldOfView = FindObjectOfType<FieldOfView>();
                UI_Game.Instance.CloseUI(UIID.UICRoom);
                UI_Game.Instance.OpenUI(UIID.UICGamePlay);
                IngameManager.Instance.StartGame(0, 1);
            }
        }
        public void InitCharacter(int selectedCharacterID, Vector3 spawnPosition)
        {
            if (isLocalPlayer)
            {
                // FunctionUpdater.Create(() =>
                // {
                //     if (m_Character.CurrentState != null && m_Character.CurrentState.Equals(ControlState.Instance))
                //     {
                //         if (joystick.Direction.sqrMagnitude > 0.0001f)
                //         {
                //             fieldOfView.SetAimDirection(joystick.Direction);
                //         }
                //         fieldOfView.SetOrigin(m_Character.Transform.position);
                //     }

                // });
            }

        }
        public void SetCharacterPos(Vector3 pos)
        {
            m_Character.Transform.position = pos;
        }
        private void Update()
        {
            if (isLocalPlayer)
            {
                if (IngameManager.Instance.Player == null || IngameManager.Instance.GetCharacter() == null)
                {
                    IngameManager.Instance.Player = this;
                }
                if (m_Character.IsDead()) return;
                Vector2 inputDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
                if (inputDirection.sqrMagnitude > 0f)
                {
                    if (!m_Character.isControling)
                    {
                        m_Character.ChangeState(ControlState.Instance);
                        m_Character.isControling = true;
                    }
                    if (footstep.isPlaying){
                        footstep.Stop();
                    }
                    footstep.Play();
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
        private void SetDirection(Vector2 dir)
        {
            Direction = dir;
        }
        private void SetCharacterDir(Vector2 oldVar, Vector2 newVar)
        {
            m_Character.SetDirection(newVar);
        }

        [Command]
        public void CmdMagic(Player target, float damage)
        {
            RpcDoMagic(this, target, damage);
        }

        [ClientRpc]
        public void RpcDoMagic(Player owner, Player target, float damage)
        {
            // This will appear on the opponent's client, not the attacking player's
            Debug.Log($"Magic Damage = {damage}!" + "Player Index: " + index );
            target.Character.OnHit(owner.Character, damage, target.index);
            IngameManager.Instance.EndGame();
        }

        [Command]
        public void CmdChangeAnimation(AnimationPlayer typeAnim)
        {
            AnimationPlayer = (int)typeAnim;
        }
        
        private void SetCharacterAnim(int oldVar, int newVar)
        {
            switch ((AnimationPlayer) newVar)
            {
                case GamePlay.AnimationPlayer.Idle:
                    m_Character.CharacterControl.AnimationManager.SetState(CharacterState.Idle);
                    break;
                case GamePlay.AnimationPlayer.Die:
                    m_Character.CharacterControl.AnimationManager.Die();
                    break;
                case GamePlay.AnimationPlayer.Hit:
                    m_Character.CharacterControl.AnimationManager.Hit();
                    break;
                case GamePlay.AnimationPlayer.Run:
                    m_Character.CharacterControl.AnimationManager.SetState(CharacterState.Run);
                    break;
                case GamePlay.AnimationPlayer.Slash1H:
                    m_Character.CharacterControl.AnimationManager.Slash1H();
                    break;
                case GamePlay.AnimationPlayer.Slash2H:
                    m_Character.CharacterControl.AnimationManager.Slash2H();
                    break;
                case GamePlay.AnimationPlayer.CrossbowShot:
                    m_Character.CharacterControl.AnimationManager.CrossbowShot();
                    break;
                case GamePlay.AnimationPlayer.Fire:
                    m_Character.CharacterControl.AnimationManager.Fire();
                    if (m_Character.CharacterControl.Parts[0].PrimaryWeapon != null)
                    {
                        var firearm = m_Character.CharacterControl.SpriteCollection.Firearm1H.SingleOrDefault(i => i.Sprites.Contains(m_Character.CharacterControl.Parts[0].PrimaryWeapon))
                                      ?? m_Character.CharacterControl.SpriteCollection.Firearm2H.SingleOrDefault(i => i.Sprites.Contains(m_Character.CharacterControl.Parts[0].PrimaryWeapon));

                        if (firearm != null)
                        {
                            m_Character.FirearmFx.CreateFireMuzzle(firearm.Name, firearm.Collection);
                        }
                    }
                    break;
                case GamePlay.AnimationPlayer.SecondaryShot:
                    m_Character.CharacterControl.AnimationManager.SecondaryShot();
                    break;
                case GamePlay.AnimationPlayer.Slash:
                    m_Character.CharacterControl.AnimationManager.Slash(false);
                    break;
            }
        }
    }
}