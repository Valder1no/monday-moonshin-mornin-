using UnityEngine;
using System.Collections;

public class MiniGameBowlController2 : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 30f;

    private bool isActive = false;

    [SerializeField] private ParticleSystem pourEffect;
    [SerializeField] private Transform pourPoint;
    [SerializeField] private LayerMask bowlMask;
    [SerializeField] private float pourRayDistance = 1.5f;

    public GameObject pourEffectThing;

    private float pourTimer = 0f;
    public float pourDuration = 4f;

    public GameObject flourBlockPrefab;
    public Transform bowlFillPoint;

    public ResourceTriggerZone2 gameManagerRef;

    private bool isPouring = false;
    private bool hasPoured = false;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

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
        if (!isActive || hasPoured) return;

        float moveX = Input.GetAxis("Mouse X") * moveSpeed;
        float moveY = Input.GetAxis("Mouse Y") * moveSpeed;
        transform.position += new Vector3(-moveX, moveY, 0f);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);

        float currentZRotation = transform.localEulerAngles.z;
        if (currentZRotation > 180f) currentZRotation -= 360f;

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
                pourEffectThing.transform.position = pourPoint.position;
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
        if (hasPoured) return;
        hasPoured = true;

        if (flourBlockPrefab != null && bowlFillPoint != null)
        {
            Instantiate(flourBlockPrefab, bowlFillPoint.position, bowlFillPoint.rotation);
        }

        StartCoroutine(EndMiniGameAfterDelay());
    }

    private IEnumerator EndMiniGameAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        pourEffect.Stop();
        transform.position = initialPos;
        transform.rotation = initialRot;

        gameManagerRef?.ExitMiniGame();
    }

    private bool IsTargetBowlUnderPourPoint()
    {
        return Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, pourRayDistance, bowlMask)
               && hit.collider.CompareTag("Bowl");
    }

    private void TryStartPouring()
    {
        if (isPouring) return;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out RaycastHit hit, pourRayDistance, bowlMask))
        {
            if (hit.collider.CompareTag("Bowl"))
            {
                pourEffect.Play();
                Debug.Log("PourEffectShouldBePlaying");
                isPouring = true;
                hit.collider.GetComponent<FlourReceiver>()?.StartFilling();
            }
        }
    }

    private void StopPouring()
    {
        if (!isPouring) return;

        pourEffect.Stop();
        isPouring = false;

        Collider[] cols = Physics.OverlapSphere(pourPoint.position, 0.1f, bowlMask);
        foreach (var col in cols)
        {
            col.GetComponent<FlourReceiver>()?.StopFilling();
        }
    }
}
