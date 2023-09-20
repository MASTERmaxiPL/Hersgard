using UnityEngine;

public class CameraMapManager : MonoBehaviour
{
    private Transform mapTarget;
    [SerializeField] private Camera cam;
    [SerializeField] private float zoom, maxZoom, minZoom;
    [SerializeField] private SpriteRenderer mapRenderer;
    [SerializeField] private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D coll;

    private Vector3 dragOrigin;
    private Vector3 mousePos;

    private static CameraMapManager instance;
    public static CameraMapManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Something is wrong with map camera");
        }
        instance = this;
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        mapTarget = GameObject.Find("map").transform;
        TurnOffMap();
    }

    private void Update()
    {
        MoveCam();
    }
    private void MoveCam()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            print("origin " + dragOrigin + ", new position" + cam.ScreenToWorldPoint(Input.mousePosition) + ", diff= " + diff);
            cam.transform.position = ClampCamera(cam.transform.position + diff);
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            print("Create point at:");
            mousePos = Input.mousePosition;
            mousePos.x = mousePos.x + 500;
            mousePos.y = mousePos.y - 300;
        }

        float x, y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x * 150, y * 150);
    }
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

    public void ScrollUp()
    {
        float newZoom = cam.orthographicSize + zoom;
        if(cam.orthographicSize != maxZoom)
        {
            coll.size = new Vector2(coll.size.x + 54, coll.size.y + 54);
        }
        cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        cam.transform.position = ClampCamera(cam.transform.position);
    }
    public void ScrollDown()
    {
        if (cam.orthographicSize != minZoom)
        {
            coll.size = new Vector2(coll.size.x - 54, coll.size.y - 54);
        }
        float newZoom = cam.orthographicSize - zoom;
        cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void TurnOnMap()
    {
        cam.gameObject.SetActive(true);
    }
    public void TurnOffMap() 
    {
        cam.gameObject.SetActive(false);
    }
}
