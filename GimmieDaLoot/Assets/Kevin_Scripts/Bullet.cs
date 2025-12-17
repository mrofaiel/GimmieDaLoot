using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 50f;
    public float damage = 5f;   // make this bigger so you can SEE it working
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with Player
        if (other.CompareTag("Player"))
            return;

        // Damage enemy on this collider OR its parents
        EnemyHealthTest enemy = other.GetComponentInParent<EnemyHealthTest>();
        if (enemy != null)
        {
            Debug.Log("Bullet hit enemy: " + enemy.name);
            enemy.TakeDamage(damage);
        }

        // Destroy bullet on ANY non-player hit
        Destroy(gameObject);
    }

    
}
