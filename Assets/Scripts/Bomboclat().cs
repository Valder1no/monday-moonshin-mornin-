using UnityEngine;

public class ItemInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float pickupRange = 3f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float throwForce = 500f;

    private Rigidbody heldItemRb;
    private PickupableItem heldItem;

    public GameObject raycastStartPoint;

    void Update()
    {
        if (heldItem == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickup();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) // Left click to throw
            {
                Throw();
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Q to drop
            {
                Drop();
            }
        }
    }

    void FixedUpdate()
    {
        if (heldItemRb != null)
        {
            float maxHoldDistance = 4f;
            float bufferDistance = 0.1f;
            Vector3 origin = cam.transform.position;
            Vector3 direction = cam.transform.forward;

            Ray ray = new Ray(origin, direction);
            Vector3 targetPos = origin + direction * maxHoldDistance;

            RaycastHit[] hits = Physics.RaycastAll(ray, maxHoldDistance);

            foreach (RaycastHit hit in hits)
            {
                // Ignore the held object and trigger colliders
                if (hit.collider.attachedRigidbody == heldItemRb || hit.collider.isTrigger)
                    continue;

                // Found something else � adjust the target position
                targetPos = hit.point - direction * bufferDistance;
                break;
            }

            // Smooth movement
            heldItemRb.MovePosition(Vector3.Lerp(heldItemRb.position, targetPos, 0.2f));
            heldItemRb.MoveRotation(Quaternion.Slerp(heldItemRb.rotation, holdPoint.rotation, 0.2f));
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // center of screen
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            PickupableItem item = hit.transform.GetComponent<PickupableItem>();
            if (item != null)
            {
                heldItem = item;
                heldItemRb = item.GetComponent<Rigidbody>();

                heldItemRb.useGravity = false;
                heldItemRb.linearVelocity = Vector3.zero;
                heldItemRb.angularVelocity = Vector3.zero;
                heldItemRb.transform.SetParent(null);
            }
        }
    }

    void Drop()
    {
        if (heldItemRb != null)
        {
            heldItemRb.useGravity = true;
            heldItemRb.transform.SetParent(null);
        }
        heldItem = null;
        heldItemRb = null;
    }

    void Throw()
    {
        if (heldItemRb != null)
        {
            heldItemRb.useGravity = true;
            heldItemRb.transform.SetParent(null);
            heldItemRb.AddForce(cam.transform.forward * throwForce);
        }
        heldItem = null;
        heldItemRb = null;
    }
}
