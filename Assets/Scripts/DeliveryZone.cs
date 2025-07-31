using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public string houseName;
    public DeliveryManager deliveryManager;

    private bool active = false;

    public void Activate()
    {
        active = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        if (other.CompareTag("Cake"))
        {
            Destroy(other.gameObject); // Remove cake
            deliveryManager.CompleteDelivery(); // Notify success
        }
    }
}
