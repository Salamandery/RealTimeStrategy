using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerBehaviour : NetworkBehaviour
{
    [SerializeField]
    private List<UnitBehaviour> units = new List<UnitBehaviour>();

    public List<UnitBehaviour> GetPlayerUnits()
    {
        return units;
    }

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
    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; }

        UnitBehaviour.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        UnitBehaviour.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }

        UnitBehaviour.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        UnitBehaviour.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    private void AuthorityHandleUnitSpawned(UnitBehaviour unit)
    {
        units.Add(unit);
    }
    private void AuthorityHandleUnitDespawned(UnitBehaviour unit)
    {
        units.Remove(unit);
    }
    #endregion
}
