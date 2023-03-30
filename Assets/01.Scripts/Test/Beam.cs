using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Beam : MonoBehaviour
{
    [SerializeField] LayerMask pLayer;
    [SerializeField] GameObject startFX;
    [SerializeField] LineRenderer beam;
    [SerializeField] Light2D beamLight;
    [SerializeField] GameObject ShowRange;

    EdgeCollider2D col;
    Vector2[] points;

    float lineLength = 0.0f;
    float lineWidth = 0.5f;
    Vector3 tempScale;

    [SerializeField] float length;
    [SerializeField] float width;
    [SerializeField] float intensity = 2f;

    WaitForSeconds waitTime = new WaitForSeconds(0.4f);
    List<ParticleSystem> startFXList = new List<ParticleSystem>();

    private void Awake()
    {
        foreach(ParticleSystem fx in startFX.GetComponentsInChildren<ParticleSystem>())
            startFXList.Add(fx);

        col = beam.GetComponent<EdgeCollider2D>();
        points = col.points;
        tempScale = transform.localScale;
    }

    private void OnEnable()
    {
        Init();
        SetObjectFlip();
        StartCoroutine(OnBeam());

        ShowRange.SetActive(true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetObjectFlip()
    {
        //tempScale.y = transform.rotation.z >= 0.7f ? -1.35f : 1.35f;
        //transform.localScale = tempScale;
    }

    private void Init()
    {
        lineLength = 0;
        lineWidth = width;

        beam.startWidth = lineWidth;
        beam.endWidth = lineWidth;

        points[1] = Vector2.zero;
        col.points = points;

        beam.SetPosition(1, Vector3.zero);
    }

    //private void CheckPlayer()
    //{
    //    var hits = Physics2D.BoxCastAll(beam.transform.position, new Vector2(length, width), transform.rotation.z, transform.forward);

    //    foreach (var col in hits)
    //    {
    //        Debug.Log($"�浹ü : {col.transform.name}");
    //        Debug.Log($"�浹ü Ʈ������ : {col.transform.position}");
    //        if(col.transform.CompareTag("Player"))
    //        {
    //            Debug.Log("�÷��̾� �浹!!");
    //            GameManager.Instance.Player.OnDamage(2, gameObject, 0);
    //        }
    //    }
    //}

    private IEnumerator OnBeam()
    {
        yield return waitTime;
        while(beamLight.intensity <= intensity)
        {
            beamLight.intensity += 0.1f;
            yield return null;
        }

        CinemachineCameraShaking.Instance.CameraShake(2, 0.4f);

        for(int i = 0; i < startFXList.Count; i++)
        {
            startFXList[i].Play();
        }

        ShowRange.SetActive(false);

        while (lineLength <= length)
        {
            lineLength += 1f;
            beam.SetPosition(1, new Vector3(lineLength, 0, 0));

            points[1].x = lineLength;
            col.points = points;

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        CinemachineCameraShaking.Instance.CameraShake(3, 0.75f);
        
        lineWidth *= 2;

        points[1] = Vector2.zero;
        col.points = points;

        while (lineWidth >= 0.0f)
        {
            lineWidth -= Time.deltaTime * 2f;

            beamLight.intensity -= 0.01f;
            beam.startWidth = lineWidth;
            beam.endWidth = lineWidth;

            yield return null;
        }

        beamLight.intensity = 0;
        beam.startWidth = 0;
        beam.endWidth = 0;
    }
}
