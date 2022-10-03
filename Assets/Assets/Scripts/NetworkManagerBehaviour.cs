using Mirror;
using UnityEngine;

public class NetworkManagerBehaviour : NetworkManager
{
    #region Server
    [Server]
    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log($"A Player was connected");
    }

    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log($"{numPlayers} Players in the server!");
    }
    #endregion
}
