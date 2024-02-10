using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for the Player class, enhancing the Inspector interface.
/// </summary>
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    /// <summary>
    /// Overrides the Inspector GUI to add custom layout for the Player class.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draw the default inspector items
        base.OnInspectorGUI();

        // Get the Player target
        Player player = (Player)target;

        // Exit if the player has no currencies assigned
        if (player.GetStones() == null || player.GetStones().Count == 0)
            return;

        // Iterate over each currency and create a GUI layout for it
        foreach (var currency in player.GetStones())
        {
            if (currency.soulstoneData != null)
            {
                EditorGUILayout.BeginHorizontal();

                // Display the currency sprite
                if (currency.soulstoneData.stoneSprite != null)
                {
                    GUILayout.Label(AssetPreview.GetAssetPreview(currency.soulstoneData.stoneSprite), GUILayout.Width(50), GUILayout.Height(50));
                }

                EditorGUILayout.BeginVertical();

                // Display the currency name and quantity
                EditorGUILayout.LabelField(currency.soulstoneData.stoneName);
                EditorGUILayout.LabelField("Quantity: " + currency.quantity.ToString());

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}