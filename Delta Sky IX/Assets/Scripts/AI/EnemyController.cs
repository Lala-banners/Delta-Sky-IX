using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeltaSky.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy AI Stats")]
        public float chaseRadius = 10f;
        private NavMeshAgent agent;
        private Transform target;
        
        // Start is called before the first frame update
        void Start()
        {
            target = Temp.temp.player.transform;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {

            
        }

        /// <summary>
        /// Visible chase radius for testing
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
