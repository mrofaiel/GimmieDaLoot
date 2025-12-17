using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyHealthTest))]
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

    [Header("Animation")]
    [SerializeField] Animator animator;     // drag Dog_001 (with Animator) here
    [SerializeField] string vertParam = "Vert";
    [SerializeField] string stateParam = "State";

    Transform player;
    Rigidbody rb;
    EnemyHealthTest health;
    bool canAttack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;                      // we move via transform, not physics forces
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        health = GetComponent<EnemyHealthTest>();
    }

    void Update()
    {
        // If this dog is dead, do nothing
        if (health != null && health.IsDead)
        {
            UpdateAnimator(false);
            return;
        }

        bool isMovingThisFrame = false;

        // Find player if we don't have one yet
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

            UpdateAnimator(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // Lose target if they get far away
        if (distance > detectionRadius * 1.5f)
        {
            player = null;
            UpdateAnimator(false);
            return;
        }

        // Rotate toward player
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
        }

        // Move toward player, but don't overlap too much
        if (distance > stopDistance)
        {
            Vector3 move = transform.forward * chaseSpeed * Time.deltaTime;
            transform.position += move;             // kinematic move
            isMovingThisFrame = true;
        }

        // Attack when close
        if (distance <= biteRange && canAttack)
        {
            StartCoroutine(Attack());
        }

        UpdateAnimator(isMovingThisFrame);
    }

    void UpdateAnimator(bool isMoving)
    {
        if (animator == null) return;

        // 0 = idle, 1 = moving/running
        animator.SetFloat(vertParam, isMoving ? 1f : 0f);
        animator.SetFloat(stateParam, isMoving ? 1f : 0f);
    }

IEnumerator Attack()
{
    canAttack = false;

    Debug.Log("Dog bites the player!");

    if (player != null)
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(biteDamage);  // <-- THIS DOES REAL DAMAGE
        }
    }

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
