using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaSky.Controllers.UI;

namespace DeltaSky.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private Transform target;
        
        [Header("Enemy AI Stats")]
        [SerializeField] private float chaseRadius = 5f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float distance;
        [SerializeField] private float moveSpeed;

        [Header("Enemy Damage Stats")] 
        public float damage;
        [SerializeField] private float attackRadius = 2f;
        
        // Start is called before the first frame update
        void Start()
        {
            target = Temp.temp.player.transform;
        }

        // Update is called once per frame
        void Update()
        {
            ChasePlayer();
        }
        
        public void ChasePlayer()
        {
            distance = Vector3.Distance(transform.position, target.position);

            moveSpeed = speed * Time.deltaTime;
            
            if (distance <= chaseRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
            }

            if (distance <= attackRadius)
            {
                DamagePlayer(5f);
            }
        }

        public void DamagePlayer(float _damagePoints)
        {
            damage = _damagePoints;
            
            if (distance <= attackRadius)
            {
                Temp.temp.health -= damage;
            }

            if (Temp.temp.health <= 0)
            {
                InGameUI.instance.GameOver();
            }
        }

        /// <summary>
        /// Visible chase and attack radius for Aliens (Testing)
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
