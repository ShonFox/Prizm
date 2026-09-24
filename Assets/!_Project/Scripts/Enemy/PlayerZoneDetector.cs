using UnityEngine;

public class PlayerZoneDetector : MonoBehaviour
{
    //ввод игрока в инспекторе
    [SerializeField] private BallController _player;

    //Поле хранящее состояние
    private bool _isPlayerInZone = false;

    public bool IsPlayerInZone => _isPlayerInZone;

    //Вход в тригерную зону
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallController>(out _))
        {
            _isPlayerInZone = true;
        }
    }

    //Выход из тригерной зоны
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BallController>(out _))
        {
            _isPlayerInZone = false;
        }
    }

    //Возвращает текущую позицию игрового объекта
    public Vector3 GetTargetPosition()
    {
        return _player.transform.position;
    }
}
