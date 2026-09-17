using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Радиус действия для взаимодействия с объектами
    // (например, подбор предметов, открытие дверей).
    // Настраивается в инспекторе. По умолчанию — 2 метра.
    [SerializeField] private float _interactionRadius = 2f;

    // Слой, на котором находятся интерактивные объекты (например, "Interactable").
    // Позволяет фильтровать объекты при проверке взаимодействия.
    [SerializeField] private LayerMask _interactableLayer;

    /// <summary>
    /// Вызывается при вводе команды взаимодействия (например, нажатие клавиши).
    /// Проверяет, началось ли нажатие, и запускает действие.
    /// </summary>
    /// <param name="context">Контекст ввода от Input System.</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Реагируем только на начало нажатия
        if (context.started)
        {
            Interact();
        }
    }

    /// <summary>
    /// Ищет интерактивные объекты в радиусе и пытается активировать первый найденный (например, дверь).
    /// </summary>
    private void Interact()
    {
        // Ищем все коллайдеры в радиусе на слое интерактивных объектов
        Collider[] hits = Physics.OverlapSphere(transform.position, _interactionRadius, _interactableLayer);

        // Перебираем результаты
        foreach (Collider hit in hits)
        {
            // Если объект имеет компонент DoorActivator — пытаемся его активировать
            if (hit.TryGetComponent<DoorActivator>(out var activator))
            {
                activator.TryActivate();
                // Выходим после первого успешного взаимодействия
                return;
            }
        }
    }
}
