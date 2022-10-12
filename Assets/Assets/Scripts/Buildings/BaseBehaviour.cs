using Mirror;
using System;
using UnityEngine;

public class BaseBehaviour : NetworkBehaviour
{
    [SerializeField]
    private Health health;

    public static event Action<BaseBehaviour> ServerOnBaseSpawned;
    public static event Action<BaseBehaviour> ServerOnBaseDespawned;

    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerOnDie;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerOnDie;

        ServerOnBaseDespawned?.Invoke(this);
    }

    [Server]
    private void ServerOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion
}
