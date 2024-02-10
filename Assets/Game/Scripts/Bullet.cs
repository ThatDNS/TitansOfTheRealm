using UnityEngine;

/// <summary>
/// A game object representing a projectile and apply damage when hit
/// It implements the IDoDamage interface, so when hit the object knows what
/// methods to use
/// </summary>
public class Bullet : MonoBehaviour, IDoDamage
{
    /// <summary>
    /// Expose two properties, one to pass a prefab that can be used as special effect
    /// and the amount of damage applied
    /// </summary>
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private int damage = 35;

    /// <summary>
    /// Make sure the game object is destroyed after 3 seconds
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, 3);
    }

    /// <summary>
    /// Destroy this game object after colliding 
    /// </summary>
    private void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Return tha amount of damage the bullet (projectile) will apply when hit
    /// </summary>
    /// <returns>int: the damage amount</returns>
    public int GetDamage() { return damage; }

    /// <summary>
    /// Return the game object (prefab) that can be used to apply a special effect
    /// </summary>
    /// <returns>GameObject: the prefab</returns>
    public GameObject GetHitPrefab() { return bulletHole; }
}