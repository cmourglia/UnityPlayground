using System.Collections;
using UnityEngine;

public class SimulationHandler : MonoBehaviour
{
    [SerializeField] PopulationHandler populationHandler;
    [SerializeField] FoodSpawner foodSpawner;

    Coroutine _stepCoroutine;

    void Start()
    {
        Debug.Log("From simulation");
        populationHandler.simulationHandler = this;
        populationHandler.InitializePopulation();

        _stepCoroutine = StartCoroutine(Step());
    }

    IEnumerator Step()
    {
        foodSpawner.ResetWorld();
        populationHandler.StartGeneration();

        yield return new WaitForSeconds(60f);

        populationHandler.StopGeneration();
        populationHandler.NextGeneration();

        _stepCoroutine = StartCoroutine(Step());
    }

    public void OnPopulationEradication()
    {
        if (_stepCoroutine != null)
        {
            StopCoroutine(_stepCoroutine);

            populationHandler.StopGeneration();
            populationHandler.NextGeneration();
        }

        _stepCoroutine = StartCoroutine(Step());
    }
}
