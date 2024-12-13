using UnityEngine;
using System.Collections.Generic;

public class FishBKSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] fishPrefabs;
    [SerializeField] private GameObject[] bubblePrefabs;
    [SerializeField] private GameObject[] jellyfishPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2.0f;
    [SerializeField] private float spawnRangeY = 5.0f;

    [Header("Speed Settings")]
    [SerializeField] private float fishSpeedMin = 2.0f;
    [SerializeField] private float fishSpeedMax = 5.0f;
    [SerializeField] private float bubbleSpeedMin = 0.5f; 
    [SerializeField] private float bubbleSpeedMax = 1.5f;

    [Header("Scale Settings")]
    [SerializeField] private float fishScaleMin = 0.5f;
    [SerializeField] private float fishScaleMax = 1.5f;
    [SerializeField] private float bubbleScaleMin = 0.3f;
    [SerializeField] private float bubbleScaleMax = 0.7f;
    [SerializeField] private float jellyfishScaleMin = 0.7f;
    [SerializeField] private float jellyfishScaleMax = 1.3f;

    [Header("Max Counts")]
    [SerializeField] private int maxFishCount = 10;
    [SerializeField] private int maxBubbleCount = 15;
    [SerializeField] private int maxJellyfishCount = 5;

    [Header("Jellyfish Speed Settings")]
    [SerializeField] private float jellyfishSpeedMin = 1.0f;
    [SerializeField] private float jellyfishSpeedMax = 3.0f;

    private float timer;

    private List<GameObject> activeFishes = new List<GameObject>();
    private List<GameObject> activeBubbles = new List<GameObject>();
    private List<GameObject> activeJellyfishes = new List<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (activeFishes.Count < maxFishCount)
                SpawnEntity(fishPrefabs, fishScaleMin, fishScaleMax, fishSpeedMin, fishSpeedMax, activeFishes, true, false);

            if (activeBubbles.Count < maxBubbleCount)
                SpawnEntity(bubblePrefabs, bubbleScaleMin, bubbleScaleMax, bubbleSpeedMin, bubbleSpeedMax, activeBubbles, false, false);

            if (activeJellyfishes.Count < maxJellyfishCount)
                SpawnEntity(jellyfishPrefabs, jellyfishScaleMin, jellyfishScaleMax, jellyfishSpeedMin, jellyfishSpeedMax, activeJellyfishes, false, true);

            timer = 0f;
        }

        CleanupInactiveEntities(activeFishes);
        CleanupInactiveEntities(activeBubbles);
        CleanupInactiveEntities(activeJellyfishes);
    }

    private void SpawnEntity(GameObject[] prefabs, float scaleMin, float scaleMax, float speedMin, float speedMax, List<GameObject> entityList, bool randomHorizontal, bool spawnFromBottom)
    {
        if (prefabs.Length == 0) return;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        float spawnX = spawnFromBottom ? Random.Range(-2f, 2f) : Random.Range(-2f, 2f); 
        float spawnY = spawnFromBottom ? -6f : Random.Range(-spawnRangeY, spawnRangeY);

        GameObject entity = Instantiate(prefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
        entityList.Add(entity);

        float randomScale = Random.Range(scaleMin, scaleMax);
        entity.transform.localScale = new Vector3(randomScale, randomScale, 1);

        Rigidbody2D rb = entity.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomSpeed = Random.Range(speedMin, speedMax);
            Vector2 direction = spawnFromBottom ? Vector2.up : new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.velocity = direction * randomSpeed;
        }
    }
    private void CleanupInactiveEntities(List<GameObject> entityList)
    {
        for (int i = entityList.Count - 1; i >= 0; i--)
        {
            if (entityList[i] == null || !entityList[i].activeInHierarchy ||
                entityList[i].transform.position.y > 6f) 
            {
                Destroy(entityList[i]); 
                entityList.RemoveAt(i);
            }
        }
    }


}
