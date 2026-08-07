using UnityEngine;

public class Background : MonoBehaviour
{
    //TODO: CREATE PARALAX FOR THE BACKGROUND, IT SHOULD CATCHUP TO THE MAIN CAMERA
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float parallaxMultiplier = .1f;

    private Vector2 mainCameraPreviousPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCameraPreviousPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // LateUpdate runs once per frame, always after every Update() has finished (including Cinemachine's camera update).
    // Use it here so we read the camera's final position for this frame, not a stale one from before it moved.
    void LateUpdate()
    {
        Vector2 mainCameraCurrentPosition = mainCamera.transform.position;
        Vector2 positionDelta = mainCameraCurrentPosition - mainCameraPreviousPosition;

        transform.position += (Vector3)positionDelta * parallaxMultiplier;
        mainCameraPreviousPosition = mainCameraCurrentPosition;
    }
}
