using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDefault : MonoBehaviour
{
    protected Transform playerTransform;
    private float distanceToPlayer;

    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    void Start()
    {
        playerTransform = Player.Instance.transform;
    }


    void Update()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        switch (distanceToPlayer)
        {
            case var a when a <= detectDistance && a >= minDistance:
                MoveToPlayer();
                break;
            case var a when a < minDistance:
                AttackToPlayer();
                break;
            default:
                break;
        }
    }

    public abstract void MoveToPlayer();
    public virtual void AttackToPlayer(){}
}
