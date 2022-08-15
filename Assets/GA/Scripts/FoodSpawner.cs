using System;
using System.Collections;
using System.Collections.Generic;
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
    Color _goodFoodColor;
    Color _badFoodColor;

    public List<Food> goodFood { get; private set; } = new();
    public List<Food> badFood { get; private set; } = new();

    Coroutine _spawnFoodCoroutine = null;

    void Awake()
    {
        Debug.Log("From FoodSpawner");

        _halfWorldSize = worldSize * 0.75f * 0.5f;
        _goodFoodColor = Color.green;// new Color(69f / 255f, 106f / 255f, 89f / 255f);
        _badFoodColor = Color.red; // new Color(255f / 255f, 99f / 255f, 71f / 255f);
    }

    public void ResetWorld()
    {
        if (_spawnFoodCoroutine != null)
        {
            StopCoroutine(_spawnFoodCoroutine);
        }

        foreach (Food food in goodFood)
        {
            Destroy(food.gameObject);
        }
        goodFood.Clear();

        foreach (Food food in badFood)
        {
            Destroy(food.gameObject);
        }
        badFood.Clear();

        for (int i = 0; i < initialFoodCount; i++)
        {
            SpawnFood();
        }

        _spawnFoodCoroutine = StartCoroutine(SpawnNextFood());
    }

    IEnumerator SpawnNextFood()
    {
        yield return new WaitForSeconds(spawnRate);

        SpawnFood();

        _spawnFoodCoroutine = StartCoroutine(SpawnNextFood());
    }

    void SpawnFood()
    {
        Food food = Instantiate(foodPrefab, transform).GetComponent<Food>();

        FoodType foodType = Random.Range(0f, 1f) < poisonRate
            ? FoodType.Poison
            : FoodType.Food;

        food.type = foodType;
        if (food.type == FoodType.Food)
        {
            food.GetComponent<SpriteRenderer>().color = _goodFoodColor;
            goodFood.Add(food);
        }
        else
        {
            food.GetComponent<SpriteRenderer>().color = _badFoodColor;
            badFood.Add(food);
        }

        food.transform.position = new Vector2(
            Random.Range(-_halfWorldSize.x, _halfWorldSize.x),
            Random.Range(-_halfWorldSize.y, _halfWorldSize.y));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, worldSize * 0.75f);
    }

    public float EatFood(Food food)
    {
        float healthIncrement;

        if (food.type == FoodType.Food)
        {
            healthIncrement = 1f;
            _ = goodFood.Remove(food);
        }
        else
        {
            healthIncrement = -5f;
            _ = badFood.Remove(food);
        }

        Destroy(food.gameObject);

        return healthIncrement;
    }
}
