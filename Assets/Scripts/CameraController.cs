using UnityEngine;
using UnityEngine.UI;

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
    public Button button;
    public float panSpeed = 30f;
    public float rotSpeed = 10f;
    public float zoomSpeed = 20f;
    public float scrollSpeed = 5f;
    public float panBorderThickness = 10f;
    public float minY = 10f;
    public float maxY = 80f;
    private bool doMovement = false;
    private Vector3 startPos;
    private Quaternion startRot;
    public Transform target;
    public bool tracking;
    public Vector3 targetOffset = new Vector3(0, 3, -5);

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        SetButtonColor(doMovement);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver)
        {
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.L)) ToggleDoMovement();

        if (!doMovement) return;

        if (Input.GetKey(KeyCode.C))
        {
            transform.position = startPos;
            transform.rotation = startRot;
            DisengageTarget();
        }

        if (tracking)
        {
            UpdateFocusedCameraControl();

        } else
        {
            UpdateFreeCameraControls();
        }
    }

    public void SetDoMovement(bool status)
    {
        doMovement = status;

        SetButtonColor(status);
    }

    public void ToggleDoMovement()
    {
        SetDoMovement(!doMovement);
    }

    void SetButtonColor(bool status)
    {
        if (button)
        {
            ColorBlock cb = button.colors;
            cb.colorMultiplier = status ? 2 : 1;
            button.colors = cb;
        }
    }

    public void FocusOnTarget(Transform _target)
    {
        if (!doMovement) return;

        target = _target;
        tracking = true;
    }

    public void DisengageTarget()
    {
        target = null;
        tracking = false;
    }

    void UpdateFreeCameraControls()
    {
        if (Input.GetKey("w") || Input.mousePosition.y > Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y < panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x > Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x < panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
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

    void UpdateFocusedCameraControl()
    {
        float angle;
        int dirY = 1;
        int dir = 1;
        Vector3 axis = Vector3.up;

        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool closer = Input.GetKey(KeyCode.W);
        bool farther = Input.GetKey(KeyCode.S);

        if (left || right)
        {
            dirY = right ? 1 : -1;
            axis = Vector3.up;
            angle = Time.deltaTime * dirY * rotSpeed;

            CameraController.RotateAround(transform, target.position, axis, angle);
        } else if (closer || farther)
        {
            dir = closer ? 1 : -1;
            Vector3 nCam = Vector3.Normalize(target.position - transform.position);

            transform.position += dir * nCam * Time.deltaTime * zoomSpeed;
        } else
        {
            UpdateTrackTarget();
            return;
        }

        targetOffset = transform.position - target.position;
    }

    void UpdateTrackTarget()
    {
        transform.position = target.position + targetOffset;
        transform.LookAt(target);
    }

    // from https://answers.unity.com/questions/1751620/rotating-around-a-pivot-point-using-a-quaternion.html
    static void RotateAround(Transform transform, Vector3 pivotPoint, Vector3 axis, float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;
    }
}
