using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float zoomSpeed = 10f;

    private const float MinFov = 3f;
    private const float MaxFov = 22f;

	private const float MaxUp = 60f;
	private const float MaxDown = 9.3f;
	private const float MaxLeft = 24.7f;
	private const float MaxRight = 45.5F;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        MovementUpdate();
        CameraZoomUpdate();
    }

    private void MovementUpdate()
    {
        Vector3 direction = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"), 
            0
        ).normalized;

        Vector3 newPosition = _camera.transform.position + direction * movementSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, MaxLeft, MaxRight);
        newPosition.y = Mathf.Clamp(newPosition.y, MaxDown, MaxUp);

        _camera.transform.position = newPosition;
    }

    private void CameraZoomUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        _camera.orthographicSize = Mathf.Clamp(
            _camera.orthographicSize - scroll * zoomSpeed, 
            MinFov, 
            MaxFov
        );
    }
}
