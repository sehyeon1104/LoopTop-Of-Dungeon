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

        dissolveMat.SetFloat("_Alpha", -0.55f);
        alphaSet = -0.55f;

        beam.SetPosition(1, Vector3.zero);
    }

    private void CheckPlayer()
    {
        Collider2D hit = Physics2D.OverlapBox(beam.transform.position, new Vector2(length, width), transform.eulerAngles.z, 1 << 8);

        if(hit != null)
        {
            Debug.Log("플레이어 충돌!!");
            GameManager.Instance.Player.OnDamage(15, 0);
        }
    }

    private IEnumerator OnBeam()
    {
        yield return waitTime;

        while(beamLight.intensity <= intensity)
        {
            beamLight.intensity += Time.deltaTime * 10f;
            yield return null;
        }

        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);

        for(int i = 0; i < startFXList.Count; i++)
        {
            startFXList[i].Play();
        }
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Beam.wav");

        ShowRange.SetActive(false);
        CheckPlayer();
        while (lineLength <= length)
        {
            lineLength += Time.deltaTime * 100f;
            beam.SetPosition(1, new Vector3(lineLength, 0, 0));

            yield return null;
        }
        yield return new WaitForSeconds(0.25f);

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
