using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform trans;
    public Transform Transform
    {
        get
        {
            if (trans == null)
            {
                trans = gameObject.transform;
            }
            return trans;
        }
    }
    public IngameType Ingame_ID;
    public PoolType poolType;

    public void Start()     
    {
        OnInit();
    }
    public virtual void OnInit() { }
    public virtual void OnDespawn(){}
}