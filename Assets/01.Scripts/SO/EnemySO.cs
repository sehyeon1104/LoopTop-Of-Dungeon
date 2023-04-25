using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy")]
public class EnemySO : ScriptableObject
{
    public float hp;
    public float damage;
    public float speed;
    public float attackSpeed;
}
