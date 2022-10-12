using Mirror;
using System;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField, SyncVar]
    private float currentHealth;

    public event Action ServerOnDie;

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

    #endregion
}
