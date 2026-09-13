using UnityEngine;

public class InstantKill : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Пытаемся найти на столкнувшемся объекте компонент ObjectDurability.
        if (collision.gameObject.TryGetComponent<ObjectDurability>(out var durability))
        {
            // Если компонент прочности найден, вызываем метод мгновенного уничтожения.
            durability.KillInstantly();
        }
    }
}
