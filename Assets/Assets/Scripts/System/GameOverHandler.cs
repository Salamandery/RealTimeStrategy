using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{
    private List<BaseBehaviour> bases;
    #region Server
    public override void OnStartServer()
    {
        BaseBehaviour.ServerOnBaseSpawned += ServerHandleBaseSpawned;
        BaseBehaviour.ServerOnBaseDespawned += ServerHandleBaseDespawned;
    }

    public override void OnStopServer()
    {
        BaseBehaviour.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
        BaseBehaviour.ServerOnBaseDespawned -= ServerHandleBaseDespawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(BaseBehaviour unitBase)
    {
        bases.Add(unitBase);
    }

    [Server]
    private void ServerHandleBaseDespawned(BaseBehaviour unitBase)
    {
        bases.Remove(unitBase);

        if (bases.Count != 1) { return; }

        Debug.Log($"Game Over");
    }
    #endregion

    #region Client

    #endregion
}
