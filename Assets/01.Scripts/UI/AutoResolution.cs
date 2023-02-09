using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResolution : MonoBehaviour
{
    [SerializeField]
    private float fixedAspectRatioWidth = 16f;
    [SerializeField]
    private float fixedAspectRatioHeight = 9f;

    private float fixedAspectRatio = 0f;

    private float currentAspectRatio = (float)Screen.width / (float)Screen.height;

    [SerializeField]
    private CanvasScaler thisCanvas;

    private void Awake()
    {
        thisCanvas = GetComponent<CanvasScaler>();
        fixedAspectRatio = fixedAspectRatioWidth / fixedAspectRatioHeight;
    }

    private void Start()
    {
        //���� �ػ� ���� ������ �� �� ���
        if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 0f;
        //���� �ػ��� ���� ������ �� �� ���
        else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1f;
        else thisCanvas.matchWidthOrHeight = 0.5f;
    }


}
