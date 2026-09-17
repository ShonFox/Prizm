using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    // Начальные углы поворота
    private const float _startVerticalAngle = 45f;
    private const float _startHorizontalAngle = 0f;

    // Ссылка на трансформ игрока
    [SerializeField] private Transform _playerTransform;

    // Радиус орбиты (базовое расстояние)
    [SerializeField, Range(1f, 20f)] private float _radius = 5f;

    // Ограничения вертикального угла (pitch)
    [SerializeField, Range(0f, 89f)] private float _minVerticalAngle = 15f;
    [SerializeField, Range(0f, 89f)] private float _maxVerticalAngle = 60f;

    // Скорость вращения
    [SerializeField, Range(1f, 360f)] private float _turnSpeed = 50f;

    // Настройки коллизий (новые поля)
    [Header("Collision Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _collisionOffset = 0.5f; // Отступ от препятствия
    [SerializeField] private LayerMask _obstacleLayerMask; // Слой препятствий

    // Входные данные и углы
    private Vector2 _inputDeltaPointMove;
    private Vector2 _orbitAngles;

    private void Awake()
    {
        if (_playerTransform == null)
        {
            Debug.LogError("[CameraOrbit] Не назначен Player Transform в инспекторе!");
            enabled = false;
            return;
        }

        _orbitAngles = new Vector2(_startVerticalAngle, _startHorizontalAngle);
    }

    private void LateUpdate()
    {
        // 1. Создаём кватернион текущей ориентации камеры
        Quaternion rotation = Quaternion.Euler(_orbitAngles.x, _orbitAngles.y, 0f);

        // 2. Вычисляем направление вперёд в мировых координатах
        Vector3 direction = rotation * Vector3.forward;

        // 3. Рассчитываем идеальную позицию камеры (без учёта препятствий)
        Vector3 idealPosition = _playerTransform.position - direction * _radius;

        // 4. ПРОВЕРКА НА ПРЕПЯТСТВИЯ (Linecast)
        // Проверяем отрезок от игрока до идеальной позиции камеры
        if (Physics.Linecast(_playerTransform.position, idealPosition, out RaycastHit hit, _obstacleLayerMask))
        {
            // Если есть препятствие, вычисляем безопасное расстояние до него
            float distanceToHit = Vector3.Distance(_playerTransform.position, hit.point);
            float adjustedDistance = distanceToHit - _collisionOffset;

            // Не позволяем камере оказаться внутри игрока (минимальный радиус)
            adjustedDistance = Mathf.Max(adjustedDistance, 0.5f);

            // Вычисляем новую позицию с учётом препятствия
            idealPosition = _playerTransform.position - direction * adjustedDistance;
        }

        // 5. Применяем итоговую позицию и поворот
        transform.position = idealPosition;
        transform.rotation = rotation;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Получаем вектор смещения мыши
        Vector2 input = context.ReadValue<Vector2>();

        // Инвертируем Y-ось для естественного поведения
        float deltaX = input.x;
        float deltaY = -input.y;

        // Накапливаем изменения углов
        _orbitAngles.x += deltaY * _turnSpeed * Time.unscaledDeltaTime;
        _orbitAngles.y += deltaX * _turnSpeed * Time.unscaledDeltaTime;

        // Ограничиваем вертикальный угол (pitch)
        _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, _minVerticalAngle, _maxVerticalAngle);

        // Делаем горизонтальный угол цикличным (0–360 градусов)
        _orbitAngles.y = Mathf.Repeat(_orbitAngles.y, 360f);
    }
}
