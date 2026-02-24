using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Question")]
    public string questionText = "Domani vai a scuola?";

    [Header("UI References")]
    public Text questionLabel;
    public Text resultLabel;
    public GameObject resultPanel;

    [Header("Marble Prefabs")]
    public GameObject marbleSIPrefab;
    public GameObject marbleNOPrefab;

    [Header("Spawn")]
    public Transform spawnSI;
    public Transform spawnNO;

    private bool gameFinished = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        resultPanel.SetActive(false);

        if (questionLabel != null)
            questionLabel.text = questionText;

        StartCoroutine(SpawnMarbles());
    }

    IEnumerator SpawnMarbles()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 siPos  = spawnSI  != null ? spawnSI.position  : new Vector3(-0.3f, 8f, 0f);
        Vector3 noPos  = spawnNO  != null ? spawnNO.position  : new Vector3( 0.3f, 8f, 0f);

        Instantiate(marbleSIPrefab, siPos, Quaternion.identity);
        Instantiate(marbleNOPrefab, noPos, Quaternion.identity);
    }

    public void OnMarbleFinished(bool isSI)
    {
        if (gameFinished) return;
        gameFinished = true;

        Time.timeScale = 1f;

        string msg = isSI ? "SI ✅" : "NO ❌";
        ShowResult(msg);
    }

    void ShowResult(string msg)
    {
        resultPanel.SetActive(true);
        if (resultLabel != null)
            resultLabel.text = msg;

        StartCoroutine(BounceAnimation(resultPanel.transform));
    }

    IEnumerator BounceAnimation(Transform t)
    {
        t.localScale = Vector3.zero;
        float duration = 0.5f;
        float elapsed  = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float p = elapsed / duration;
            float scale = Mathf.Sin(p * Mathf.PI) * 1.2f + (p >= 0.5f ? 1f : 0f);
            scale = Mathf.Lerp(0f, 1f, p < 0.5f ? 2f * p * p : -1f + (4f - 2f * p) * p);
            t.localScale = Vector3.one * scale;
            yield return null;
        }

        t.localScale = Vector3.one;
    }

    public bool IsFinished => gameFinished;
}
