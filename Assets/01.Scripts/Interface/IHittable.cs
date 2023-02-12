using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    Vector3 hitPoint { get; }
    void OnDamage(float damage, GameObject damageDealer, float critChance);
}
