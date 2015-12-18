using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {
    //Declaring the variables and arrays
    public Mesh[] meshes;
    public Material material;
    public int depth;
    public int maxDepth = 4;
    public float scale;
    public float spawnChance;
    public float maxSpeed;
    private float rotSpeed;
    public float maxTwist;
    public float justTime;

    private static Vector3[] spawnDirection =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };
    
    private static Quaternion[] spawnOrientation =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, -90f)
    };

    private Material[,] colors;
    
    //Initializing materials to allow for dynamic batching
    private void InitializeColors ()
    {
        colors = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float lc = i / (maxDepth - 1f);
            lc *= lc;
            colors[i, 0] = new Material(material);
            colors[i, 0].color = 
                Color.Lerp(Color.red, Color.white, (float)i / maxDepth);

            colors[i, 1] = new Material(material);
            colors[i, 1].color =
                Color.Lerp(Color.cyan, Color.white, (float)i / maxDepth);
        }
        colors[maxDepth, 0].color = Color.magenta;
        colors[maxDepth, 1].color = Color.blue;

    }

    // Use this for initialization
    void Start ()
    {
        rotSpeed = Random.Range(-maxSpeed, maxSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        if (colors == null)
        {
            InitializeColors();
        }
        gameObject.AddComponent<MeshFilter>().mesh = 
            meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = colors[depth, Random.Range(0,2)];
        //creating children
        //depth used to prevent the fracral from growing infinitely
        if (depth < maxDepth)
        {
            StartCoroutine(ChildCreate());
        }
    }

    private IEnumerator ChildCreate ()
    {
        for (int i = 0; i < spawnDirection.Length; i++)
        {
            if (Random.value < spawnChance)
            {
                yield return new WaitForSeconds(justTime);
                new GameObject("child").AddComponent<Fractal>().
                    Initialization(this, i);
            }
        }
    }

    private void Initialization (Fractal parent, int spawnIndex)
    {
        meshes = parent.meshes;
        colors = parent.colors;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        scale = parent.scale;
        spawnChance = parent.spawnChance;
        maxSpeed = parent.maxSpeed;
        maxTwist = parent.maxTwist;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * scale;
        transform.localPosition = spawnDirection[spawnIndex] * (0.5f + 0.5f * scale);
        transform.localRotation = spawnOrientation[spawnIndex];
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
	}
}
