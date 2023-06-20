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
        // TODO : ����Ʈ ���� ��� �� Ŭ���� ó��
    }

    public void InstantiateMoveMapIcon()
    {
        Debug.Log("������ ����");
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
        // TODO : ����Ʈ ���� ���� �� ����ǥ�� �� ����Ʈ ������ �߰�

        EnemySpawnManager.Instance.SpawnEliteMonster(transform);
    }
}
