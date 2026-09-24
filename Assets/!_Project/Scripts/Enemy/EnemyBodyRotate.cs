using UnityEngine;

public class EnemyBodyRotate : MonoBehaviour
{
    //задать скорость вращения объекта в градусах в секунду
    [SerializeField] private float _rotationSpeed = 30f;
    //задаёт ось, вокруг которой будет происходить вращение
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;

    //вызывается каждый кадр, происходит вычисление угла поворота
    private void Update()
    {
        transform.Rotate(_rotationAxis, _rotationSpeed * Time.deltaTime);
    }
}
