using UnityEngine;

public class ObjectJump : MonoBehaviour
{
    // Физический компонент объекта, необходимый для изменения скорости
    [SerializeField] private Rigidbody _rigidbody;

    // Компонент проверки контакта с землёй (GroundCheck)
    [SerializeField] private GroundCheck _groundCheck;

    // Сила прыжка при контакте с землёй (в диапазоне 0-10)
    [SerializeField, Range(0f, 10f)] private float _groundJumpHeight = 2f;

    // Сила второго прыжка в воздухе (в диапазоне 0-10)
    [SerializeField, Range(0f, 10f)] private float _airJumpHeight = 1f;

    // Флаг: true, если игрок уже использовал двойной прыжок
    private bool _hasDoubleJumped = false;

    public void Jump()
    {
        // Если игрок стоит на земле
        if (_groundCheck.IsGrounded())
        {
            // Применяем обычный прыжок с полной силой
            ApplyJumpForce(_groundJumpHeight);
            // Разрешаем использовать двойной прыжок
            _hasDoubleJumped = true;
        }
        // Если игрок в воздухе и ещё не использовал двойной прыжок
        else if (_hasDoubleJumped)
        {
            // Применяем второй прыжок с меньшей силой
            ApplyJumpForce(_airJumpHeight);
            // Блокируем повторный двойной прыжок
            _hasDoubleJumped = false;
        }
    }

    private void ApplyJumpForce(float jumpHeight)
    {
        // Получаем текущую скорость объекта
        var velocity = _rigidbody.linearVelocity;

        // Если объект падает вниз (скорость Y отрицательна),
        // сбрасываем скорость до нуля, иначе прыжок только замедлит падение
        velocity.y = velocity.y < 0f ? 0f : velocity.y;

        // Формула для расчёта скорости для прыжка на заданную высоту:
        // v = sqrt(-2 \* g \* h), где g - гравитация, h - высота
        velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);

        // Применяем новую скорость к объекту
        _rigidbody.linearVelocity = velocity;
    }
}
