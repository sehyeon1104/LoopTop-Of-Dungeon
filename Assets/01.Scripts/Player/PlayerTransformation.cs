using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoSingleton<PlayerTransformation> 
{
    [Space]
    [Header("변신")]
    [SerializeField]
     List<PlayerSkillData> playerTransformDataSOArr; // 모든 변신 데이터
    [SerializeField]
    PlayerSkillData playerTransformDataSO;     // 현재 변신 데이터
    private void Awake()
    {
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        playerTransformDataSO = playerTransformDataSOArr[0];
    }
    public PlayerSkillData GetPlayerData(int indexAilen) => playerTransformDataSOArr[indexAilen];
}
