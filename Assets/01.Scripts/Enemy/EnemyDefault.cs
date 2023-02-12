using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDefault : MonoBehaviour
{
    protected Transform player;
    private float distanceToPlayer;

    [SerializeField] private float minDistance = 1f;

    void Start()
    {
        player = Player.Instance.transform;
    }


    void Update()
    {
        distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer >= minDistance)
            MoveToPlayer();
        else
            AttackToPlayer();
    }

    public abstract void MoveToPlayer();
    public virtual void AttackToPlayer(){}
}
