using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossVolume : MonoBehaviour
{
    Volume volume;
    [SerializeField] VolumeProfile[] profiles;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }
    private void Start()
    {
        volume.profile = profiles[(int)GameManager.Instance.mapTypeFlag];
    }
}
