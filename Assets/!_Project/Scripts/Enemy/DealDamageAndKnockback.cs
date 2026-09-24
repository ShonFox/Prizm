using UnityEngine;

public class DealDamageAndKnockback : MonoBehaviour
{
    //задаёт количество урона
    [SerializeField] private float _damage = 10f;
    //задаёт силу импульса
    [SerializeField] private float _knockbackForce = 5f;

    //при начале физического контакта между коллайдерами
    private void OnCollisionEnter(Collision collision)
    {
        //есть ли у столкнувшегося объекта компонент ObjectDurability
        if (collision.gameObject.TryGetComponent<ObjectDurability>(out var durability))
        {
            durability.TakeDamage(_damage);
        }

        if (collision.rigidbody)
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.AddForce(direction * _knockbackForce, ForceMode.Impulse);
        }
    }
}
