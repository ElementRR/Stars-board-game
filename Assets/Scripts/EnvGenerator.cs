using UnityEngine;

public class EnvGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] fieldPrefabs;

    [SerializeField] private GameObject[] soilPrefabs;
    [SerializeField] private Vector3 soilPosition;
    [SerializeField] private Vector3 soilScale;

    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private Vector3 treePosition;
    [SerializeField] private Vector3 treeScale;
    [SerializeField] private Vector3 treePosition1;
    [SerializeField] private Vector3 tree1Scale;

    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private Vector3 rockPosition;
    [SerializeField] private Vector3 rockScale;
    [SerializeField] private Vector3 rockPosition1;
    [SerializeField] private Vector3 rock1Scale;

    [SerializeField] private GameObject[] artifactPrefabs;
    [SerializeField] private Vector3 artifactPosition;

    [SerializeField] private GameObject[] whetherPrefabs;
    [SerializeField] private Vector3 whetherPosition;

    public int artifactChance = 10;  // must be in %

    void Awake()
    {
        Quaternion rockRotation = Quaternion.Euler(0, -134, 0);
        Quaternion treeRotation = Quaternion.Euler(0, -30, 0);
        Quaternion artifactRotation = Quaternion.Euler(-90, 0, -39);
        Quaternion whetherRotation = Quaternion.Euler(285, 90, 270);

        Instantiate(fieldPrefabs[Random.Range(0, fieldPrefabs.Length)]);

        GameObject soil = Instantiate(soilPrefabs[Random.Range(0, soilPrefabs.Length)], soilPosition, transform.rotation, transform);
        soil.transform.localScale = soilScale;

        GameObject tree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], treePosition, treeRotation, transform);
        tree.transform.localScale = treeScale;
        GameObject tree1 = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], treePosition1, transform.rotation, transform);
        tree1.transform.localScale = tree1Scale;

        GameObject rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], rockPosition, transform.rotation, transform);
        rock.transform.localScale = rockScale;
        GameObject rock1 = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], rockPosition1, rockRotation, transform);
        rock1.transform.localScale = rock1Scale;

        int randNumber = Random.Range(0, 101);

        if (randNumber <= artifactChance)
        {
            Instantiate(artifactPrefabs[Random.Range(0, artifactPrefabs.Length)], artifactPosition, artifactRotation, transform);
            Instantiate(whetherPrefabs[Random.Range(0, whetherPrefabs.Length)], whetherPosition, whetherRotation, transform);
        }
    }
}
