using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeltaSky.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy AI Stats")]
        public float chaseRadius = 10f;
        private Transform target;
        [SerializeField] private float speed = 5f;
        
        // Start is called before the first frame update
        void Start()
        {
            target = Temp.temp.player.transform;
            transform.position = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            ChasePlayer();
        }

        public void ChasePlayer()
        {
            float distance = Vector3.Distance(transform.position, target.position);

            float moveSpeed = speed * Time.deltaTime;
            if (distance <= chaseRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
            }
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
