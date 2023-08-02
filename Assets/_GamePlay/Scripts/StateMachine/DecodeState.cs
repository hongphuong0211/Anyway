using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay
{
    public class DecodeState : Singleton<DecodeState>, IState<Character>
    {
        public void OnEnter(Character t)
        {
            //t.OnChangeAnim(Constant.ANIM_CONTROL);
            ((Survivor)t).OnDecodeStart();
        }

        public void OnExecute(Character t)
        {
            ((Survivor)t).OnDecodeExecute();
        }

        public void OnExit(Character t)
        {
            ((Survivor)t).OnDecodeExit();
        }
    }
}
