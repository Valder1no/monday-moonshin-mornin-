using UnityEngine;

public class FlourReceiver : MonoBehaviour
{
    [SerializeField] private Transform fillContainer;
    [SerializeField] private GameObject flourChunkPrefab;
    [SerializeField] private float fillRate = 0.5f; // Time between chunks

    private bool isFilling = false;
    private float fillTimer = 0f;

    public void StartFilling()
    {
        isFilling = true;
    }

    public void StopFilling()
    {
        isFilling = false;
    }

    void Update()
    {
        if (!isFilling) return;

        fillTimer += Time.deltaTime;
        if (fillTimer >= fillRate)
        {
            fillTimer = 0f;
            GameObject flour = Instantiate(flourChunkPrefab, fillContainer.position, Quaternion.identity);
            flour.transform.SetParent(fillContainer); // Keeps it organized
        }
    }
}
