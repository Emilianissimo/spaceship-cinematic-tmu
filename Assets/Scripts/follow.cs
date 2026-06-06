using UnityEngine;

public class CartFollower : MonoBehaviour
{
    [SerializeField] private Transform targetCart;

    private void LateUpdate()
    {
        if (targetCart != null)
        {
            transform.position = targetCart.position;
            transform.rotation = targetCart.rotation;
        }
    }
}
