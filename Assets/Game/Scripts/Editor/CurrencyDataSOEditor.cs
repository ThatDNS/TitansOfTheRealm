using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for CurrencyDataSO, providing enhanced UI features in the Inspector.
/// </summary>
[CustomEditor(typeof(StonesDataSO))]
public class CurrencyDataSOEditor : Editor
{
    private StonesDataSO currency;
    private float imageSize = 100f; // Default image size

    /// <summary>
    /// Called when the script is loaded or the object is selected.
    /// Retrieves the target object.
    /// </summary>
    private void OnEnable()
    {
        currency = target as StonesDataSO;
    }

    /// <summary>
    /// Overrides the Inspector GUI to add custom features.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draw the default inspector items
        base.OnInspectorGUI();

        // Exit if the currency sprite is not set
        if (currency.stoneSprite == null) return;

        // Get a preview texture of the currency sprite
        Texture2D texture = AssetPreview.GetAssetPreview(currency.stoneSprite);
        if (texture == null)
        {
            EditorGUILayout.HelpBox("Preview not available", MessageType.Info);
            return;
        }

        // Slider to adjust the preview image size
        imageSize = EditorGUILayout.Slider("Preview Size", imageSize, 25f, 200f); // Slider range: 25 to 200

        // Calculate aspect ratio of the texture
        float aspectRatio = (float)texture.width / texture.height;

        // Compute and display the preview image
        float imageWidth = imageSize * aspectRatio;
        GUILayout.Label("", GUILayout.Height(imageSize), GUILayout.Width(imageWidth));
        Rect lastRect = GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(lastRect, texture, ScaleMode.ScaleToFit);
    }
}