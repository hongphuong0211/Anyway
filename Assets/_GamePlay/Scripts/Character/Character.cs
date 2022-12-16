using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : GameUnit
{
    public const float ATTACK_TIME = 1.8f;
    public const float BUFF_ATTACK_SPEED = 1.5f;

    protected BigNumber m_Hp;
    protected BigNumber m_Damage;
    public BigNumber m_Coin;

    public float AttackRange => m_Type == CharacterType.SURVIVOR ? 0.4f : 10f;

    public bool IsDeath => m_Hp <= 0;

    public CharacterType m_Type;
    public int m_Level;

    [Header("Stat")]
    public Vector3 hpOffset = Vector3.up;
    public NavMeshAgent agent;

    CharacterData characterData;

    private IState<Character> currentState;

    public float BodySize => agent.radius;

    [Header("Anim")]
    public Animator animator;

    public virtual void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

    }

    public void SetTeam(IngameType id)
    {
        this.ID = id;
    }

    public void SetData(CharacterData characterData)
    {
        this.characterData = characterData;
    }

    public override void OnInit()
    {
        base.OnInit();

        IngameEntityManager.Instance.RegisterEntity(this);

        m_Hp = characterData.HitPoint;
        m_Damage = characterData.Damage;
        m_Coin = characterData.CoinGain;

        //TODO: update lai cac chi so UI cac thu

        ChangeState(IdleState.Instance);

        agent.enabled = false;



        ////cai nay dang ra phai ke thua va override nhu ma game be nen ke me no
        //if (ID == IngameType.ENEMY)
        //{
        //    LevelManager.Instance.AddEnemyHPMax(m_Hp);
        //}

        //if (weaponHand != null) weaponHand.SetActive(true);
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
        m_Hp -= damage;

        if (IsDeath)
        {
            m_Hp = 0;
            Death();
        }

    }

    public void Death()
    {
        //TODO: Add vfx
        IngameEntityManager.Instance.UnregisterEntity(this);
        //OnDespawn();
        ChangeState(null);
        OnChangeAnim(Constant.ANIM_DIE);

        agent.enabled = false;

        //switch (m_Type)
        //{
        //    case CharacterType.MELEE:
        //        if (Random.Range(0, 2) == 0)
        //        {
        //            SoundManager.Instance.PlayFx(FxID.MeleeDeath_1);
        //        }
        //        else
        //        {
        //            SoundManager.Instance.PlayFx(FxID.MeleeDeath_2);
        //        }
        //        break;
        //    case CharacterType.RANGE:
        //        if (Random.Range(0, 2) == 0)
        //        {
        //            SoundManager.Instance.PlayFx(FxID.RangeDeath_1);
        //        }
        //        else
        //        {
        //            SoundManager.Instance.PlayFx(FxID.RangeDeath_2);
        //        }
        //        break;
        //}
    }

    public void OnChangeAnim(string animName)
    {
        animator?.SetTrigger(animName);
    }

    public void SetTarget(Vector3 point)
    {
        agent.SetDestination(point);
    }


    private Character target;

    private Character SeekNearestTarget()
    {
        return IngameEntityManager.Instance.GetNearestObject<Character>(tf.position, CompetitorType());
    }

    private IngameType CompetitorType()
    {
        IngameType type = IngameType.SURVIVOR;
        switch (ID)
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
        agent.isStopped = true;
        OnChangeAnim(Constant.ANIM_IDLE);
    }


    //#region Attack

    //CounterTime counterTime = new CounterTime();
    //public float attackDelayAnim = 0.2f;

    //public bool IsTargetInRange => Vector3.Distance(tf.position, target.tf.position) <= BodySize + target.BodySize + AttackRange;

    //public virtual void AttackEnter()
    //{
    //    if (target == null || target.IsDeath || !IsTargetInRange)
    //    {
    //        ChangeState(PatrolState.Instance);
    //        return;
    //    }
    //    OnChangeAnim(Constant.ANIM_ATTACK);
    //    counterTime.CounterStart(null, AttackAction, (IsMeleeBuff || IsRangerBuff) ? attackDelayAnim / BUFF_ATTACK_SPEED : attackDelayAnim);
    //    agent.isStopped = true;
    //    tf.LookAt(target.tf);

    //    if (weaponHand != null) weaponHand.SetActive(true);

    //}

    //public virtual void AttackExecute()
    //{
    //    counterTime.CounterExecute();
    //}

    //public virtual void AttackAction()
    //{
    //    if (target == null || target.IsDeath)
    //    {
    //        ChangeState(PatrolState.Instance);
    //    }
    //    else
    //    {
    //        counterTime.CounterStart(null, AttackEnter, (IsMeleeBuff || IsRangerBuff) ? (ATTACK_TIME - attackDelayAnim) / BUFF_ATTACK_SPEED : (ATTACK_TIME - attackDelayAnim));

    //        //target.OnHit(damage);
    //        //BigNumber damage = (IsContainBuff(SpinResultType.BOWx2) && m_Type == CharacterType.RANGE) || (IsContainBuff(SpinResultType.SWORDx2) && m_Type == CharacterType.MELEE) ? this.m_Damage * 1.5f : this.m_Damage;
    //        BigNumber coin = IsContainBuff(SpinResultType.COINx2) ? this.m_Coin * 2f : this.m_Coin;
    //        coin = IsContainBuff(SpinResultType.COINx3) ? this.m_Coin * 3f : this.m_Coin;
    //        weapon.Attack(this, target, this.m_Damage, coin);

    //        if (weaponHand != null) weaponHand.SetActive(false);
    //    }

    //}

    //#endregion

    //#region Patrol

    //public virtual void PatrolEnter()
    //{
    //    agent.enabled = true;
    //    agent.isStopped = false;

    //    target = SeekNearestTarget();

    //    if (target == null || target.IsDeath)
    //    {
    //        PatrolEnter();
    //    }
    //    else
    //    if (IsTargetInRange)
    //    {
    //        ChangeState(AttackState.Instance);
    //        return;
    //    }

    //    OnChangeAnim(Constant.ANIM_RUN);

    //    counterTime.CounterStart(null, () => target = SeekNearestTarget(), 0.5f);
    //}

    //public virtual void PatrolExecute()
    //{
    //    counterTime.CounterExecute();

    //    if (target == null || target.IsDeath)
    //    {
    //        ChangeState(PatrolState.Instance);
    //    }
    //    else if (IsTargetInRange)
    //    {
    //        ChangeState(AttackState.Instance);
    //    }
    //    else
    //    {
    //        agent.SetDestination(target.tf.position);
    //    }
    //}

    //#endregion

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        switch (m_Type)
        {
            case CharacterType.SURVIVOR:
                Gizmos.DrawWireSphere(tf.position, AttackRange);
                break;
            case CharacterType.HUNTER:
                break;
            default:
                break;
        }
    }
#endif

}


