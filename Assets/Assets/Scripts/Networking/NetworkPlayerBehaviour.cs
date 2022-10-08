using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerBehaviour : NetworkBehaviour
{
    [SerializeField]
    private List<UnitBehaviour> units = new List<UnitBehaviour>();

    #region Server
    public override void OnStartServer()
    {
        UnitBehaviour.ServerOnUnitSpawned += ServerHandlerUnitSpawned;
        UnitBehaviour.ServerOnUnitDespawned += ServerHandlerUnitDespawned;
    }

    public override void OnStopServer()
    {
        UnitBehaviour.ServerOnUnitSpawned -= ServerHandlerUnitSpawned;
        UnitBehaviour.ServerOnUnitDespawned -= ServerHandlerUnitDespawned;
    }

    private void ServerHandlerUnitSpawned(UnitBehaviour unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        units.Add(unit);
    }
    private void ServerHandlerUnitDespawned(UnitBehaviour unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        units.Remove(unit);
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        if (!isClientOnly) { return; }

        UnitBehaviour.AuthorityOnUnitSpawned += AuthorityHandlerUnitSpawned;
        UnitBehaviour.AuthorityOnUnitDespawned += AuthorityHandlerUnitDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly) { return; }

        UnitBehaviour.AuthorityOnUnitSpawned -= AuthorityHandlerUnitSpawned;
        UnitBehaviour.AuthorityOnUnitDespawned -= AuthorityHandlerUnitDespawned;
    }

    private void AuthorityHandlerUnitSpawned(UnitBehaviour unit)
    {
        if (!hasAuthority) { return; }
        units.Add(unit);
    }
    private void AuthorityHandlerUnitDespawned(UnitBehaviour unit)
    {
        if (!hasAuthority) { return; }
        units.Remove(unit);
    }
    #endregion
}
