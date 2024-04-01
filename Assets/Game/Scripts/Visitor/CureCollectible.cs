
using UnityEngine;

public class CureCollectible : MonoBehaviour, ICollectible
{
    public float cureAmount;
    public LayerMask groundLayer;
    public float groundCheckDistance;
    public void Accept(IPlayerVisitor visitor)
    {
        visitor.Visit(this);
        Destroy(gameObject,0.1f);
    }

    void Update()
    {
        CheckGrounded();
    }
    private void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance, groundLayer))
        {
            OnHitGround();
        }

    }
    private void OnHitGround()
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        rb.isKinematic = true;
    }
}
