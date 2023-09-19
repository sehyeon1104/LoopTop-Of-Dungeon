using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel = null;
    [SerializeField]
    private Image screenShotImage = null;
    [SerializeField]
    private TextMeshProUGUI playTimeTMP = null;
    [SerializeField]
    private GameObject earnItems = null;
    [SerializeField]
    private TextMeshProUGUI backTMP = null;
    private float fadeInOutDelay = 1f;
    private WaitForSeconds waitFadeDelay;

    private GameObject itemUITemplate = null;

    private bool isShow = false;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (isShow)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BackToCenterScene();
        }
    }

    private void Init()
    {
        itemUITemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemUI.prefab");
        waitFadeDelay = new WaitForSeconds(fadeInOutDelay);
    }

    public void ToggleGameOverPanel()
    {
        panel.SetActive(!panel.activeSelf);
        ShowGameOverContent();
    }

    public void UpdateValue()
    {
        screenShotImage.sprite = ScreenShotManager.Instance.ScreenshotToSprite();
    }

    public void ShowGameOverContent()
    {
        playTimeTMP.SetText($"플레이 타임 : {GamePlayTimerManager.Instance.GetTimer()}");
        AddCurItemSlot();
        StartCoroutine(FadeInOutBackTMP());
        isShow = true;
    }

    public void AddCurItemSlot()
    {
        foreach(var item in ItemManager.Instance.GetCurItemDic().Values)
        {
            GameObject itemUI = Instantiate(itemUITemplate, earnItems.transform);
            itemUI.GetComponent<Button>().enabled = false;
            Image itemIcon = itemUI.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
        }
    }

    private IEnumerator FadeInOutBackTMP()
    {
        while (true)
        {
            backTMP.DOFade(1f, fadeInOutDelay);
            yield return waitFadeDelay;
            backTMP.DOFade(0f, fadeInOutDelay);
            yield return waitFadeDelay;
        }
    }

    public void BackToCenterScene()
    {
        SaveManager.DeleteAllData();
        Managers.Scene.LoadScene(Define.Scene.Center);
    }
}
