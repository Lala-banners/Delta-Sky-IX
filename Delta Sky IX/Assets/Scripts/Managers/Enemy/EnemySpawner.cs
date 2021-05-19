using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaSky.Controllers;

namespace DeltaSky.Controllers.Spawn
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Alien Spawning")] 
        [SerializeField] private GameObject alienPrefab;
        [SerializeField] private float xPos;
        [SerializeField] private float zPos;
        public int enemyCount;
        private int maxEnemyCount;
        
        void Start()
        {
            maxEnemyCount = 10;
            StartCoroutine(GenerateEnemies());
        }

        /// <summary>
        /// Will run repeatedly until maximum count is reached. 
        /// </summary>
        IEnumerator GenerateEnemies()
        {
            while (enemyCount < maxEnemyCount)
            {
                xPos = Random.Range(1, 50);
                zPos = Random.Range(1, 30);
                //Place enemy at generated coords
                Instantiate(alienPrefab, new Vector3(xPos, 2, zPos), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                enemyCount += 1;
            }
        }
    }
}
