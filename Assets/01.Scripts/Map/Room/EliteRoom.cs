using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRoom : RoomBase
{
    private GameObject portalMapIcon = null;

    private void Start()
    {
        InstantiateMoveMapIcon();
    }

    protected override void IsClear()
    {
        // TODO : 엘리트 몬스터 사망 시 클리어 처리
    }

    public void InstantiateMoveMapIcon()
    {
        Debug.Log("아이콘 생성");
        portalMapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/PortalMapIcon.prefab");
        portalMapIcon.transform.position = transform.position;
        portalMapIcon.SetActive(false);
    }

    protected override void ShowIcon()
    {
        portalMapIcon.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // TODO : 엘리트 몬스터 등장 전 위험표시 및 엘리트 몬스터전 추가

        EnemySpawnManager.Instance.SpawnEliteMonster(transform);
    }
}
