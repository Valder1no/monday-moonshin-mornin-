using System.Collections.Generic;
using UnityEngine;

public class ResourceTriggerZone2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTargetPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private MonoBehaviour miniGameScript;
    [SerializeField] private MiniGameBowlController2 bowlController2;

    private readonly List<GameObject> resources = new();
    private string lastResourceTag = "Flour";
    private int matchingResourceCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (matchingResourceCount == 0)
        {
            lastResourceTag = tag;
            matchingResourceCount = 1;
            resources.Add(other.gameObject);
        }
        else if (tag == lastResourceTag)
        {
            matchingResourceCount++;
            resources.Add(other.gameObject);

            if (matchingResourceCount == 2)
            {
                DestroyResources();
                StartMiniGame();
            }
        }
        else
        {
            ResetResourceTracking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (resources.Remove(other.gameObject))
        {
            matchingResourceCount = resources.Count;
        }
    }

    private void DestroyResources()
    {
        foreach (GameObject obj in resources)
        {
            Destroy(obj);
        }
        ResetResourceTracking();
    }

    private void ResetResourceTracking()
    {
        resources.Clear();
        matchingResourceCount = 0;
        lastResourceTag = "Flour";
    }

    private void StartMiniGame()
    {
        bowlController2.gameManagerRef = this;
        bowlController2.Activate();

        if (miniGameScript != null) miniGameScript.enabled = true;
        if (playerController != null) playerController.enabled = false;

        StartCoroutine(MoveCameraToTable());
    }

    public void ExitMiniGame()
    {
        if (miniGameScript != null) miniGameScript.enabled = false;
        if (playerController != null) playerController.enabled = true;

        ResetResourceTracking();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private System.Collections.IEnumerator MoveCameraToTable()
    {
        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;

        Vector3 targetPos = cameraTargetPoint.position;
        Quaternion targetRot = cameraTargetPoint.rotation;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            player.transform.position = Vector3.Lerp(startPos, targetPos, t);
            player.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos;
        player.transform.rotation = targetRot;
    }
}
