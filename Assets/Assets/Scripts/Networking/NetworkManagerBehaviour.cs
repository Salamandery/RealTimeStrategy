using Mirror;
using UnityEngine;

public class NetworkManagerBehaviour : NetworkManager
{
    [SerializeField]
    private GameObject unitSpawnPrefab;
    #region Server
    public override void OnClientConnect()
    {
        Debug.Log($"A Player was connected");
        base.OnClientConnect();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log($"{numPlayers} Players in the server!");
        base.OnServerAddPlayer(conn);
        GameObject unitInstance = Instantiate(
            unitSpawnPrefab,
            conn.identity.transform.position,
            conn.identity.transform.rotation
        );

        NetworkServer.Spawn(unitInstance, conn);
    }
    #endregion
}
