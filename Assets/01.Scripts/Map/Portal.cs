using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    private bool isLoadScene = false;
    Define.Scene sceneType;

    private bool isInteraction = false;
    Button interactionButton;
    private GameObject moveCanvas = null;
    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        interactionButton = UIManager.Instance.GetInteractionButton();
        moveCanvas = transform.Find("MoveCanvas").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        moveCanvas.gameObject.SetActive(false);
        isLoadScene = false;
        isInteraction = false;
        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Portal/{GameManager.Instance.mapTypeFlag}PortalSprite.png");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractionPlayer();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInteraction = false;
        UIManager.Instance.RotateAttackButton();
        ToggleMoveTMP();
        interactionButton.onClick.RemoveListener(MoveNextStage);
    }
    public void InteractionPlayer()
    {
        isInteraction = true;

        UIManager.Instance.RotateInteractionButton();

        ToggleMoveTMP();
        interactionButton.onClick.AddListener(MoveNextStage);
    }

    public void ToggleMoveTMP()
    {
        moveCanvas.gameObject.SetActive(isInteraction);
    }

    public void MoveNextStage()
    {
        if (!isLoadScene)
        {
            isLoadScene = true;

            GameManager.Instance.StageMoveCount++;

            if (GameManager.Instance.StageMoveCount == 0 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.BossScene;
            }
            else if(GameManager.Instance.StageMoveCount < 3 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.StageScene;
            }
            else if (GameManager.Instance.sceneType == Define.Scene.BossScene)
            {
                sceneType = Define.Scene.CenterScene;
            }
            GameManager.Instance.SetSceneType(sceneType);

            GameManager.Instance.SaveData();
            Fade.Instance.FadeInAndLoadScene(sceneType);

        }
    }

}
