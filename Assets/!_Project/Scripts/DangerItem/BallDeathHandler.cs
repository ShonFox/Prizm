using System.Collections;
using UnityEngine;

public class BallDeathHandler : MonoBehaviour
{
    [SerializeField] private SceneSwitcher _sceneSwitcher;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;

    // Время задержки в секундах перед загрузкой сцены поражения.
    [SerializeField] private float _delayBeforeGameOver = 3.0f;


    /// <summary>
    /// Этот метод должен быть назначен в инспекторе как реакция на событие OnDestroyed
    /// компонента прочности (например, ObjectDurability).
    /// </summary>
    public void HandleDestroyed()
    {
        DisableRenderer();
        DisablePhysics();
        StartCoroutine(WaitAndLoadFatalScene());
    }

    /// <summary>
    /// Отключает все компоненты Renderer на объекте и его дочерних элементах,
    /// делая шар невидимым.
    /// </summary>
    private void DisableRenderer()
    {
        _renderer.enabled = false;
    }

    /// <summary>
    /// Отключает физическое взаимодействие шара:
    /// — Переводит Rigidbody в режим кинематики.
    /// — Отключает обнаружение коллизий.
    /// — Отключает коллайдер.
    /// </summary>
    private void DisablePhysics()
    {
        _rigidBody.isKinematic = true;
        _rigidBody.detectCollisions = false;
        _collider.enabled = false;
    }

    /// <summary>
    /// Сопрограмма:
    /// 1. Ждёт указанное время (для визуальных или звуковых эффектов).
    /// 2. Загружает сцену поражения.
    /// </summary>
    private IEnumerator WaitAndLoadFatalScene()
    {
        yield return new WaitForSeconds(_delayBeforeGameOver);
        _sceneSwitcher.LoadFatalScene();
    }
}
