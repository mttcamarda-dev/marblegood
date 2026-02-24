using UnityEngine;

/// <summary>
/// La camera segue entrambe le biglie, si centra tra loro
/// e applica zoom dinamico in base alla distanza.
/// Negli ultimi secondi prima della finish line attiva lo slow motion.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Tracking")]
    public float smoothSpeed     = 5f;
    public float minOrthoSize    = 3f;
    public float maxOrthoSize    = 7f;
    public float sizeMargin      = 1.5f;

    [Header("Slow Motion")]
    [Tooltip("Distanza dalla finish line per attivare lo slow motion")]
    public float slowMotionDist  = 3f;
    public float slowTimeScale   = 0.4f;
    public float finishLineY     = -8.5f;

    private Camera cam;
    private Transform marbleSI;
    private Transform marbleNO;
    private bool slowMoActive = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        // Verticale 9:16
        cam.orthographic = true;
        cam.orthographicSize = 5f;
    }

    void LateUpdate()
    {
        FindMarbles();
        if (marbleSI == null && marbleNO == null) return;

        Vector3 target = ComputeTarget();
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.x, target.y, transform.position.z),
            smoothSpeed * Time.unscaledDeltaTime);

        float targetSize = ComputeOrthoSize();
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize,
            smoothSpeed * Time.unscaledDeltaTime);

        HandleSlowMotion();
    }

    void FindMarbles()
    {
        if (marbleSI == null)
        {
            MarbleController[] all = FindObjectsOfType<MarbleController>();
            foreach (var m in all)
            {
                if (m.isSI)  marbleSI = m.transform;
                else          marbleNO = m.transform;
            }
        }
    }

    Vector3 ComputeTarget()
    {
        if (marbleSI == null) return marbleNO.position;
        if (marbleNO == null) return marbleSI.position;
        return (marbleSI.position + marbleNO.position) * 0.5f;
    }

    float ComputeOrthoSize()
    {
        if (marbleSI == null || marbleNO == null)
            return (minOrthoSize + maxOrthoSize) * 0.5f;

        float dist = Vector2.Distance(marbleSI.position, marbleNO.position);
        float size = dist * 0.5f + sizeMargin;
        return Mathf.Clamp(size, minOrthoSize, maxOrthoSize);
    }

    void HandleSlowMotion()
    {
        if (slowMoActive) return;
        if (GameManager.Instance != null && GameManager.Instance.IsFinished) return;

        float lowestY = float.MaxValue;
        if (marbleSI != null) lowestY = Mathf.Min(lowestY, marbleSI.position.y);
        if (marbleNO != null) lowestY = Mathf.Min(lowestY, marbleNO.position.y);

        if (lowestY <= finishLineY + slowMotionDist)
        {
            slowMoActive     = true;
            Time.timeScale   = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}
