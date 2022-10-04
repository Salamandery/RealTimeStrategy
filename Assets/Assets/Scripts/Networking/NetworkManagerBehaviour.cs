using Mirror;
using UnityEngine;

public class NetworkManagerBehaviour : NetworkManager
{
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
    }
    #endregion
}
