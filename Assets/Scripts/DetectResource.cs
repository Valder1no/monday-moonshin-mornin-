using UnityEngine;

public class ResourceTriggerZone : MonoBehaviour
{
    private bool resource1In = false;
    private bool resource2In = false;

    [SerializeField] private string resource1Tag = "Resource1";
    [SerializeField] private string resource2Tag = "Resource2";

    [SerializeField] Transform cameraTargetPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private MonoBehaviour playerController; // Assign your movement script here
    [SerializeField] private MonoBehaviour miniGameScript;   // Assign mini-game controller here

    public MiniGameBowlController bowlController;

    private int resourcesInPlace;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(resource1Tag)) resource1In = true;
        if (other.CompareTag(resource2Tag)) resource2In = true;

        TryActivateSequence();
        if (other.CompareTag(resource1Tag) || (other.CompareTag(resource2Tag)))
        {
            resourcesInPlace++;
        }

        if (resourcesInPlace >= 2)
        {
            StartMiniGame();
        }
    }

    private void StartMiniGame() 
    {
        bowlController.Activate();
        Debug.Log("game is called from detector");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(resource1Tag)) resource1In = false;
        if (other.CompareTag(resource2Tag)) resource2In = false;
        if (other.CompareTag(resource1Tag) || other.CompareTag(resource2Tag)) resourcesInPlace--;
    }

    private void TryActivateSequence()
    {
        if (resource1In && resource2In)
        {
            playerController.enabled = false;
            StartCoroutine(MoveCameraToTable());
        }
    }

    private System.Collections.IEnumerator MoveCameraToTable()
    {
        // Disable movement
        playerController.enabled = false;

        // Lerp camera to new position
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

        // Enable minigame
        if (miniGameScript != null)
            miniGameScript.enabled = true;
        playerController.enabled = false;
    }
}
