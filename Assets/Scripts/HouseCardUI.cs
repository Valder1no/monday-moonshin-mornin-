using UnityEngine;

public class DeliveryCardUI : MonoBehaviour
{
    public GameObject cardPanel;
    public TMPro.TextMeshProUGUI houseNameText;

    public void Show(string houseName)
    {
        houseNameText.text = "Deliver to: " + houseName;
        cardPanel.SetActive(true);
    }

    public void Hide()
    {
        cardPanel.SetActive(false);
    }
}
