using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;
namespace GamePlay
{
    public class Hunter : Character
    {
        private Player currentTarget;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constant.TAG_CHARACTER) && IngameManager.Instance.Player.index == indexChar)
            {
                if (currentTarget == null)
                {
                    if (!IngameManager.Instance.m_Players.ContainsKey(other))
                    {
                        IngameManager.Instance.m_Players.Add(other, other.gameObject.GetComponent<Player>());
                    }
                    currentTarget = IngameManager.Instance.m_Players[other];
                    if (currentTarget != null && currentTarget.Character is Survivor)
                    {
                        UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveAttack(true, Attack);
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Constant.TAG_CHARACTER) && IngameManager.Instance.Player.index == indexChar && currentTarget != null)
            {
                if (!IngameManager.Instance.m_Players.ContainsKey(other))
                {
                    IngameManager.Instance.m_Players.Add(other, other.gameObject.GetComponent<Player>());
                }
                if (currentTarget == IngameManager.Instance.m_Players[other])
                {
                    UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveAttack(false);
                    currentTarget = null;
                }
            }
        }
        public void Attack()
        {
            Debug.Log("Attack");
            ChangeState(AttackState.Instance);
        }
        #region StateMachine
        // Hunter Attack State
        public virtual void OnAttackStart()
        {
        }
        public virtual void OnAttackExecute()
        {
            if (currentTarget != null)
            {
                AnimAttack();
                //IngameManager.Instance.Player.CmdChangeAnimation((AnimationPlayer.Attack));
                IngameManager.Instance.Player.CmdMagic(currentTarget, 50);
                ChangeState(null);
                currentTarget = null;
                Invoke(nameof(ResetControl), 3.5f);
            }
            else
            {
                currentTarget = null;
            }
        }

        public void AnimAttack()
        {
            if (CharacterControl.AnimationManager.IsAction) return;
            
            Debug.Log(CharacterControl.WeaponType);
            switch (CharacterControl.WeaponType)
            {
                case WeaponType.Melee1H:
                    IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.Slash1H);
                    //CharacterControl.AnimationManager.Slash1H();
                    break;
                case  WeaponType.Melee2H:
                    IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.Slash2H);
                    //CharacterControl.AnimationManager.Slash2H();
                    break;
                case WeaponType.Crossbow:
                    Debug.Log("Cross Bow");
                    IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.CrossbowShot);
                    //CharacterControl.AnimationManager.CrossbowShot();
                    break;
                case WeaponType.Firearm1H: 
                case WeaponType.Firearm2H:
                {
                    Debug.Log("Fire Arm");
                    IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.Fire);
                    //CharacterControl.AnimationManager.Fire();
                    break;
                }
                case WeaponType.Paired:
                    IngameManager.Instance.Player.CmdChangeAnimation(AnimationPlayer.SecondaryShot);
                    //CharacterControl.AnimationManager.SecondaryShot();
                    break;
            }
        }
        public virtual void OnAttackExit()
        {
        }
        private void ResetControl()
        {
            ChangeState(ControlState.Instance);
        }
        #endregion
    }
}