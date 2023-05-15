using UnityEngine;

public class FieldSlot : MonoBehaviour
{
    public GameObject[] towerIndex;
    public GameObject installFX;
    public int towerToInstantiate;
    public bool isFilled;

    [Header("Sound FX")]
    public AudioClip installTower;
    public AudioSource reproduce;

    public void InstantiateInSlot()
    {
        Quaternion towerRotation = Quaternion.Euler(0, 90, 0);

        reproduce.PlayOneShot(installTower);
        Instantiate(towerIndex[towerToInstantiate], transform.position, towerRotation, gameObject.transform);
        Instantiate(installFX, transform.position, transform.rotation, gameObject.transform);
        isFilled = true;
    }
}
