using UnityEngine;

public class FirstIndicator : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(-8.3f, 0, -7.7f);
    public Vector3 enemyPosition = new Vector3(-8.3f, 0, 7.7f);
    void Update()
    {
        if (GameManager.instance.showFase1)
        {
            transform.position = playerPosition;
        }
        else
        {
            transform.position = enemyPosition;
        }
    }
}
