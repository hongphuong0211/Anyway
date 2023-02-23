using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : Singleton<AttackState>, IState<Character>
{
    public void OnEnter(Character t)
    {
        ((Hunter) t).OnAttackStart();
    }

    public void OnExecute(Character t)
    {
        ((Hunter) t).OnAttackExecute();
    }

    public void OnExit(Character t)
    {
        ((Hunter) t).OnAttackExit();
    }
}
