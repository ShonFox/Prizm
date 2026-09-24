using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerZoneDetector _zoneDetector;
    [SerializeField] private PatrolPoints _patrolPoints;
    [SerializeField] private ObjectMovement _movement;
    [SerializeField] private EnemyShooter _shooter;

    //происходит проверка, находится ли игрок в зоне
    private void Update()
    {
        if (_zoneDetector.IsPlayerInZone)
        {
            PursuePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // получается позиция игрока
    private void PursuePlayer()
    {
        Vector3 targetPosition = _zoneDetector.GetTargetPosition();
        Vector3 directionToTarget = GetDirectionToTarget(targetPosition);
        MoveAndShoot(directionToTarget, true);
    }

    //получается позиция следующей точки патрулирования
    private void Patrol()
    {
        Vector3 targetPosition = _patrolPoints.GetTargetPosition();
        Vector3 directionToTarget = GetDirectionToTarget(targetPosition);
        MoveAndShoot(directionToTarget, false);
    }

    //вычисляет вектор направления от текущей позиции врага к целевой
    private Vector3 GetDirectionToTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0f;
        directionToTarget = directionToTarget.normalized;
        return directionToTarget;
    }

    //централизованно вызывает методы других компонентов
    private void MoveAndShoot(Vector3 direction, bool canShoot)
    {
        _movement.Move(direction);
        _shooter.Shoot(canShoot);
    }
}
