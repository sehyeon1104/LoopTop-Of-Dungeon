using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    [SerializeField]
    private float shakingAmount = 2f;

    private Camera cam;
    private Vector2 initPos;

    private float shakeTime = 0f;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(CameraShake(0.01f));
        }
    }

    public IEnumerator CameraShake(float time)
    {
        shakeTime = time;

        initPos = cam.transform.position;

        //while(shakeTime > 0f)
        //{
        cam.transform.position = Random.insideUnitCircle * shakingAmount + initPos;
        //}

        shakeTime = 0f;

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);

    }

}
