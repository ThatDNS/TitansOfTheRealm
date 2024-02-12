using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages player-related data, specifically handling various currencies and their operations.
/// </summary>
public class Player : MonoBehaviour
{
    #region Structs
    /// <summary>
    /// Represents an event when the Soulstone currency is updated.
    /// </summary>
    public struct SoulstoneUpdatedEvent
    {
        public SoulstoneCache UpdatedStones { get; private set; }

        public SoulstoneUpdatedEvent(SoulstoneCache updatedStones)
        {
            UpdatedStones = updatedStones;
        }
    }

    /// <summary>
    /// Struct to hold Soulstone currency data and quantity.
    /// </summary>
    [Serializable]
    public struct SoulstoneCache
    {
        public StonesDataSO soulstoneData;
        public int quantity;
    }
    #endregion

    #region Variables
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "playerSave.json");

    [SerializeField]
    private List<SoulstoneCache> stones = new List<SoulstoneCache>();
    #endregion

    #region Unity Methods
    private void Start()
    {
        LoadPlayerData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
    #endregion

    #region Currency Management
    /// <summary>
    /// Updates the amount of a specific Soulstone currency.
    /// </summary>
    /// <param name="soulstone">The Soulstone to update.</param>
    /// <param name="amount">The amount to add or subtract.</param>
    public void UpdateCurrencyAmount(StonesDataSO soulstone, int amount)
    {
        SoulstoneCache soulstoneCache = stones.Find(c => c.soulstoneData == soulstone);
        if (soulstoneCache.soulstoneData != null)
        {
            soulstoneCache.quantity += amount;
            int index = stones.FindIndex(c => c.soulstoneData == soulstone);
            if (index != -1) stones[index] = soulstoneCache;

            EventBus.Instance.Publish(new SoulstoneUpdatedEvent(stones[index]));
        }
    }

    /// <summary>
    /// Retrieves the quantity of a specific Soulstone currency.
    /// </summary>
    /// <param name="soulstone">The Soulstone to retrieve.</param>
    /// <returns>The quantity of the specified Soulstone.</returns>
    public int GetCurrencyAmount(StonesDataSO soulstone)
    {
        SoulstoneCache soulstoneCache = stones.Find(c => c.soulstoneData == soulstone);
        if (soulstoneCache.soulstoneData != null)
        {
            return soulstoneCache.quantity;
        }
        return 0;
    }

    /// <summary>
    /// Retrieves the current list of Soulstone currencies the player has.
    /// </summary>
    /// <returns>A list of <see cref="SoulstoneCache"/> representing the player's current Soulstone currencies.</returns>
    public List<SoulstoneCache> GetStones()
    {
        return stones;
    }

    #endregion

    #region Persistence
    /// <summary>
    /// Saves player data, including Soulstone currencies, to a persistent file.
    /// </summary>
    public void SavePlayerData()
    {
        PlayerData data = new PlayerData { stones = stones };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Player data saved at: {SaveFilePath}");
    }

    /// <summary>
    /// Loads player data, including Soulstone currencies, from a persistent file. Initializes default cache if the file doesn't exist.
    /// </summary>
    public void LoadPlayerData()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            stones = data.stones;
        }
        else
        {
            InitializeDefaultCache();
        }
    }

    /// <summary>
    /// Initializes default Soulstone currencies for the player.
    /// </summary>
    private void InitializeDefaultCache()
    {
        // Initialize default Soulstone cache
    }

    [System.Serializable]
    public class PlayerData
    {
        public List<SoulstoneCache> stones;
    }
    #endregion

    // Additional methods for gameplay logic...
}
