using Mirror;
using System;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField, SyncVar(hook = nameof(HandleHealthUpdated))]
    private float currentHealth;

    public event Action ServerOnDie;
    public event Action<float, float> ClientOnHealthUpdated;

    #region Server
    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(float damageAmount)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();

        Debug.Log("Dead");
    }
    #endregion

    #region Client
    private void HandleHealthUpdated(float oldHealth, float newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }
    #endregion
}
