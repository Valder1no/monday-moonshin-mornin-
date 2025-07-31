using UnityEngine;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public List<DeliveryZone> deliveryZones;
    public DeliveryCardUI deliveryCardUI;

    private DeliveryZone currentZone;

    void Start()
    {
        StartDeliveryRound();
    }

    void StartDeliveryRound()
    {
        // Pick random delivery zone
        currentZone = deliveryZones[Random.Range(0, deliveryZones.Count)];
        currentZone.Activate();

        // Show the card UI
        deliveryCardUI.Show(currentZone.houseName); // e.g., "Blue House"
    }

    public void CompleteDelivery()
    {
        //currentZone.Deactivate();
        //deliveryCardUI.Hide();
        StartDeliveryRound();

        Debug.Log("Cake delivered to " + currentZone.houseName + "!");
        // Optionally: End game, score up, restart, etc.
    }
}

