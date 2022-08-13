using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] int initialFoodCount = 100;
    [SerializeField] float spawnRate = 1f;
    [SerializeField, Range(0f, 1f)] float poisonRate = 0.3f;
    [SerializeField] Vector2 worldSize;

    [SerializeField] GameObject foodPrefab;

    Vector2 _halfWorldSize;

    void Start()
    {
        _halfWorldSize = worldSize * 0.5f;

        for (int i = 0; i < initialFoodCount; i++)
        {
            SpawnFood();
        }

        _ = StartCoroutine(SpawnNextFood());
    }

    IEnumerator SpawnNextFood()
    {
        yield return new WaitForSeconds(spawnRate);

        SpawnFood();

        _ = StartCoroutine(SpawnNextFood());
    }

    void SpawnFood()
    {
        Food food = Instantiate(foodPrefab).GetComponent<Food>();

        FoodType foodType = Random.Range(0f, 1f) < poisonRate
            ? FoodType.Poison
            : FoodType.Food;

        food.type = foodType;
        food.GetComponent<SpriteRenderer>().color = foodType == FoodType.Poison ? Color.red : Color.green;

        food.transform.position = new Vector2(
            Random.Range(-_halfWorldSize.x, _halfWorldSize.x),
            Random.Range(-_halfWorldSize.y, _halfWorldSize.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, worldSize);
    }
}
