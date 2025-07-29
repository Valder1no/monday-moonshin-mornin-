using UnityEngine;

public class TableGameController : MonoBehaviour
{

    [SerializeField] private MonoBehaviour playerController;
    private bool playerShouldAlive = true;
    private void Update()
    {
        if (playerShouldAlive)
        {
            playerController.enabled = false;
        }
    }
}
