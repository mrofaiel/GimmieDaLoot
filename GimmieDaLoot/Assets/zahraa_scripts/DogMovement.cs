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

    [Header("Animation")]
    [SerializeField] Animator animator;      // <-- drag child Animator here in Inspector
    [SerializeField] string vertParam = "Vert";
    [SerializeField] string stateParam = "State";

    Transform player;
    Rigidbody rb;
    bool canAttack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;                       // <- we'll move via transform, not physics
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        bool isMovingThisFrame = false;

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

        if (distance > detectionRadius * 1.5f)
        {
            player = null;
            UpdateAnimator(false);
            return;
        }

        // Look at player
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
            transform.position += move;              // <- no physics push
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

        // 0 = idle, ~1 = moving
        animator.SetFloat(vertParam, isMoving ? 1f : 0f);
        animator.SetFloat(stateParam, isMoving ? 1f : 0f);
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
