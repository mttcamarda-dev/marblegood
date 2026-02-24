using UnityEngine;

/// <summary>
/// Aggiunto sia alla biglia SI (verde) che alla biglia NO (rossa).
/// Applica variazioni fisiche casuali all'avvio e una piccola forza laterale iniziale.
/// Non decide mai il vincitore: è la fisica a farlo.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class MarbleController : MonoBehaviour
{
    [Header("Identity")]
    public bool isSI = true;          // true = SI (verde), false = NO (rosso)
    public string label = "SI";

    [Header("Physics Variation")]
    [Tooltip("Variazione massa ±%")]
    public float massVariationPct   = 3f;
    [Tooltip("Variazione drag ±%")]
    public float dragVariationPct   = 2f;
    [Tooltip("Forza laterale iniziale massima")]
    public float maxLateralForce    = 0.8f;

    [Header("Visual")]
    public float marbleRadius = 0.18f;

    private Rigidbody2D rb;
    private CircleCollider2D cc;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();

        SetupPhysics();
        SetupVisual();
    }

    void Start()
    {
        ApplyInitialForce();
    }

    void SetupPhysics()
    {
        // Massa base con variazione casuale ±massVariationPct%
        float massBase = 1f;
        float massMult = 1f + Random.Range(-massVariationPct, massVariationPct) / 100f;
        rb.mass = massBase * massMult;

        // Drag base con variazione casuale ±dragVariationPct%
        float dragBase = 0.05f;
        float dragMult = 1f + Random.Range(-dragVariationPct, dragVariationPct) / 100f;
        rb.drag = dragBase * dragMult;

        rb.angularDrag    = 0.02f;
        rb.gravityScale   = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation  = RigidbodyInterpolation2D.Interpolate;
        rb.constraints    = RigidbodyConstraints2D.None;

        // Collider
        cc.radius = marbleRadius;

        // PhysicsMaterial2D: leggero rimbalzo
        PhysicsMaterial2D mat = new PhysicsMaterial2D("MarbleMat");
        mat.bounciness = 0.3f;
        mat.friction   = 0.4f;
        cc.sharedMaterial = mat;
    }

    void SetupVisual()
    {
        // Crea sprite circolare procedurale
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        sr.sprite       = CreateCircleSprite(64);
        sr.color        = isSI ? new Color(0.1f, 0.8f, 0.2f) : new Color(0.9f, 0.15f, 0.1f);
        sr.sortingOrder = 5;

        // Etichetta testo sotto la biglia
        GameObject labelGo = new GameObject("Label_" + label);
        labelGo.transform.parent        = transform;
        labelGo.transform.localPosition = new Vector3(0f, -0.32f, 0f);
        labelGo.transform.localScale    = Vector3.one * 0.018f;

        TextMesh tm       = labelGo.AddComponent<TextMesh>();
        tm.text           = label;
        tm.fontSize       = 48;
        tm.alignment      = TextAlignment.Center;
        tm.anchor         = TextAnchor.MiddleCenter;
        tm.color          = Color.white;
        tm.fontStyle      = FontStyle.Bold;

        MeshRenderer mr   = labelGo.GetComponent<MeshRenderer>();
        mr.sortingOrder   = 6;
    }

    void ApplyInitialForce()
    {
        // Piccola forza laterale casuale per evitare percorsi identici
        float lateralX = Random.Range(-maxLateralForce, maxLateralForce);
        rb.AddForce(new Vector2(lateralX, 0f), ForceMode2D.Impulse);
    }

    // Genera uno sprite circolare bianco 64×64
    Sprite CreateCircleSprite(int resolution)
    {
        Texture2D tex    = new Texture2D(resolution, resolution);
        Color[]   pixels = new Color[resolution * resolution];
        float     center = resolution * 0.5f;
        float     radius = center - 1f;

        for (int y = 0; y < resolution; y++)
        for (int x = 0; x < resolution; x++)
        {
            float dx   = x - center;
            float dy   = y - center;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);
            pixels[y * resolution + x] = dist <= radius ? Color.white : Color.clear;
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, resolution, resolution),
                             new Vector2(0.5f, 0.5f), resolution / (marbleRadius * 2f));
    }
}
