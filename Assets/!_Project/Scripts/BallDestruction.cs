using UnityEngine;

public class BallDestruction : MonoBehaviour
{
    [SerializeField] private GameObject deathEffectPrefab;

    public void Die()
    {
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
    }
}
