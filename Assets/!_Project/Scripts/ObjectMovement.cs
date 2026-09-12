using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Физический компонент объекта, необходимый для изменения скорости
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GroundCheck _groundCheck;

    // Ускорение на земле (в диапазоне 0-10)
    [SerializeField, Range(0f, 10f)] private float _groundAcceleration = 4f;

    // Ускорение в воздухе (в диапазоне 0-10)
    [SerializeField, Range(0f, 10f)] private float _airAcceleration = 3f;

    // Максимальная скорость движения (в диапазоне 0-10)
    [SerializeField, Range(0f, 10f)] private float _maxSpeed = 6f;

    // Применяет движение объекта в указанном направлении
    public void Move(Vector3 direction)
    {
        // Вычисляем максимально возможную скорость в заданном направлении
        Vector3 maximalVelocity = direction * _maxSpeed;

        // Сохраняем текущую скорость объекта
        Vector3 currentVelocity = _rigidbody.linearVelocity;

        // Сохраняем вертикальную скорость отдельно (чтобы не менять её)
        float verticalSpeed = currentVelocity.y;

        // Обнуляем вертикальную компоненту для расчёта только горизонтального движения
        currentVelocity.y = 0;

        float currentAcceleration = _groundCheck.IsGrounded() ? _groundAcceleration : _airAcceleration;

        // Рассчитываем приращение скорости за один кадр
        // Используем Time.fixedDeltaTime для корректности физики
        float deltaAcceleration = currentAcceleration * Time.fixedDeltaTime;

        // Плавно увеличиваем скорость до максимальной
        // Vector3.MoveTowards обеспечивает плавный переход
        currentVelocity = Vector3.MoveTowards(currentVelocity, maximalVelocity, deltaAcceleration);

        // Восстанавливаем сохранённую вертикальную скорость
        currentVelocity.y = verticalSpeed;

        // Применяем новую скорость к объекту
        _rigidbody.linearVelocity = currentVelocity;
    }
}
