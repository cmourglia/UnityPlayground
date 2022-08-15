using UnityEngine;

public class DNA
{
    // Start is called before the first frame update
    public float[] genes;
    public float mutationRate = 0.05f;

    public DNA()
    {
        genes = new float[4];
        genes[0] = Random.Range(0f, 1f);
        genes[1] = Random.Range(0f, 1f);
        genes[2] = Random.Range(0f, 1f);
        genes[3] = Random.Range(0f, 1f);
    }

    public static DNA Cross(DNA a, DNA b)
    {
        DNA result = new();

        for (int i = 0; i < a.genes.Length; ++i)
        {
            float r = Random.Range(0f, 1f);

            if (r < 0.45f)
            {
                result.genes[i] = a.genes[i];
            }
            else if (r < 0.90f)
            {
                result.genes[i] = b.genes[i];
            }
            // else keep random value
        }

        return result;
    }

    public DNA Clone()
    {
        DNA result = new();

        for (int i = 0; i < genes.Length; ++i)
        {
            result.genes[i] = genes[i];
        }

        return result;
    }

    public void Mutate()
    {
        for (int i = 0; i < genes.Length; ++i)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                genes[i] += Random.Range(-0.1f, 0.1f);
            }
        }
    }
}
