using System.Collections;
using TMPro;
using UnityEngine;

public class HintDisplay : MonoBehaviour
{
    // Ссылка на текстовый элемент, в котором будет отображаться подсказка
    [SerializeField] private TMP_Text _textComponent;

    // Текущая сопрограмма, отвечающая за скрытие подсказки
    private Coroutine _currentCoroutine = null;

    // Свойство возвращает true, если таймер запущен, иначе false
    public bool IsCoroutineRunning => _currentCoroutine != null;

    public void ShowHint(string message, float duration)
    {
        // Получаем текущий текст из компонента
        string currentText = _textComponent.text;

        // Если сообщения отличаются 
        if (currentText != message)
        {
            // Останавливаем сопрограмму, если запущена
            if (IsCoroutineRunning)
            {
                StopCoroutine(_currentCoroutine);
            }

            // Запускаем новую сопрограмму для вывода и скрытия сообщения
            // через указанное время
            _currentCoroutine = StartCoroutine(HideAfterDelay(message, duration));
        }
    }

    /// <summary>
    /// Сопрограмма, которая выводит подсказку, ждёт заданное время и
    /// затем скрывает подсказку, очищая текстовое поле.
    /// </summary>
    /// <param name="delay">Время ожидания перед скрытием</param>
    private IEnumerator HideAfterDelay(string message, float delay)
    {
        // Отображаем подсказку
        _textComponent.text = message;

        // Ожидаем указанное время
        yield return new WaitForSeconds(delay);

        // Скрываем подсказку
        _textComponent.text = "";

        // Сбрасываем ссылку на сопрограмму
        _currentCoroutine = null;
    }
}
