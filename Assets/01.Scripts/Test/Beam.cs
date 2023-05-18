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
    Material dissolveMat;
    List<ParticleSystem> startFXList = new List<ParticleSystem>();

    float alphaSet = -0.53f;

    private void Awake()
    {
        foreach(ParticleSystem fx in startFX.GetComponentsInChildren<ParticleSystem>())
            startFXList.Add(fx);

        col = beam.GetComponent<EdgeCollider2D>();
        points = col.points;
        dissolveMat = GetComponent<Renderer>().material;
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
        StopCoroutine(OnBeam());

        lineLength = 0;
        lineWidth = width;

        beam.startWidth = lineWidth;
        beam.endWidth = lineWidth;

        points[1] = Vector2.zero;
        col.points = points;
        col.edgeRadius = lineWidth * 0.5f;

        dissolveMat.SetFloat("_Alpha", -0.55f);
        alphaSet = -0.55f;

        beam.SetPosition(1, Vector3.zero);
    }

    //private void CheckPlayer()
    //{
    //    var hits = Physics2D.BoxCastAll(beam.transform.position, new Vector2(length, width), transform.rotation.z, transform.forward);

    //    foreach (var col in hits)
    //    {
    //        Debug.Log($"충돌체 : {col.transform.name}");
    //        Debug.Log($"충돌체 트랜스폼 : {col.transform.position}");
    //        if(col.transform.CompareTag("Player"))
    //        {
    //            Debug.Log("플레이어 충돌!!");
    //            GameManager.Instance.Player.OnDamage(2, gameObject, 0);
    //        }
    //    }
    //}

    private IEnumerator OnBeam()
    {
        yield return waitTime;

        while(beamLight.intensity <= intensity)
        {
            Debug.Log(Time.deltaTime);
            beamLight.intensity += Time.deltaTime * 10f;
            yield return null;
        }

        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);

        for(int i = 0; i < startFXList.Count; i++)
        {
            startFXList[i].Play();
        }
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Beam.wav",Define.Sound.Effect,1,0.5f);

        ShowRange.SetActive(false);

        while (lineLength <= length)
        {
            lineLength += Time.deltaTime * 100f;
            beam.SetPosition(1, new Vector3(lineLength, 0, 0));

            points[1].x = lineLength;
            col.points = points;


            yield return null;
        }

        //yield return new WaitForSeconds(0.2f);
        //CinemachineCameraShaking.Instance.CameraShake(3, 0.5f);

        //lineWidth *= 2;

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

        while (alphaSet <= 1.5f)
        {
            alphaSet += Time.deltaTime * 2;

            dissolveMat.SetFloat("_Alpha", alphaSet);
            yield return null;
        }
    }
}
