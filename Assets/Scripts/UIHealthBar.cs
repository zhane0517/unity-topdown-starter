using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minimal UI updater for a Health component. Expects an Image fill (or Slider) to display health.
/// </summary>
public class UIHealthBar : MonoBehaviour
{
    public Health target;
    public Image fillImage;

    void Start()
    {
        if (target != null)
        {
            target.OnDamaged.AddListener(_ => Refresh());
            target.OnHealed.AddListener(_ => Refresh());
            target.OnDead.AddListener(() => Refresh());
            Refresh();
        }
    }

    public void Refresh()
    {
        if (target == null || fillImage == null) return;
        fillImage.fillAmount = (float)target.currentHealth / target.maxHealth;
    }
}
