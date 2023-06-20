using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRoom : RoomBase
{
    private GameObject portalMapIcon = null;

    private bool isEliteMonsterSpawn = false;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    private void Start()
    {
        InstantiateMoveMapIcon();
        MoveNextMapPortal = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Maps/Magic_Circle_Move.prefab");
    }

    protected override void IsClear()
    {
        // TODO : 엘리트 몬스터 사망 시 클리어 처리
        if(isEliteMonsterSpawn && EnemySpawnManager.Instance.curEnemies.Count == 0)
        {
            isClear = true;
            AssignMoveNextMapPortal();
            Door.Instance.OpenDoors();
        }
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

        if (collision.CompareTag("Player") && !isEliteMonsterSpawn)
        {
            isEliteMonsterSpawn = true;
            // TODO : 엘리트 몬스터 등장 전 위험표시 및 엘리트 몬스터전 추가

            Door.Instance.CloseDoors();
            EnemySpawnManager.Instance.SpawnEliteMonster(transform);
        }
    }

    public void AssignMoveNextMapPortal()
    {
        Instantiate(MoveNextMapPortal, transform.position, Quaternion.identity);
    }
}
