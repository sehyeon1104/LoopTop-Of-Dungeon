using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangePattern : MonoBehaviour
{
    [SerializeField] private GameObject FPR; //Floor Pattern Range
    [SerializeField] private GameObject FPRS; //Floor Pattern Range Start

    private WaitForSeconds WaitForRangeStart = new WaitForSeconds(2f);
    private WaitForSeconds WaitForStart = new WaitForSeconds(0.8f);
    private WaitForSeconds FPRSpeed = new WaitForSeconds(0.01f);

    private float ScaleX;
    private float ScaleY;

    private void Start()
    {
        StartCoroutine(FloorPattern());
    }

    IEnumerator  FloorPattern()
    {
        FPR.transform.position = transform.position;
        FPRS.transform.position = transform.position;

        FPR.SetActive(true);
        FPRS.SetActive(true);

        yield return WaitForRangeStart; //패턴 시전 대기

        while(ScaleX < 17f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FPR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return FPRSpeed;
        }

        ScaleX = 0;
        ScaleY = 0;

        yield return WaitForStart;

        while (ScaleX < 17f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return FPRSpeed;
        }

        //FPRS.


    }

}
