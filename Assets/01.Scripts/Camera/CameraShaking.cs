using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaking : MonoSingleton<CameraShaking>
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

    // 디버깅용
    public void CameraShakeAtk()
    {
        //StartCoroutine(IECameraShakeOnce());
    }

    /// <summary>
    /// 고정된 세기
    /// </summary>
    /// <returns></returns>
    public IEnumerator IECameraShakeOnce()
    {

        cinemachineCam.enabled = false; // 시네머신 off
        initPos = cam.transform.position;
        // 초기 위치

        postPos = Random.insideUnitCircle * shakingAmount + initPos;
        postPos.z = -10f;

        cam.transform.position = postPos;

        // 원의 방향중 랜덤 좌표 지정 * 세기 + 초기 위치
        // 카메라의 위치 = 흔들린 카메라의 위치

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);
        // 카메라 쉐이킹 후 카메라의 위치 복구

        cinemachineCam.enabled = true;

        yield break;
    }

    /// <summary>
    /// 세기 조절 가능
    /// </summary>
    /// <param name="shakingAmount"></param>
    /// <returns></returns>
    public IEnumerator IECameraShakeOnce(float shakingAmount)
    {

        cinemachineCam.enabled = false; // 시네머신 off
        initPos = cam.transform.position;
        // 초기 위치

        postPos = Random.insideUnitCircle * shakingAmount + initPos;
        postPos.z = -10f;

        cam.transform.position = postPos;

        // 원의 방향중 랜덤 좌표 지정 * 세기 + 초기 위치
        // 카메라의 위치 = 흔들린 카메라의 위치

        yield return new WaitForSeconds(0.01f);
        cam.transform.position = new Vector3(initPos.x, initPos.y, -10f);
        // 카메라 쉐이킹 후 카메라의 위치 복구

        cinemachineCam.enabled = true;

        yield break;
    }

    /// <summary>
    /// 일정 시간동안 카메라 흔듬, 고정된 세기
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
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
