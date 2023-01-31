using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    [SerializeField]
    private float shakingAmount = 2f;

    private Camera cam;
    private Vector2 initPos;
    private Vector3 postPos;

    private float shakeTime = 0f;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(IECameraShake(0.01f));
        }
    }

    // µð¹ö±ë¿ë
    public void CameraShakeAtk()
    {
        StartCoroutine(IECameraShake(0.01f));
    }

    public IEnumerator IECameraShake(float time)
    {
        //shakeTime = time;

        initPos = cam.transform.position;

        //while(shakeTime > 0f)
        //{
        postPos = Random.insideUnitCircle * shakingAmount + initPos;
        postPos.z = -10f;
        Debug.Log(postPos);

        cam.transform.position = postPos;
        //}

        //shakeTime = 0f;

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);

    }

}
