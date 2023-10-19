using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image itemImage = null;
    [SerializeField]
    private TextMeshProUGUI stackTMP = null;
    [SerializeField]
    private Image timerImage = null;

    private Coroutine timerCoroutine = null;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private Define.ItemType itemType;
    private Item item = null;

    private float timer = 0f;
    private float targetTime = 0f;

    public void SetValue(Item item)
    {
        this.item = item;
        UpdateValues();
    }

    public void UpdateValues()
    {
        itemType = item.itemType;
        itemImage.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
        timerImage.gameObject.SetActive(false);
    }

    public void UpdateStack(int stack)
    {
        stackTMP.SetText($"{stack}");
    }

    public void ToggleStackTMP()
    {
        stackTMP.gameObject.SetActive(!stackTMP.gameObject.activeSelf);
    }

    public void UpdateTimerPanel(float time)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerPanel(time));
    }

    public IEnumerator TimerPanel(float time)
    {
        timerImage.gameObject.SetActive(true);
        timer = time;
        targetTime = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerImage.fillAmount = (float)(timer / targetTime);

            yield return waitForEndOfFrame;
        }

        timerImage.gameObject.SetActive(false);
    }
}
