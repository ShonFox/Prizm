using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    // Ссылка на контроллер двери, которую нужно активировать
    [SerializeField] private DoorController _door;

    // Ссылка на компонент отображения подсказок
    [SerializeField] private HintDisplay _hintDisplay;

    // Сообщения
    [SerializeField] private string _interactPrompt = "Нажмите [E], чтобы открыть дверь";
    [SerializeField] private string _failMessage = "Нужно набрать 100 очков!";
    [SerializeField] private string _successMessage = "Дверь разблокирована!";
    [SerializeField] private string _repeatMessage = "Дверь уже разблокирована!";

    // Время (в секундах), в течение которого подсказка остаётся видимой.
    [SerializeField] private float _hintDuration = 2f;

    // Флаг: находится ли игрок рядом и может ли взаимодействовать
    private bool _playerInRange = false;

    /// <summary>
    /// Вызывается при входе объекта в триггер активатора.
    /// Если это игрок — показываем подсказку "Нажмите E".
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInteraction>(out _))
        {
            _playerInRange = true;
            if (_door.IsActivated == false)
            {
                _hintDisplay.ShowHint(_interactPrompt, float.MaxValue);
            }
        }
    }

    /// <summary>
    /// Вызывается при выходе объекта из триггера активатора.
    /// Скрываем подсказку "Нажмите E".
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerInteraction>(out _))
        {
            _playerInRange = false;
            _hintDisplay.ShowHint("", 0f);
        }
    }

    /// <summary>
    /// Вызывается игроком при нажатии клавиши взаимодействия (например, E).
    /// Проверяет, можно ли активировать дверь, и выполняет соответствующие действия.
    /// </summary>
    public void TryActivate()
    {
        if (!_playerInRange) return;

        if (CanActivateDoor())
        {
            ActivateDoor();
            ShowTemporaryHint(_successMessage);
        }
        else if (_door.IsActivated)
        {
            ShowTemporaryHint(_repeatMessage);
        }
        else
        {
            ShowTemporaryHint(_failMessage);
        }
    }

    /// <summary>
    /// Проверяет, можно ли активировать дверь.
    /// Дверь можно активировать, только если:
    /// - она ещё не активирована
    /// - и игрок набрал очкu.
    /// </summary>
    /// <returns>True, если условия активации выполнены</returns>
    private bool CanActivateDoor()
    {
        return _door.IsActivated == false && ScoreCounter.RedScore == 1 && ScoreCounter.GreenScore == 1 && ScoreCounter.BlueScore == 1;
    }

    /// <summary>
    /// Активирует дверь, устанавливая флаг IsActivated в true.
    /// </summary>
    private void ActivateDoor()
    {
        _door.IsActivated = true;
    }

    /// <summary>
    /// Показывает временную подсказку (например, об ошибке или успехе).
    /// </summary>
    private void ShowTemporaryHint(string message)
    {
        _hintDisplay.ShowHint(message, _hintDuration);
    }
}
