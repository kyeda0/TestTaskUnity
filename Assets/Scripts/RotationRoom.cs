using UnityEngine;

public class RotationRoom : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("Zoom")]
    [SerializeField] private float distance = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 20f;

    private float horizontalAngle;
    private float verticalAngle;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        horizontalAngle = angles.y;
        verticalAngle = angles.x;
    }

    private void Update()
    {
        Rotate();
        Zoom();
        UpdatePosition();
    }


    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            horizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed * 100f * Time.deltaTime;
            verticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed * 100f * Time.deltaTime;

            verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
        }
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed * 100f * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 direction = rotation * Vector3.back * distance;

        transform.position = target.position + direction;
        transform.rotation = rotation;
    }
}
