using UnityEngine;
using System.Collections;

public class MiniGameBowlController : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 30f;

    private bool isActive = false;

    [SerializeField] private ParticleSystem pourEffect;
    [SerializeField] private Transform pourPoint; // where flour comes out
    [SerializeField] private LayerMask bowlMask;
    [SerializeField] private float pourRayDistance = 1.5f;

    public GameObject pourEffectThing;

    private float pourTimer = 0f;
    public float pourDuration = 4f;

    public GameObject flourBlockPrefab;
    public Transform bowlFillPoint; // Assign a transform inside the empty bowl where flour appears

    public ResourceTriggerZone gameManagerRef;

    private bool isPouring = false;

    public void Activate()
    {
        isActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Game is called from mini game manager");
    }

    public void Deactivate()
    {
        isActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isActive) return;

        // --- Bowl Movement ---
        float moveX = Input.GetAxis("Mouse X") * moveSpeed;
        float moveY = Input.GetAxis("Mouse Y") * moveSpeed;
        transform.position += new Vector3(moveX, moveY, 0f);

        // --- Bowl Rotation ---
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);

        // --- Pouring Check ---
        float currentZRotation = transform.localEulerAngles.z;
        if (currentZRotation > 180) currentZRotation -= 360f;

        bool isTilting = Mathf.Abs(currentZRotation) >= 55f;
        bool bowlUnder = IsTargetBowlUnderPourPoint();

        if (isTilting && bowlUnder)
        {
            TryStartPouring();
            pourTimer += Time.deltaTime;

            if (pourTimer >= pourDuration)
            {
                CompletePouring();
            }

            if (isPouring)
            {
                pourEffectThing.transform.position = pourPoint.transform.position;
            }
        }
        else
        {
            StopPouring();
            pourTimer = 0f;
        }
    }

    private void CompletePouring()
    {
        if (flourBlockPrefab != null && bowlFillPoint != null)
        {
            Instantiate(flourBlockPrefab, bowlFillPoint.position, bowlFillPoint.rotation);
        }

        // Optional: Prevent it from repeating
        isActive = false;

        // Lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Optionally wait a bit and then exit
        StartCoroutine(EndMiniGameAfterDelay());
    }

    private IEnumerator EndMiniGameAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        if (gameManagerRef != null)
        {
            pourEffect.Stop();
            transform.position = new Vector3(-5, -0, 1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gameManagerRef.ExitMiniGame(); // You create this method
        }
    }



    private bool IsTargetBowlUnderPourPoint()
    {
        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, pourRayDistance, bowlMask))
        {
            return hit.collider.CompareTag("Bowl");
        }
        return false;
    }

    private void TryStartPouring()
    {
        if (isPouring) return;

        // Raycast to check for second bowl
        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, 1.5f, bowlMask))
        {
            if (hit.collider.CompareTag("Bowl"))
            {
                pourEffect.Play();
                Debug.Log("PourEffectShouldBePlaying");
                isPouring = true;

                // You could also trigger filling here
                hit.collider.GetComponent<FlourReceiver>()?.StartFilling();
            }

        }
    }

    private void StopPouring()
    {
        if (!isPouring) return;

        pourEffect.Stop();
        isPouring = false;

        // Optional: tell receiver to stop
        Collider[] cols = Physics.OverlapSphere(pourPoint.position, 0.1f, bowlMask);
        foreach (var col in cols)
        {
            col.GetComponent<FlourReceiver>()?.StopFilling();
        }
    }
}
