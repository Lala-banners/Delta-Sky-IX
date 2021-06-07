using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour
{
    [SerializeField, Tooltip("X Position Sensitivity")]
    private float horizontalSpeed;

    [SerializeField, Tooltip("Y Position Sensitivity")]
    private float verticalSpeed;

    [Tooltip("Rotation on Y axis")] private float yaw;
    [Tooltip("Rotation on X axis")] private float pitch;

    private Image crosshairs;
    
    private void Start() {
        // crosshairs = FindObjectOfType<Image>();
        horizontalSpeed = 2f;
        verticalSpeed = 2f;
    }

    // Update is called once per frame
    void Update() {
        // crosshairs.transform.position = Input.mousePosition;
        FollowMouseInput();
    }
    
    /// <summary>
    /// Makes the camera follow the mouse position.
    /// </summary>
    public void FollowMouseInput() {
        yaw += horizontalSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}