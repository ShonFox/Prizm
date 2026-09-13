using TMPro;
using UnityEngine;
using static CollectibleItem;

public class ScoreCounter : MonoBehaviour
{
    // Отдельные статические переменные для счёта по каждому цвету.
    // Статика позволяет хранить состояние между объектами, но в рамках одной сцены.
    private static int _redScore = 0;
    private static int _greenScore = 0;
    private static int _blueScore = 0;

    // Ссылки на текстовые компоненты для отображения счёта по каждому цвету.
    // Нужно назначить в инспекторе соответствующие TMP_Text.
    [SerializeField] private TMP_Text _redText;
    [SerializeField] private TMP_Text _greenText;
    [SerializeField] private TMP_Text _blueText;

    // Публичные свойства для доступа к текущему счёту извне (только чтение).
    public static int RedScore => _redScore;
    public static int GreenScore => _greenScore;
    public static int BlueScore => _blueScore;

    // При старте сцены сбрасываем все счётчики в ноль.
    private void Awake()
    {
        ResetScore();
    }

    /// <summary>
    /// Сбрасывает все счётчики на ноль и обновляет отображение на экране.
    /// Удобно использовать при перезапуске уровня или начале новой игры.
    /// </summary>
    public void ResetScore()
    {
        _redScore = 0;
        _greenScore = 0;
        _blueScore = 0;
        DisplayAll();
    }

    /// <summary>
    /// Добавляет очки к соответствующему цвету.
    /// Проверяет, что очки положительные, чтобы избежать ошибок логики.
    /// </summary>
    public void AddScore(int points, ItemColor color)
    {
        if (points <= 0)
        {
            Debug.LogWarning("ScoreCounter: Попытка добавить неположительное количество очков!");
            return;
        }

        // В зависимости от цвета предмета увеличиваем нужный счётчик.
        switch (color)
        {
            case ItemColor.Red:
                _redScore = Mathf.Clamp(_redScore + points, 0, int.MaxValue);
                break;
            case ItemColor.Green:
                _greenScore = Mathf.Clamp(_greenScore + points, 0, int.MaxValue);
                break;
            case ItemColor.Blue:
                _blueScore = Mathf.Clamp(_blueScore + points, 0, int.MaxValue);
                break;
        }

        // После изменения любого из счётчиков обновляем все текстовые поля.
        DisplayAll();
    }

    /// <summary>
    /// Обновляет текст в UI для всех трёх цветов.
    /// Проверка на null защищает от ошибок, если какой-то текст не назначен в инспекторе.
    /// Формат D2 выводит число с ведущим нулём (например, 01, 09, 10).
    /// </summary>
    private void DisplayAll()
    {
        if (_redText != null)
            _redText.text = $"{_redScore:D2}";

        if (_greenText != null)
            _greenText.text = $"{_greenScore:D2}";

        if (_blueText != null)
            _blueText.text = $"{_blueScore:D2}";
    }
}
