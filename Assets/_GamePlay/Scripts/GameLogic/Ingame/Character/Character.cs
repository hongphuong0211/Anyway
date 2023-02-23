using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor4D.Common.CharacterScripts;
using HeroEditor4D.Common.Enums;
using UnityEngine;
using UnityEngine.AI;

public class Character : Creature
{
    public const float ATTACK_TIME = 1.8f;
    public const float BUFF_ATTACK_SPEED = 1.5f;
    public CharacterDataConfig m_CharacterDataConfig;
    public float m_RotateSpeed = 15;
    protected BigNumber m_Hp = 100;
    protected BigNumber m_Damage;
    public BigNumber m_Coin;

    public float AttackRange => m_Type == CharacterType.SURVIVOR ? 0.4f : 10f;

    public bool IsDeath => m_Hp <= 0;
    protected Vector2 m_InputMovement;
    public CharacterType m_Type;

    protected bool IsRunning;

    public GameObject highlight;

    CharacterData characterData;

    protected IState<Character> currentState;
    public IState<Character> CurrentState
    {
        get
        {
            if (currentState == null)
            {
                currentState = IdleState.Instance;
            }
            return currentState;
        }
    }


    public virtual void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

    }

    public void SetTeam(IngameType id)
    {
        this.Ingame_ID = id;
    }

    public void SetData(CharacterData characterData)
    {
        this.characterData = characterData;
    }
    public void SetInputMovement(Vector2 movement)
    {
        m_InputMovement = movement;
    }
    public override void OnInit()
    {
        base.OnInit();
        IngameEntityManager.Instance.RegisterEntity(this);
        CharacterControl.AnimationManager.SetState(CharacterState.Idle);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        IngameEntityManager.Instance.UnregisterEntity(this);
        SimplePool.Despawn(this);
    }

    public void ChangeState(IState<Character> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void OnPlay()
    {
    }

    public void OnHit(Character owner, BigNumber damage)
    {
        if (!IsDeath)
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(BigNumber damage)
    {
        Debug.Log(m_Hp + " " + damage.ToString());
        m_Hp -= damage;
        CharacterControl.AnimationManager.Hit();
        if (IsDeath)
        {
            m_Hp = 0;
            Death();
        }

    }

    public void Death()
    {
        //TODO: Add vfx
        Debug.Log("Die");
        IngameEntityManager.Instance.UnregisterEntity(this);
        CharacterControl.AnimationManager.Die();
        //OnDespawn();
        ChangeState(null);
    }



    private Character target;

    private Character SeekNearestTarget()
    {
        return IngameEntityManager.Instance.GetNearestObject<Character>(Transform.position, CompetitorType());
    }

    private IngameType CompetitorType()
    {
        IngameType type = IngameType.SURVIVOR;
        switch (Ingame_ID)
        {
            case IngameType.SURVIVOR:
                type = IngameType.HUNTER;
                break;

            case IngameType.HUNTER:
                type = IngameType.SURVIVOR;
                break;
        }

        return type;
    }

    public void OnWin()
    {
        ChangeState(null);
    }


    #region State Machine Method

    private float m_PauseDetectTime = 0.5f;
    public override void OnRunning()
    {
        if (IsDead()) return;
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.B))
        {
            m_Animator.SetBool("IsInCombat", true);
            m_Animator.SetTrigger("Fire");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_Animator.SetBool("IsInCombat", false);
            m_Animator.SetTrigger("Fire");
        }
#endif
    }
    // Character Idle State
    public virtual void OnIdleStart()
    {
        CharacterControl.AnimationManager.SetState(CharacterState.Idle);
    }
    public virtual void OnIdleExecute()
    {

    }
    public virtual void OnIdleExit() { }
    // Character Dead State
    public virtual void OnDeadStart() { }
    public virtual void OnDeadExecute() { }
    public virtual void OnDeadExit() { }
    // End Character Dead State

    // Character Wait State
    public virtual void OnWaitStart() { }
    public virtual void OnWaitExecute() { }
    public virtual void OnWaitExit() { }
    // End Character Wait State

    // Character Control State
    public virtual void OnControlStart() { }
    public virtual void OnControlExecute()
    {
        if (IsDead()) return;
        IsRunning = m_InputMovement.x != 0f || m_InputMovement.y != 0f;
        Move();
    }
    public virtual void OnControlExit() { }
    // End Character Control State
    #endregion
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        switch (m_Type)
        {
            case CharacterType.SURVIVOR:
                Gizmos.DrawWireSphere(Transform.position, AttackRange);
                break;
            case CharacterType.HUNTER:
                break;
            default:
                break;
        }
    }
#endif
    public Character4D CharacterControl;
    private Vector2 curDirection;

    public void SetDirection(Vector2 direction)
    {
        if (direction == curDirection)
        {
            return;
        }
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            CharacterControl.Parts[0].gameObject.SetActive(false);
            CharacterControl.Parts[1].gameObject.SetActive(false);
            CharacterControl.Parts[2].gameObject.SetActive(direction.x < 0);
            CharacterControl.Parts[3].gameObject.SetActive(direction.x >= 0);
        }
        else
        {
            CharacterControl.Parts[2].gameObject.SetActive(false);
            CharacterControl.Parts[3].gameObject.SetActive(false);
            CharacterControl.Parts[0].gameObject.SetActive(direction.y < 0);
            CharacterControl.Parts[1].gameObject.SetActive(direction.y >= 0);
        }
        CharacterControl.SetDirection(direction);
    }

    private void Move()
    {

        var direction = m_InputMovement;

        if (!IsRunning)
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Idle);
            m_Rigidbody.velocity = Vector2.zero;
        }
        else
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Run);
            m_Rigidbody.velocity = m_InputMovement.normalized * Movement.GetMoveSpeed() * 0.75f;
        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CharacterControl.AnimationManager.Slash(CharacterControl.WeaponType == WeaponType.Melee2H);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            CharacterControl.AnimationManager.Jab();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterControl.AnimationManager.SecondaryShot();
        }
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Idle);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Ready);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Walk);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Run);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Jump);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterControl.AnimationManager.SetState(CharacterState.Climb);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CharacterControl.AnimationManager.Die();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CharacterControl.AnimationManager.Hit();
        }
    }
}


