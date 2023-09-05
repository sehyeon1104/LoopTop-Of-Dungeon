using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Experimental.GraphView.GraphView; // 빌드오류로 인해 일시적 주석
public class GhostBossArmPatternAnim : MonoBehaviour
{

    public GameObject AttackRange = null;


    public Vector2 size1;
    public LayerMask Layer;

    public void CheckPlayer()
    {
        Managers.Pool.PoolManaging("SummonArm",transform.position, Quaternion.identity);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_RaiseupArm1.wav", Define.Sound.Effect, 1, 0.5f);
        Collider2D hit1 = Physics2D.OverlapBox(AttackRange.transform.position, size1, 0, Layer);

        if (hit1?.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.Player.OnDamage(1, 0);
        }
    }

    public void GhostArmDownEffect()
    {
        Managers.Pool.PoolManaging("10.Effects/ghost/GhostBossArmPatternSmoke", transform.position, Quaternion.identity);

        int randomNum = Random.Range(0, 5);
        if (randomNum >= 3)
            Managers.Pool.PoolManaging("10.Effects/ghost/Bubble", transform.position, Quaternion.identity);
        else if (randomNum >= 2)
            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BubbleBlue.prefab", transform.position, Quaternion.identity);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackRange.transform.position, size1);
    }
}
