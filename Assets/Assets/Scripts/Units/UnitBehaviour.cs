using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitBehaviour : NetworkBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private UnitsMovements unitsMovements;
    [SerializeField]
    private Targeter targeter;
    [SerializeField]
    private UnityEvent onSelected;
    [SerializeField]
    private UnityEvent onDeselected;

    public static event Action<UnitBehaviour> ServerOnUnitSpawned;
    public static event Action<UnitBehaviour> ServerOnUnitDespawned;

    public static event Action<UnitBehaviour> AuthorityOnUnitSpawned;
    public static event Action<UnitBehaviour> AuthorityOnUnitDespawned;

    public Targeter GetTargeter()
    {
        return targeter;
    }
    public UnitsMovements GetUnitsMovements()
    {
        return unitsMovements;
    }
    #region Server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);

        health.ServerOnDie += ServerHadleDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);

        health.ServerOnDie -= ServerHadleDie;
    }

    [Server]
    private void ServerHadleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client
    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) { return; }
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
