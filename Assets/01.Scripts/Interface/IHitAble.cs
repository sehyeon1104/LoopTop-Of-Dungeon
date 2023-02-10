using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitAble
{
    public void GetHit(float damage, GameObject damageDealer, float critChance);
}
