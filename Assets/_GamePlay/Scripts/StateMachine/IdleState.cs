using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay
{
    public class IdleState : Singleton<IdleState>, IState<Character>
    {
        public void OnEnter(Character t)
        {
            t.OnIdleStart();
        }

        public void OnExecute(Character t)
        {
            t.OnIdleExecute();
        }

        public void OnExit(Character t)
        {
            t.OnIdleExit();
        }

    }
}
