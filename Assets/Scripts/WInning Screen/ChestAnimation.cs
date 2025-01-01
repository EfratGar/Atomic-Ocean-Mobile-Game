using UnityEngine;

public class ChestLidSpriteSwitcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer lidRenderer; // SpriteRenderer for the lid
    [SerializeField] private Sprite closedLidSprite; // Sprite for the closed lid
    [SerializeField] private Sprite openLidSprite; // Sprite for the open lid
    [SerializeField] private GameObject coinParticlesPrefab; // Prefab for the coin particle effect
    [SerializeField] private float switchSpeed = 0.5f; // Time before switching the lid sprite
    [SerializeField] private int closedLidSortingOrder = 5; // Sorting order for closed lid
    [SerializeField] private int openLidSortingOrder = 3; // Sorting order for open lid
    [SerializeField] private Transform closedLid;
    [SerializeField] private Transform openLid;

    private void Start()
    {
        // Start the sequence to open the chest
        Invoke(nameof(SwitchToOpenLid), switchSpeed);
    }

    private void SwitchToOpenLid()
    {
        if (lidRenderer != null && openLidSprite != null)
        {
            // Switch the lid to the open sprite
            lidRenderer.sprite = openLidSprite;

            // Update sorting order to ensure proper layering
            lidRenderer.sortingOrder = openLidSortingOrder;

            // Adjust the position and rotation of the open lid
            if (openLid != null)
            {
                lidRenderer.transform.localPosition = openLid.localPosition;
                lidRenderer.transform.localRotation = openLid.localRotation;
            }

        }
        else
        {
            Debug.LogError("LidRenderer or OpenLidSprite is not assigned!");
        }
    }

}
