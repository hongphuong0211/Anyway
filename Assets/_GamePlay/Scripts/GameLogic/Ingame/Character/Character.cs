using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Assets.HeroEditor4D.Common.Scripts.ExampleScripts;
using UnityEngine;
namespace GamePlay{
public class Character : Creature
{
    public int indexChar;
    public Material m_CharacterMaterial;
    public FirearmFxExample FirearmFx;
    public const float ATTACK_TIME = 1.8f;
    public const float BUFF_ATTACK_SPEED = 1.5f;
    protected CharacterDataConfig m_CharacterDataConfig;
    protected BigNumber m_Damage;

    public float AttackRange => m_Type == CharacterType.SURVIVOR ? 0.4f : 10f;

    protected Vector2 m_InputMovement;
    public CharacterType m_Type;

    protected bool IsRunning;

    public GameObject highlight;

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

    public virtual void InitCharacter(CharacterDataConfig characterData) {
        m_CharacterDataConfig = characterData;
        m_MaxHP = 100;
        //m_MaxHP = 100000;
        m_HP = m_MaxHP;
        m_Type = characterData.characterType;
        m_Movement.maxSpeed = characterData.moveSpeed;
        m_Movement.minSpeed = characterData.minSpeed;
        m_Movement.moveSpeed = characterData.maxSpeed;
        GameObject control = Instantiate(Resources.Load<Character4D>("Character/" + (m_Type == CharacterType.SURVIVOR?"Survivor/":"Hunter/") + characterData.name),transform).gameObject;
        if (control != null)
        {
            Destroy(CharacterControl.gameObject);
            CharacterControl = control.GetComponent<Character4D>();
            CharacterControl.Parts.ForEach(i =>
            {
                i.DefaultMaterial = m_CharacterMaterial;
                i.Initialize();
            });
        }
        RefreshAllStats();
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

    public void OnHit(Character owner, BigNumber damage, int index)
    {
        if (!IsDead())
        {
            Debug.Log(m_HP + " " + damage.ToString());
            m_HP -= damage;
            CharacterControl.AnimationManager.Hit();
            if (IsDead())
            {
                m_HP = 0;
                Death();
            }
            UI_Game.Instance.GetUI<UICGamePlay>(UIID.UICGamePlay).OnChangeStatus(index, -damage.ToFloat());
        }
    }


    public void Death()
    {
        //TODO: Add vfx
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
    public virtual void OnControlExit() {
        isControling = false;
        m_Rigidbody.velocity = Vector2.zero;
    }
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
    public bool isControling = false;

    public void SetDirection(Vector2 direction)
    {
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
}
}