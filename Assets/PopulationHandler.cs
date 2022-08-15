using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulationHandler : MonoBehaviour
{
    [SerializeField] Agent agentPrefab;
    [SerializeField] FoodSpawner foodSpawner;
    [SerializeField] Vector2 spawnBounds;

    [Header("GA Properties")]
    [SerializeField] int populationCount = 25;
    [SerializeField] int bestKept = 1;
    [SerializeField] int crossOverAmount = 5;

    public SimulationHandler simulationHandler { get; set; }

    Vector2 _halfSpawnBounds;
    Vector2 _halfForbidenBounds;

    Agent[] _population;
    int _activeAgentsCount;

    // Start is called before the first frame update
    void Awake()
    {
        _halfSpawnBounds = spawnBounds * 0.5f;
        _halfForbidenBounds = _halfSpawnBounds * 0.75f;
    }

    public void InitializePopulation()
    {
        _population = GenerateRandomPopulation();
    }

    public void NextGeneration()
    {
        SortPopulation();

        Debug.Log("Previous best: " + _population[0].lifetime);

        Agent[] nextGeneration = GenerateRandomPopulation();

        int childIndex = 0;
        for (int i = 0; i < bestKept; ++i, ++childIndex)
        {
            nextGeneration[childIndex].dna = _population[childIndex].dna.Clone();
        }

        for (int i = 0; i < crossOverAmount; ++i, ++childIndex)
        {
            DNA parentA = _population[Random.Range(0, populationCount / 2)].dna;
            DNA parentB = _population[Random.Range(0, populationCount / 2)].dna;

            nextGeneration[childIndex].dna = DNA.Cross(parentA, parentB);
        }

        foreach (Agent agent in nextGeneration)
        {
            agent.dna.Mutate();
        }

        foreach (Agent agent in _population)
        {
            Destroy(agent.gameObject);
        }

        _population = nextGeneration;
    }

    private Agent[] GenerateRandomPopulation()
    {
        Agent[] population = new Agent[populationCount];

        for (int i = 0; i < populationCount; i++)
        {
            Agent agent = Instantiate(agentPrefab, transform);

            agent.populationHandler = this;
            agent.foodSpawner = foodSpawner;
            agent.dna = new DNA();

            do
            {
                agent.transform.position = new Vector2(
                    Random.Range(-_halfSpawnBounds.x, _halfSpawnBounds.x),
                    Random.Range(-_halfSpawnBounds.y, _halfSpawnBounds.y));
            } while (
                (agent.transform.position.x > -_halfForbidenBounds.x) && (agent.transform.position.x < _halfForbidenBounds.x) &&
                (agent.transform.position.y > -_halfForbidenBounds.y) && (agent.transform.position.y < _halfForbidenBounds.y));

            population[i] = agent;
            agent.gameObject.SetActive(false);
        }

        return population;
    }

    public void StartGeneration()
    {
        foreach (Agent agent in _population)
        {
            agent.gameObject.SetActive(true);
        }

        _activeAgentsCount = _population.Length;
    }

    public void StopGeneration()
    {
        foreach (Agent agent in _population)
        {
            agent.gameObject.SetActive(false);
        }
    }

    public void SortPopulation()
    {
        _population = _population.OrderByDescending(a => a.lifetime).ToArray();
    }

    public void OnAgentDeath(Agent agent)
    {
        _activeAgentsCount -= 1;

        if (_activeAgentsCount <= 0)
        {
            simulationHandler.OnPopulationEradication();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, spawnBounds);
    }
}
