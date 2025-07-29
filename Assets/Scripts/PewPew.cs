
using UnityEngine;

[System.Serializable]
public class PickupableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName = "Item";
    public Sprite itemIcon;

    void Start()
    {
        // Ensure the object has a collider
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // Add rigidbody if it doesn't exist
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }
}