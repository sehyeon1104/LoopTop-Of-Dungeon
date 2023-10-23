using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TxtSetMatInTimeline : MonoBehaviour
{
    private Material mat;

    [SerializeField] private float setMatDilate;
    [SerializeField] private float setMatTickness;
    [SerializeField] private float setMatPower;

    private void Awake()
    {
        mat = GetComponent<TextMeshProUGUI>().fontMaterial;
    }

    void Update()
    {
        mat.SetFloat("_FaceDilate", setMatDilate);
        mat.SetFloat("_OutlineTickness", setMatTickness);
        mat.SetFloat("_GlowPower", setMatPower);
    }
}
