using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnDown : MonoBehaviour
{
   [SerializeField] AnimationCurve curve;

    float Time = 0;
    void Start()
    {
        
    }
    void Move()
    {
        curve.Evaluate(Time);
    }
}
