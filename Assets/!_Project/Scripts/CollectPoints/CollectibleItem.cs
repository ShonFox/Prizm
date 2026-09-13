using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    
    // Количество очков, начисляемых за сбор этого предмета.
    // В текущей задаче ставим 1, но можно менять при необходимости.
    [SerializeField] private int _points = 1;

    // Цвет предмета — определяет, в какую категорию счёта он попадёт.
    // Назначается в инспекторе Unity для каждого объекта отдельно.
    [SerializeField] private ItemColor _color;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли на объекте-столкновении компонент ItemCollector.
        // Это нужно, чтобы очки начислялись только игроку (или нужному объекту), а не всему подряд.
        if (other.TryGetComponent<ItemCollector>(out var itemCollector))
        {
            // Передаём и количество очков, и цвет предмета в систему сбора.
            itemCollector.CollectPoints(_points, _color);

            // Удаляем предмет со сцены после сбора, чтобы его нельзя было собрать повторно.
            Destroy(gameObject);
        }
    }

    public enum ItemColor
    {
        Red,
        Green,
        Blue
    }
}
