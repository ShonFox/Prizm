using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Ссылка на Animator двери. Должен содержать булев параметр (например, "State"),
    // который управляет состоянием анимации (открыто/закрыто).
    [SerializeField] private Animator _animator;

    // Имя булева параметра в Animator, управляющего состоянием двери.
    // Указывается в инспекторе Unity (например, "IsOpen").
    [SerializeField] private string parameterName = "IsOpen";

    // Ссылка на компонент отображения подсказок
    [SerializeField] private HintDisplay _hintDisplay;

    // Сообщение, которое будет показано игроку, если дверь ещё не активирована.
    [SerializeField] private string _hintMessage = "Нужно активировать дверь!";

    // Время (в секундах), в течение которого подсказка остаётся видимой.
    [SerializeField] private float _hintDuration = 2f;

    // Хеш имени параметра Animator. Используется для оптимизации — 
    // методы Animator работают быстрее с хешами, чем со строками.
    private int _stateHash;

    // Публичное свойство, позволяющее другим скриптам (например, активатору)
    // включить или выключить возможность открытия двери.
    public bool IsActivated { get; set; } = false;

    /// <summary>
    /// Вызывается один раз при создании объекта или активации сцены.
    /// Инициализирует хеш параметра Animator для оптимизации.
    /// </summary>
    private void Awake()
    {
        // Преобразуем имя параметра в числовой хеш один раз при запуске.
        // Это ускоряет последующие вызовы SetBool в Animator.
        _stateHash = Animator.StringToHash(parameterName);
    }

    /// <summary>
    /// Вызывается автоматически Unity, когда другой объект входит в триггерную зону.
    /// Проверяет, является ли объект игроком, и реагирует в зависимости от состояния двери.
    /// </summary>
    /// <param name="other">Коллайдер объекта, вошедшего в триггер</param>
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли у вошедшего объекта компонент BallController —
        // это признак того, что это игрок.
        if (other.TryGetComponent<BallController>(out _))
        {
            // Если дверь уже активирована (например, получено достаточно очков)
            if (IsActivated)
            {
                // Открываем её
                Open();
            }
            // Если дверь ещё не активирована 
            else
            {
                // Показываем подсказку игроку.
                ShowHint();
            }
        }
    }

    /// <summary>
    /// Вызывается автоматически Unity, когда объект выходит из триггерной зоны.
    /// Закрывает дверь, если она была открыта и активирована.
    /// </summary>
    /// <param name="other">Коллайдер объекта, вышедшего из триггера</param>
    private void OnTriggerExit(Collider other)
    {
        // Проверяем, что вышедший объект — это игрок.
        if (other.TryGetComponent<BallController>(out _))
        {
            // Закрываем дверь, только если она активирована —
            // неактивированная дверь никогда не открывается, значит, и закрывать нечего.
            if (IsActivated)
            {
                Close();
            }
        }
    }

    /// <summary>
    /// Открывает дверь, устанавливая булевый параметр Animator в true.
    /// Анимация открытия должна быть настроена на изменение этого параметра.
    /// </summary>
    private void Open()
    {
        _animator.SetBool(_stateHash, true);
    }

    /// <summary>
    /// Закрывает дверь, устанавливая булев параметр Animator в false.
    /// Анимация закрытия должна быть настроена на это изменение.
    /// </summary>
    private void Close()
    {
        _animator.SetBool(_stateHash, false);
    }

    /// <summary>
    /// Инициирует отображение подсказки, если она ещё не показана.
    /// </summary>
    private void ShowHint()
    {
        _hintDisplay.ShowHint(_hintMessage, _hintDuration);
    }
}
