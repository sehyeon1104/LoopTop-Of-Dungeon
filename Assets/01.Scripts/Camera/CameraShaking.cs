using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaking : MonoBehaviour
{
    [SerializeField]
    private float shakingAmount = 0.5f;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCam;

    private Camera cam;
    private Vector2 initPos;
    private Vector3 postPos;

    private float shakeTime = 0f;

    private void Start()
    {
        cam = Camera.main;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        StartCoroutine(IECameraShakeOnce());
    //    }
    //}

    // ������
    public void CameraShakeAtk()
    {
        StartCoroutine(IECameraShakeOnce());
    }

    public IEnumerator IECameraShakeOnce()
    {

        initPos = cam.transform.position;
        // �ʱ� ��ġ
        cinemachineCam.enabled = false;

        postPos = Random.insideUnitCircle * shakingAmount + initPos;
        postPos.z = -10f;

        cam.transform.position = postPos;

        // ���� ������ ���� ��ǥ ���� * ���� + �ʱ� ��ġ
        // ī�޶��� ��ġ = ��鸰 ī�޶��� ��ġ

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);
        // ī�޶� ����ŷ �� ī�޶��� ��ġ ����

        cinemachineCam.enabled = true;

        yield break;
    }

    public IEnumerator IECameraShakeMultiple(float time)
    {
        shakeTime = time;

        initPos = cam.transform.position;

        while(shakeTime > 0f)
        {
            postPos = Random.insideUnitCircle * shakingAmount + initPos;
            postPos.z = -10f;

            cam.transform.position = postPos;
            shakeTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        shakeTime = 0f;

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);

        yield break;
    }

}
