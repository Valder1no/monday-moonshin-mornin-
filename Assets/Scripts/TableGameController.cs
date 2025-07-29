using UnityEngine;

public class MiniGameBowlController : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 30f;

    private bool isActive = false;

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

        // Mouse movement
        float moveX = Input.GetAxis("Mouse X") * moveSpeed;
        float moveY = Input.GetAxis("Mouse Y") * moveSpeed;

        transform.position += new Vector3(moveX, moveY, 0f);

        // Tilt rotation for pouring
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
        }
    }
}
