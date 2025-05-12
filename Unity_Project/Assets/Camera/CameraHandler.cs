using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    public Camera cam;

    [SerializeField]
    Canvas health;

    private float zoomValue;

    public float minZoomValue;
    public float maxZoomValue;

    private float zoom;

    public float zoomMultiplier;

    private float velocityZoom = 0f;
    private float smoothTime = 0.25f;

    public Transform player;

    private Vector3 velocityFollow = Vector3.zero;

    public void Start()
    {
        zoom = cam.orthographicSize;
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        moveCamera();
        cameraZoom();
    
    }

    private void moveCamera()
    {
        Vector3 target = new Vector3(player.position.x, player.position.y, cam.transform.position.z);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, target, ref velocityFollow, smoothTime);
    }

    private void cameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoomValue, maxZoomValue);

      

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocityZoom, smoothTime);
    }
}
