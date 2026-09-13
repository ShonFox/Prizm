using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    // Начальные углы поворота: 
    // — Вертикальный угол (pitch): 45° (смотрим немного вниз)
    // — Горизонтальный угол (yaw): 0° (смотрим прямо)
    private const float _startVerticalAngle = 45f;
    private const float _startHorizontalAngle = 0f;

    // Ссылка на трансформ игрока 
    [SerializeField] private Transform _playerTransform;

    // Радиус орбиты (расстояние от игрока до камеры)
    [SerializeField, Range(1f, 20f)] private float _radius = 5f;

    // Минимальный и максимальный вертикальные углы (ограничивают наклон камеры)
    [SerializeField, Range(0f, 89f)] private float _minVerticalAngle = 15f;
    [SerializeField, Range(0f, 89f)] private float _maxVerticalAngle = 60f;

    // Скорость вращения камеры (в градусах в секунду)
    [SerializeField, Range(1f, 360f)] private float _turnSpeed = 50f;

    // Вектор смещения мыши (накопленный за кадр)
    private Vector2 _inputDeltaPointMove;

    // Текущие углы поворота камеры (вертикальный и горизонтальный)
    private Vector2 _orbitAngles;

    private void Awake()
    {
        // Устанавливаем начальные углы поворота
        _orbitAngles = new Vector2(_startVerticalAngle, _startHorizontalAngle);
    }

    private void LateUpdate()
    {
        // 1. Создаём кватернион (Quaternion), который представляет текущую ориентацию камеры:
        //    — _orbitAngles.x: вертикальный угол (pitch)
        //    — _orbitAngles.y: горизонтальный угол (yaw)
        //    — 0f: угол по оси Z не используется
        Quaternion rotation = Quaternion.Euler(_orbitAngles.x, _orbitAngles.y, 0f);

        // 2. Вычисляем направление камеры относительно её ориентации:
        //    — Vector3.forward — это направление вперёд по оси Z
        //    — Умножение на кватернион даст направление в мировых координатах
        Vector3 direction = rotation * Vector3.forward;

        // 3. Рассчитываем абсолютную позицию камеры:
        //    Позиция игрока + направление \* радиус орбиты
        Vector3 position = _playerTransform.position - direction * _radius;

        // 4. Применяем новую позицию и поворот к камере
        transform.position = position;
        transform.rotation = rotation;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Получаем вектор смещения мыши
        Vector2 input = context.ReadValue<Vector2>();

        // Инвертируем Y-ось для естественного поведения:
        // — При движении мыши вверх камера смотрит выше
        // — При движении мыши вниз камера смотрит ниже
        float deltaX = input.x;  // Горизонтальное смещение → изменяет yaw (поворот вокруг) Y)
        float deltaY = -input.y; // Вертикальное смещение → изменяет pitch (наклон камеры)

        // Накапливаем изменения углов:
        //  Умножаем на скорость вращения и время кадра (Time.unscaledDeltaTime)
        _orbitAngles.x += deltaY * _turnSpeed  *Time.unscaledDeltaTime;
        _orbitAngles.y += deltaX * _turnSpeed * Time.unscaledDeltaTime;

        // Ограничиваем вертикальный угол (pitch):
        // — \`_minVerticalAngle\` — минимальный угол (камера не может смотреть слишком вниз)
        // — \`_maxVerticalAngle\` — максимальный угол (камера не может смотреть слишком вверх)
        _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, _minVerticalAngle, _maxVerticalAngle);

        // Делаем горизонтальный угол цикличным (от 0° до 360°):
        //  Если угол превышает 360°, он обнуляется
        _orbitAngles.y = Mathf.Repeat(_orbitAngles.y, 360f);
    }
}
