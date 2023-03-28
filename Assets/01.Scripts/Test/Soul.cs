using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Soul : MonoBehaviour
{
    private void OnEnable() 
    {
        transform.DOMove(Boss.Instance.transform.position, 1f);
    }
}
