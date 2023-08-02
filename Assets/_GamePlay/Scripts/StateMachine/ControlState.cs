using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay
{
    public class ControlState : Singleton<ControlState>, IState<Character>
    {
        public void OnEnter(Character t)
        {
            //t.OnChangeAnim(Constant.ANIM_CONTROL);
            t.OnControlStart();
        }

        public void OnExecute(Character t)
        {
            t.OnControlExecute();
        }

        public void OnExit(Character t)
        {
            t.OnControlExit();
        }
    }
}
