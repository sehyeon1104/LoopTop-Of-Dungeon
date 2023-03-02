using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIMove : MonoBehaviour
{
    private void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(Boss.Instance.transform.position);
        transform.position = screenPos;
    }
}
