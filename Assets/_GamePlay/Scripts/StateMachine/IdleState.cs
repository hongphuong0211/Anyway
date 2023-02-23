using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Singleton<IdleState>, IState<Character>
{
    public void OnEnter(Character t)
    {
        t.OnChangeAnim(Constant.ANIM_IDLE);
    }

    public void OnExecute(Character t)
    {

    }

    public void OnExit(Character t)
    {

    }

}
