using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTMPPool : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(EnemyUIManager.Instance.PoolDamageTMP(this.gameObject));
    }
}
