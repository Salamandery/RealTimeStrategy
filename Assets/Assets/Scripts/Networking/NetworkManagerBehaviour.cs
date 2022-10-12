using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerBehaviour : NetworkManager
{
    [SerializeField]
    private GameObject unitSpawnPrefab;
    [SerializeField]
    private GameOverHandler gameOverHandler;
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

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("ScMap"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandler);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
        }
    }
    #endregion
}
