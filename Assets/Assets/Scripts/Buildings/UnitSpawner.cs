using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private Transform unitSpawnPoint;

    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleOnDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleOnDie;
    }

    [Server]
    private void ServerHandleOnDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(
            unitPrefab,
            unitSpawnPoint.position,
            unitSpawnPoint.rotation
        );
        NetworkServer.Spawn(
            unitInstance,
            connectionToClient
        );
    }

    #endregion

    #region Client
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        if (!hasAuthority) { return; }

        CmdSpawnUnit();
    }
    #endregion
}
