using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity = 100f;
    [SerializeField] private float walk = 5f;
    [SerializeField] private float run = 10f;
    [SerializeField] private float crouch = 2.5f;

    private Vector3 crouchScale, normalScale;
    private bool isMoving, isCrouching, isRunning;

    private float X, Y;
    private Rigidbody rb;

    private void Start()
    {
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        #region Camera Control
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        Y -= Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        Y = Mathf.Clamp(Y, MIN_Y, MAX_Y);

        cam.transform.localRotation = Quaternion.Euler(Y, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, X, 0f);
        #endregion

        HandleMovement();
    }

    private void HandleMovement()
    {
        float speed = walk;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouch;
            isCrouching = true;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isCrouching = false;
            player.transform.localScale = normalScale;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = (transform.forward * vertical + transform.right * horizontal).normalized;

        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

        isMoving = direction.sqrMagnitude > 0f;
    }
}
