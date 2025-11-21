using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DogMovement : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] string playerTag = "Player";

    [Header("Movement Settings")]
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float turnSpeed = 10f;

    [Header("Attack Settings")]
    [SerializeField] int biteDamage = 20;
    [SerializeField] float attackCooldown = 1.2f;
    [SerializeField] float biteRange = 1.5f;

    Transform player;
    Rigidbody rb;
    bool canAttack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (player == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag(playerTag))
                {
                    player = hit.transform;
                    break;
                }
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRadius * 1.5f)
        {
            player = null;
            return;
        }

        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
        }

        if (distance > stopDistance)
        {
            Vector3 move = transform.forward * chaseSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }

        if (distance <= biteRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        Debug.Log("Dog bites the player!");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, biteRange);
    }
#endif
}
