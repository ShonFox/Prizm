using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamage : MonoBehaviour
{
    // Урон в секунду (например, 20 = 20 единиц прочности в секунду).
    [SerializeField] private float _damagePerSecond = 20f;

    // Интервал между нанесениями урона (в секундах).
    // Например, 0.1 = 10 раз в секунду.
    [SerializeField] private float _damageInterval = 0.1f;

    // Список всех объектов, находящихся в контакте и имеющих прочность.
    private List<ObjectDurability> _objectsInContact = new List<ObjectDurability>();

    // Флаг, показывающий, запущена ли сопрограмма нанесения урона.
    private bool _isCoroutineRunning = false;

    /// <summary>
    /// Вызывается при начале физического контакта с другим объектом.
    /// Если у объекта есть прочность, он добавляется в список (если ещё не добавлен).
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, есть ли у объекта система прочности.
        if (collision.gameObject.TryGetComponent<ObjectDurability>(out var durability))
        {
            // Если объекта ещё нет в списке
            // (избегаем дубликатов)
            if (!_objectsInContact.Contains(durability))
            {
                // Добавляем в список ссылку на компонент ObjectDurability.
                _objectsInContact.Add(durability);
            }

            // Если сопрограмма не запущена
            if (!_isCoroutineRunning)
            {
                // Запускаем сопрограмму.
                StartCoroutine(DamageCoroutine());
                // Устанавливаем флаг, чтобы случайно 
                // не запустить несколько копий сопрограмм.
                _isCoroutineRunning = true;
            }
        }
    }

    /// <summary>
    /// Вызывается при завершении физического контакта с другим объектом.
    /// Удаляет объект из списка, чтобы перестать наносить ему урон.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<ObjectDurability>(out var durability))
        {
            _objectsInContact.Remove(durability);
        }
    }

    /// <summary>
    /// Сопрограмма наносит урон всем объектам в списке с заданным интервалом.
    /// Останавливается автоматически, когда список становится пустым.
    /// </summary>
    private IEnumerator DamageCoroutine()
    {
        // Создаём объект WaitForSeconds с указанным интервалом времени.
        WaitForSeconds waitInterval = new WaitForSeconds(_damageInterval);

        // Сопрограмма выполняется, пока в списке есть хотя бы один объект.
        while (_objectsInContact.Count > 0)
        {
            // Рассчитываем величину урона за интервал времени.
            float damage = _damagePerSecond  * _damageInterval;

            // Обходим список С КОНЦА к началу (это важно!).
            // Это позволяет безопасно удалять элементы из списка во время перебора.
            // Почему это безопасно:
            // — При удалении элемента все элементы ПРАВЕЕ него сдвигаются влево.
            // — Так как мы идём СПРАВА НАЛЕВО, мы уже обработали всё, что правее.
            // — Элементы ЛЕВЕЕ не сдвигаются относительно текущего индекса,
            //   поэтому мы не пропустим ни один объект и не выйдем за границы списка.
            for (int i = _objectsInContact.Count - 1; i >= 0; i--)
            {
                ObjectDurability durability = _objectsInContact[i] ;

                // Проверяем, что объект всё ещё существует и жив.
                if (durability != null && durability.IsAlive)
                {
                    // Наносим урон живому объекту.
                    durability.TakeDamage(damage);
                }
                else
                {
                    // Если объект удалён со сцены или его прочность равна 0,
                    // удаляем его из списка.
                    _objectsInContact.RemoveAt(i);
                }
            }

            // Ждём заданный интервал между ударами.
            yield return waitInterval;
        }

        // Сбрасываем флаг, чтобы запустить сопрограмму заново,
        // когда в список добавятся новые объекты.
        _isCoroutineRunning = false;
    }
}
