using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoSingleton<PlayerTransformation> 
{
    [Space]
    [SerializeField]
    List<PlayerSkillData> playerTransformDataSOArr;
    [SerializeField]
    PlayerSkillData playerTransformDataSO;
    public PlayerSkillData PlayerTransformDataSO => playerTransformDataSO;
    private void Awake()
    {
       
    }
    public PlayerSkillData GetPlayerData(Define.PlayerTransformTypeFlag playerTransformType) => playerTransformDataSOArr[(int)playerTransformType];
}
