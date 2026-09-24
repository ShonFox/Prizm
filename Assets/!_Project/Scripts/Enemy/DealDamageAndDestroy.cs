using UnityEngine;

public class DealDamageAndDestroy : MonoBehaviour
{
    //Кол-во урона
    [SerializeField] private float _damage = 10f;

    //Столкновение с физическим объектом
    private void OnCollisionEnter(Collision collision)
    {
        //При столкновении наносится урон
        if(collision.gameObject.TryGetComponent<ObjectDurability>(out var durability))
        {
            durability.TakeDamage(_damage);
        }
        
        //Уничтожение пули после попадания
        Destroy(gameObject);
    }
}
