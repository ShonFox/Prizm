using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolPoints;
    //допустимое расстояние
    [SerializeField] private float _tolerance = 0.5f;

    //индекс текущей точки патрулирования в массиве
    private int _currentPointIndex = 0;

    //возвращает позицию следующей точки, к которой должен двигаться враг
    public Vector3 GetTargetPosition()
    {
        if (IsAtCurrentPatrolPoint())
        {
            NextPatrolPoint();
        }
        return _patrolPoints[_currentPointIndex].position;
    }

    //увеличивает индекс текущей точки
    public void NextPatrolPoint()
    {
        if (_patrolPoints.Length > 1)
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _patrolPoints.Length) 
            {
                _currentPointIndex = 0;
            }
        }
    }

    //вычисляет двумерное расстояние между текущей позицией объекта
    //и позицией текущей точки патрулирования
    public bool IsAtCurrentPatrolPoint()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPointPosition = _patrolPoints[_currentPointIndex].position;

        float distance = Vector2.Distance(
            new Vector2(currentPosition.x, currentPosition.z),
            new Vector2(targetPointPosition.x, targetPointPosition.z));

        return distance <= _tolerance;
    }
}
