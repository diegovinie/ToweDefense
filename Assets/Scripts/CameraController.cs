using UnityEngine;

public struct RotCorrector
{
    public Vector3 forward {
        get{ return Vector3.right; }
    }
    public Vector3 back {
        get{ return Vector3.left; }
    }
    public Vector3 right {
        get{ return Vector3.back; }
    }
    public Vector3 left {
        get{ return Vector3.forward; }
    }
}

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float scrollSpeed = 5f;
    public float panBorderThickness = 10f;
    public float minY = 10f;
    public float maxY = 80f;
    private bool doMovement = true;
    private Vector3 startPos;
    private Quaternion startRot;
    private RotCorrector corrector;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
        {
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) doMovement = !doMovement;

        if (!doMovement) return;

        if (Input.GetKey(KeyCode.C)) {
            transform.position = startPos;
            transform.rotation = startRot;
        }

        if (Input.GetKey("w") || Input.mousePosition.y >  Screen.height - panBorderThickness)
        {
            transform.Translate(corrector.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y < panBorderThickness)
        {
            transform.Translate(corrector.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x > Screen.width - panBorderThickness)
        {
            transform.Translate(corrector.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x < panBorderThickness)
        {
            transform.Translate(corrector.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.Q) && transform.position.y > minY)
        {
            transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E) && transform.position.y < maxY)
        {
            transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
