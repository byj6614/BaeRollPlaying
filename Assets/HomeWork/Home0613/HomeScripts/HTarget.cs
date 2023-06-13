using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTarget : MonoBehaviour, IAttackAble
{
    public void AttackReceive(int damage)
    {
        Destroy(gameObject);
    }

    
}
