using UnityEngine;

public class FieldSlot : MonoBehaviour
{
    public GameObject[] towerIndex;
    [SerializeField] private GameObject installFX;
    [SerializeField] private GameObject redSquare;
    public int towerToInstantiate;
    public bool isFilled;
    public bool isEnemy;

    [Header("Sound FX")]
    public AudioClip installTower;
    [SerializeField] private AudioSource reproduce;

    public void InstantiateInSlot()
    {
        //Quaternion towerRotation = Quaternion.Euler(0, 90, 0);

        reproduce.PlayOneShot(installTower);
        Instantiate(towerIndex[towerToInstantiate], transform.position, transform.rotation, gameObject.transform);
        Instantiate(installFX, transform.position, transform.rotation, gameObject.transform);
        isFilled = true;
    }

    public void FailedToInstall()
    {
        Instantiate(redSquare, gameObject.transform);
    }
}
