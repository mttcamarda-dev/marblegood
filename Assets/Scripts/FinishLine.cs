using UnityEngine;

/// <summary>
/// Trigger sulla linea di arrivo.
/// La prima biglia che la tocca vince.
/// Il vincitore emerge solo dalla fisica — questo script si limita a rilevarlo.
/// </summary>
public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance == null || GameManager.Instance.IsFinished) return;

        MarbleController marble = other.GetComponent<MarbleController>();
        if (marble == null) return;

        GameManager.Instance.OnMarbleFinished(marble.isSI);
    }
}
