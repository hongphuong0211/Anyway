using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Character
{
    private Player currentTarget;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IngameManager.Instance.GetCharacter() == this && !CurrentState.Equals(AttackState.Instance))
        {
            currentTarget = other.gameObject.GetComponent<Player>();
            if (currentTarget != null && currentTarget.Character is Survivor)
            {
                UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveAttack(true, Attack);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IngameManager.Instance.GetCharacter() == this && currentTarget != null && currentTarget == other.gameObject.GetComponent<Player>())
        {
            UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).ActiveAttack(false);
            currentTarget = null;
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
            CharacterControl.AnimationManager.Jab();
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
    public virtual void OnAttackExit()
    {
    }
    private void ResetControl(){
        ChangeState(ControlState.Instance);
    }
    #endregion
}
