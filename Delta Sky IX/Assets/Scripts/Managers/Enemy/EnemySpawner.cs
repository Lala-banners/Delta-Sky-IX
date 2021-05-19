using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeltaSky.Controllers.Spawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Alien Spawning")] 
        [SerializeField] private GameObject alienPrefab;
        [SerializeField] private float xPos;
        [SerializeField] private float zPos;
        private float xSpacing = 0.1f;
        private float zSpacing = 0.1f;
        public int enemyCount;
        private int _maxEnemyCount = 5;

        private void Awake()
        {
            StartCoroutine(GenerateEnemies());
        }

        /// <summary>
        /// Will run repeatedly until maximum count is reached. 
        /// </summary>
        IEnumerator GenerateEnemies()
        {
            while (enemyCount < _maxEnemyCount)
            {
                xPos = Random.Range(1, 10);
                zPos = Random.Range(1, 20);
                //Debug.Log(xPos + ", " + zPos); SPAWNING WORKS!

                //Place enemy at generated coords
                Instantiate(alienPrefab, new Vector3(xPos + xSpacing, 0, zPos + zSpacing), Quaternion.identity);
                yield return new WaitForSeconds(0.2f); //Ever .2 seconds an enemy will spawn
                enemyCount += 1;
            }
        }
    }
}
