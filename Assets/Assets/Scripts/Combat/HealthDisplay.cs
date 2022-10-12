using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject healthBarParent;
    [SerializeField]
    private Image healthBar;

    void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
