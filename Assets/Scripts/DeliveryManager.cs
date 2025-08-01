using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    public List<DeliveryZone> deliveryZones;
    public DeliveryCardUI deliveryCardUI;

    public TMPro.TextMeshProUGUI cakesDeliverText;
    public TMPro.TextMeshProUGUI timeLeftToDelivery;

    private DeliveryZone currentZone;
    private int cakesDelivered = 0;

    [Header("Timer Settings")]
    public float deliveryTimeLimit = 320f;
    private Coroutine deliveryTimerCoroutine;

    void Start()
    {
        StartDeliveryRound();
    }

    void StartDeliveryRound()
    {
        if (currentZone != null)
        {
            currentZone.Deactivate();
        }

        currentZone = deliveryZones[Random.Range(0, deliveryZones.Count)];
        cakesDeliverText.text = $"Syrups delivered: {cakesDelivered}";

        currentZone.Activate();
        deliveryCardUI.Show(currentZone.houseName, currentZone.houseSprite);

        // Start countdown
        if (deliveryTimerCoroutine != null) StopCoroutine(deliveryTimerCoroutine);
        deliveryTimerCoroutine = StartCoroutine(DeliveryTimer());
    }

    public void CompleteDelivery()
    {
        if (deliveryTimerCoroutine != null) StopCoroutine(deliveryTimerCoroutine);

        deliveryTimeLimit = deliveryTimeLimit - 13f;

        cakesDelivered++;
        Debug.Log("Cake delivered to " + currentZone.houseName + "!");

        StartCoroutine(HandleSuccessfulDelivery());

        StartDeliveryRound();
    }

    private IEnumerator HandleSuccessfulDelivery()
    {
        yield return deliveryCardUI.FlashSuccess();
        StartDeliveryRound();
    }

    private IEnumerator DeliveryTimer()
    {
        float timeLeft = deliveryTimeLimit;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            timeLeftToDelivery.text = $"{timeLeft}";
            yield return null;
        }

        // Time's up — flicker red and reroll
        yield return deliveryCardUI.FlashMissed();

        StartDeliveryRound();
    }
}
