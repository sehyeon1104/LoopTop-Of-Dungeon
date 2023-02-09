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
        //현재 해상도 가로 비율이 더 길 경우
        if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 0f;
        //현재 해상도의 세로 비율이 더 길 경우
        else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1f;
        else thisCanvas.matchWidthOrHeight = 0.5f;
    }


}
