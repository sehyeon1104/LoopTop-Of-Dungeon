using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoSingleton<PlayerTransformation> 
{
    [Space]
    [Header("����")]
    [SerializeField]
     List<PlayerSkillData> playerTransformDataSOArr; // ��� ���� ������
    [SerializeField]
    PlayerSkillData playerTransformDataSO;     // ���� ���� ������
    private void Awake()
    {
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        playerTransformDataSO = playerTransformDataSOArr[0];
    }
    public PlayerSkillData GetPlayerData(int indexAilen) => playerTransformDataSOArr[indexAilen];
}
