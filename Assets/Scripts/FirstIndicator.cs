using UnityEngine;

public class FirstIndicator : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(-8.3f, 0, -7.7f);
    public Vector3 enemyPosition = new Vector3(-8.3f, 0, 7.7f);

    public void UpdateIndPos(bool isShowFase1)
    {
        transform.position = (isShowFase1 ? playerPosition : enemyPosition);
    }
}
