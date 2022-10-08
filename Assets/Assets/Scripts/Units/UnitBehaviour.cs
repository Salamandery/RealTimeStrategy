using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitBehaviour : NetworkBehaviour
{
    [SerializeField]
    private UnitsMovements unitsMovements;
    [SerializeField]
    private UnityEvent onSelected;
    [SerializeField]
    private UnityEvent onDeselected;

    public static event Action<UnitBehaviour> ServerOnUnitSpawned;
    public static event Action<UnitBehaviour> ServerOnUnitDespawned;

    public static event Action<UnitBehaviour> AuthorityOnUnitSpawned;
    public static event Action<UnitBehaviour> AuthorityOnUnitDespawned;

    public UnitsMovements GetUnitsMovements()
    {
        return unitsMovements;
    }
    #region Server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }
    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }
    #endregion

    #region Client
    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnUnitDespawned?.Invoke(this);
    }
    [Client]
    public void Select()
    {
        if (!hasAuthority) { return; }
        onSelected.Invoke();
    }
    [Client]
    public void Deselect()
    {
        onDeselected.Invoke();
    }
    #endregion
}
