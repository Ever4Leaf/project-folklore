using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : BaseAttack
{
    public BasicAttack ()
    {
        attackName = "Dagger Stab";
        attackDescription = "lorem ipsum";
        attackDamage = 10f;
        apCost = 0f;
        apRecover = 1f;
    }
}
