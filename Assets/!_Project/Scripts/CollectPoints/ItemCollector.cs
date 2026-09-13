using UnityEngine;
using static CollectibleItem;

public class ItemCollector : MonoBehaviour
{
    // Ссылка на объект, который управляет подсчётом и отображением очков.
    // Назначается в инспекторе.
    [SerializeField] private ScoreCounter _scoreCounter;

    /// <summary>
    /// Метод для передачи очков и цвета в счётчик.
    /// Вызывается из CollectibleItem при столкновении.
    /// </summary>
    public void CollectPoints(int points, ItemColor color)
    {
        _scoreCounter.AddScore(points, color);
    }
}
