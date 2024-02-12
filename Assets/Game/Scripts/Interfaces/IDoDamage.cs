using UnityEngine;

/// <summary>
/// Interface that must be implemented by all objects that will cause damage after collision.
/// </summary>
public interface IDoDamage
{
    /// <summary>
    /// Returns the amount of damage dealt.
    /// </summary>
    /// <returns>int: the damage amount</returns>
    public int GetDamage();

    /// <summary>
    /// Returns a prefab that can be used as a special effect, a particle system, an image, etc.
    /// </summary>
    /// <returns>GameObject: a prefab</returns>
    public GameObject GetHitPrefab();
}

