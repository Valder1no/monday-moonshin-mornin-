using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DeliveryCardUI : MonoBehaviour
{
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TMP_Text houseNameText;
    [SerializeField] private Image flickerOverlay;

    public UnityEngine.UI.Image houseImageUI;

    //public Sprite house1Sprite;
    //public Sprite house2Sprite;
    //public Sprite house3Sprite;

    public void Show(string houseName, Sprite houseSprite)
    {
        houseNameText.text = "Deliver to: " + houseName;
        houseImageUI.sprite = houseSprite;
        houseImageUI.enabled = true; // not needed
        cardPanel.SetActive(true);
    }

    public void Hide()
    {
        cardPanel.SetActive(false);
    }

    public IEnumerator FlashMissed()
    {
        flickerOverlay.gameObject.SetActive(true);
        flickerOverlay.color = new Color(1f, 0f, 0f, 0.4f);

        float flickerDuration = 2.2f;
        float elapsed = 0f;

        while (elapsed < flickerDuration)
        {
            float t = Mathf.PingPong(elapsed * 5f, 1f);
            flickerOverlay.color = new Color(1f, 0f, 0f, t * 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        flickerOverlay.gameObject.SetActive(false);
    }

    public IEnumerator FlashSuccess()
    {
        flickerOverlay.gameObject.SetActive(true);
        flickerOverlay.color = new Color(0f, 1f, 0f, 0.4f); // semi-transparent green

        float flickerDuration = 0.6f;
        float elapsed = 0f;

        while (elapsed < flickerDuration)
        {
            float t = Mathf.PingPong(elapsed * 5f, 1f);
            flickerOverlay.color = new Color(0f, 1f, 0f, t * 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        flickerOverlay.gameObject.SetActive(false);
    }

}
