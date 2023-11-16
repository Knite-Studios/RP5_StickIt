using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Percy.EnemyVision
{
    public class ScarecrowAI : MonoBehaviour
    {
        [Header("Vision Settings")]
        public float rotationSpeed = 30f;
        public float visionAngle = 30f;
        public float visionRange = 10f;
        public LayerMask visionMask = ~(0);
        public Transform eye;

        [Header("Alert Settings")]
        public float alertRange = 20f;
        public float alertCooldown = 5f;
        public GameObject player;

        private float alertTimer = 0f;

        void Update()
        {
            Rotate();
            alertTimer += Time.deltaTime;

            if (CanSeePlayer() && alertTimer > alertCooldown)
            {
                AlertNearbyEnemies();
                alertTimer = 0f;
            }
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private bool CanSeePlayer()
        {
            if (!player) return false;

            Vector3 directionToPlayer = (player.transform.position - GetEyePosition()).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) > visionAngle / 2) return false;

            RaycastHit hit;
            if (Physics.Raycast(GetEyePosition(), directionToPlayer, out hit, visionRange, visionMask))
            {
                return hit.collider.gameObject == player;
            }

            return false;
        }

        private void AlertNearbyEnemies()
        {
            List<EnemyVision> enemiesInRange = EnemyVision.GetAllInRange(transform.position, alertRange);
            foreach (EnemyVision enemy in enemiesInRange)
            {
                enemy.Alert(transform.position);
            }
        }

        private Vector3 GetEyePosition()
        {
            return eye ? eye.position : transform.position;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, visionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, alertRange);
        }
    }
}
