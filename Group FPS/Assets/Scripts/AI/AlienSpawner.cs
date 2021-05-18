using Mirror;
using UnityEngine;

namespace Space.AI
{
    public class AlienSpawner : NetworkBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                CmdSpawnObj();
            }
        }

        [Command]
        public void CmdSpawnObj()
        {
            //This object now only lives on server
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            //Put cube as 10x10x10
            cube.transform.position = Vector3.zero * 10;
         
            //Spawn object into all clients from server
            NetworkServer.Spawn(cube);
        }
    }
}
