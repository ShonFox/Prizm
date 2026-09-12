using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Слои объектов, которые считаются "землёй"
    [SerializeField] private LayerMask groundLayerMask;

    // Максимальная длина луча, который будет проверяться
    [SerializeField] private float raycastDistance = 0.51f;

    // Проверяет, находится ли объект на поверхности "земли".
    public bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(
            transform.position,       
            Vector3.down,             
            raycastDistance,          
            groundLayerMask           
        );

        return isGrounded; 
    }
}
