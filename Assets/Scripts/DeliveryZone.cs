using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public string houseName;
    public DeliveryManager deliveryManager;
    public DeliveryCardUI deliveryCardUI;

    public Sprite houseSprite;

    private bool active = false;

    public void Activate()
    {
        active = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            if (other.CompareTag("Cake"))
            {
                Destroy(other.gameObject);
                StartCoroutine(deliveryCardUI.FlashMissed());
            }
        }

        if (!active) return;

        if (other.CompareTag("Cake"))
        {
            Destroy(other.gameObject);
            deliveryManager.CompleteDelivery();
        }
    }
}
