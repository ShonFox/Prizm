using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    // Компонент, который управляет движением шара (настраивается в инспекторе)
    [SerializeField] private ObjectMovement _objectMovement;

    // Компонент, который управляет прыжками шара (настраивается в инспекторе)
    [SerializeField] private ObjectJump _objectJump;

    // Хранит вектор направления движения (x/y) из ввода игрока
    private Vector2 _inputDirection;

    // Флаг: true, если игрок нажал на прыжок
    private bool _isJumping;

    private void FixedUpdate()
    {
        // Если есть ввод (например, игрок двигает мышкой или клавиатурой)
        if (_inputDirection != Vector2.zero)
        {
            // вызываем обработку движения
            HandleMovement();
        }

        if (_isJumping)
        {
            // Если игрок нажал на прыжок, вызываем метод прыжка
            HandleJump();
            // Сбрасываем флаг после первого кадра, чтобы не прыгать постоянно
            _isJumping = false;
        }
    }

    private void HandleJump()
    {
        // Вызываем метод прыжка из компонента ObjectJump
        // (например, добавляем силу вверх или изменяем скорость)
        _objectJump.Jump();
    }

    private void HandleMovement()
    {
        // Создаём итоговое направление:
        Vector3 movementDirection =
            Vector3.forward * _inputDirection.y + Vector3.right * _inputDirection.x;

        // Нормализуем итоговый вектор перед передачей
        movementDirection.Normalize();

        // Передаём направление в компонент ObjectMovement,
        // который будет обрабатывать движение объекта
        _objectMovement.Move(movementDirection);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Когда игрок удерживает кнопку
            // Сохраняем текущее направление движения
            _inputDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            // Когда игрок отпустил кнопку
            // Сбрасываем вектор направления
            _inputDirection = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Когда игрок начал прыжок (нажал кнопку)
        _isJumping = true;
    }
}
