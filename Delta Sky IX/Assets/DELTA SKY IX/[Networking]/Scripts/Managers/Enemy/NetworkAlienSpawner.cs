using Mirror;
using UnityEngine;

namespace DeltaSky.Networking.Enemy
{
    public class NetworkAlienSpawner : NetworkBehaviour
    {
        public GameObject enemyPrefab;
        public Transform[] spawnPos;

        public override void OnStartServer() {
            base.OnStartServer();
            CmdSpawnObj();
        }
        
        /// <summary>
        /// This is used for Networking the enemy Spawner (later)
        /// </summary>
        [Command]
        public void CmdSpawnObj() {
            //This object now only lives on server
            GameObject enemy = Instantiate(enemyPrefab, spawnPos[Random.Range(0, spawnPos.Length)].position, Quaternion.identity);

            //Spawn object into all clients from server
            NetworkServer.Spawn(enemy);
        }
    }
}
