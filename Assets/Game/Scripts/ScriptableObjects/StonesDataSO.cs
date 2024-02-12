using UnityEngine;

/// <summary>
/// ScriptableObject representing currency data.
/// </summary>
[CreateAssetMenu(fileName = "NewStone", menuName = "Stone")]
public class StonesDataSO : ScriptableObject
{
    public string stoneName;
    public StoneType stoneType;
    public StoneRarity stoneRarity;
    public Sprite stoneSprite;

    /// <summary>
    /// Enum defining various types of currencies.
    /// </summary>
    public enum StoneType
    {
        Upgrade,
        Unlocking,
        Crafting
    }

    /// <summary>
    /// Enum defining the rarity levels of currencies.
    /// </summary>
    public enum StoneRarity
    {
        Normal,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Special
    }
}