using UnityEngine;

/// <summary>
/// Genera il percorso via script: rampa, curve S, ostacoli, stretto, piano, scivolo finale.
/// Tutti i muri sono EdgeCollider2D o BoxCollider2D dinamicamente piazzati.
/// </summary>
public class TrackGenerator : MonoBehaviour
{
    [Header("Wall Material")]
    public PhysicsMaterial2D wallMaterial;

    [Header("Obstacle Settings")]
    public float obstacleRotSpeed1 = 90f;
    public float obstacleRotSpeed2 = -75f;

    // Larghezza corridoio
    const float W = 1.4f;
    // Half-width
    const float HW = W * 0.5f;

    void Start()
    {
        BuildTrack();
    }

    void BuildTrack()
    {
        // ─── 1. RAMPA INIZIALE INCLINATA ─────────────────────────────────────
        // Muro sinistro che scende verso centro-sinistra
        CreateWallEdge("Ramp_Left",
            new Vector2(-HW - 0.5f, 9.5f),
            new Vector2(-HW,        6.5f));
        // Muro destro parallelo
        CreateWallEdge("Ramp_Right",
            new Vector2( HW + 0.5f, 9.5f),
            new Vector2( HW,        6.5f));

        // ─── 2. CURVE A S ────────────────────────────────────────────────────
        // Segmento S1: curva verso sinistra (y 6.5 → 4.5)
        CreateWallEdge("S1_Left_A",  new Vector2(-HW,        6.5f), new Vector2(-HW - 0.8f, 5.5f));
        CreateWallEdge("S1_Left_B",  new Vector2(-HW - 0.8f, 5.5f), new Vector2(-HW,        4.5f));
        CreateWallEdge("S1_Right_A", new Vector2( HW,        6.5f), new Vector2( HW - 0.8f, 5.5f));
        CreateWallEdge("S1_Right_B", new Vector2( HW - 0.8f, 5.5f), new Vector2( HW,        4.5f));

        // Segmento S2: curva verso destra (y 4.5 → 2.5)
        CreateWallEdge("S2_Left_A",  new Vector2(-HW,        4.5f), new Vector2(-HW + 0.8f, 3.5f));
        CreateWallEdge("S2_Left_B",  new Vector2(-HW + 0.8f, 3.5f), new Vector2(-HW,        2.5f));
        CreateWallEdge("S2_Right_A", new Vector2( HW,        4.5f), new Vector2( HW + 0.8f, 3.5f));
        CreateWallEdge("S2_Right_B", new Vector2( HW + 0.8f, 3.5f), new Vector2( HW,        2.5f));

        // ─── 3. OSTACOLI ROTANTI ─────────────────────────────────────────────
        float speed1 = obstacleRotSpeed1 * Random.Range(0.88f, 1.12f);
        float speed2 = obstacleRotSpeed2 * Random.Range(0.88f, 1.12f);

        CreateRotatingBar("Obstacle1", new Vector2(0f, 2.0f), new Vector2(W * 0.75f, 0.12f), speed1);
        CreateRotatingBar("Obstacle2", new Vector2(0f, 0.5f), new Vector2(W * 0.75f, 0.12f), speed2);

        // Muri laterali attorno agli ostacoli (y 2.5 → -0.5)
        CreateWallEdge("Obs_Left",  new Vector2(-HW, 2.5f), new Vector2(-HW, -0.5f));
        CreateWallEdge("Obs_Right", new Vector2( HW, 2.5f), new Vector2( HW, -0.5f));

        // ─── 4. PUNTO STRETTO (una sola biglia alla volta) ───────────────────
        float narrowHW = 0.22f; // poco più di un raggio (0.18f)
        CreateWallEdge("Narrow_Left_In",  new Vector2(-HW,      -0.5f), new Vector2(-narrowHW, -1.2f));
        CreateWallEdge("Narrow_Left_Out", new Vector2(-narrowHW,-1.2f), new Vector2(-HW,       -1.9f));
        CreateWallEdge("Narrow_Right_In", new Vector2( HW,      -0.5f), new Vector2( narrowHW, -1.2f));
        CreateWallEdge("Narrow_Right_Out",new Vector2( narrowHW,-1.2f), new Vector2( HW,       -1.9f));

        // ─── 5. TRATTO QUASI PIANO ───────────────────────────────────────────
        // Leggera pendenza per mantenere il moto
        CreateWallEdge("Flat_Left",  new Vector2(-HW, -1.9f), new Vector2(-HW, -3.5f));
        CreateWallEdge("Flat_Right", new Vector2( HW, -1.9f), new Vector2( HW, -3.5f));

        // Piattaforma "piano" (floor)
        CreatePlatform("FlatFloor", new Vector2(0f, -3.5f), new Vector2(W + 0.1f, 0.12f));

        // ─── 6. SCIVOLO FINALE CON DROP VERTICALE ────────────────────────────
        CreateWallEdge("Slide_Left",  new Vector2(-HW, -3.5f), new Vector2(-HW - 0.3f, -5.5f));
        CreateWallEdge("Slide_Right", new Vector2( HW, -3.5f), new Vector2( HW + 0.3f, -5.5f));

        // Drop: canale verticale stretto
        CreateWallEdge("Drop_Left",  new Vector2(-HW, -5.5f), new Vector2(-HW, -8.5f));
        CreateWallEdge("Drop_Right", new Vector2( HW, -5.5f), new Vector2( HW, -8.5f));

        // ─── 7. LINEA DI ARRIVO ──────────────────────────────────────────────
        CreateFinishLine(new Vector2(0f, -8.5f), new Vector2(W + 0.1f, 0.1f));
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  HELPERS
    // ─────────────────────────────────────────────────────────────────────────

    void CreateWallEdge(string name, Vector2 start, Vector2 end)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;
        go.layer = LayerMask.NameToLayer("Default");

        EdgeCollider2D ec = go.AddComponent<EdgeCollider2D>();
        ec.points = new Vector2[] { start, end };
        if (wallMaterial != null) ec.sharedMaterial = wallMaterial;

        // Renderer per visualizzare il muro
        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth  = 0.06f;
        lr.endWidth    = 0.06f;
        lr.material    = new Material(Shader.Find("Sprites/Default"));
        lr.startColor  = new Color(0.3f, 0.3f, 0.3f);
        lr.endColor    = new Color(0.3f, 0.3f, 0.3f);
        lr.sortingOrder = 1;
    }

    void CreatePlatform(string name, Vector2 center, Vector2 size)
    {
        GameObject go = new GameObject(name);
        go.transform.parent   = transform;
        go.transform.position = center;

        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        bc.size = size;
        if (wallMaterial != null) bc.sharedMaterial = wallMaterial;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite        = CreateRect(size);
        sr.color         = new Color(0.25f, 0.25f, 0.25f);
        sr.sortingOrder  = 1;
    }

    void CreateRotatingBar(string name, Vector2 center, Vector2 size, float rotSpeed)
    {
        GameObject go = new GameObject(name);
        go.transform.parent   = transform;
        go.transform.position = center;

        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        bc.size = size;
        if (wallMaterial != null) bc.sharedMaterial = wallMaterial;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite       = CreateRect(size);
        sr.color        = new Color(0.6f, 0.4f, 0.1f);
        sr.sortingOrder = 2;

        RotatingObstacle rot = go.AddComponent<RotatingObstacle>();
        rot.rotationSpeed = rotSpeed;
    }

    void CreateFinishLine(Vector2 center, Vector2 size)
    {
        GameObject go = new GameObject("FinishLine");
        go.transform.parent   = transform;
        go.transform.position = center;

        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        bc.size      = size;
        bc.isTrigger = true;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite       = CreateRect(size);
        sr.color        = new Color(1f, 0.85f, 0f);
        sr.sortingOrder = 2;

        go.AddComponent<FinishLine>();
    }

    // Crea uno sprite rettangolare procedurale
    Sprite CreateRect(Vector2 size)
    {
        int pw = Mathf.Max(1, Mathf.RoundToInt(size.x * 100));
        int ph = Mathf.Max(1, Mathf.RoundToInt(size.y * 100));
        Texture2D tex = new Texture2D(pw, ph);
        Color[] pixels = new Color[pw * ph];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, pw, ph), new Vector2(0.5f, 0.5f), 100f);
    }
}
