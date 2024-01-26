using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] float interpolationTimeCount = 1; // Number of frames to completely interpolate between the 2 positions
    float elapsedTime = 0;

    private void Awake()
    {
        ShowTurnAction.OnEn_positionLoc += GetEndPos;
        startPos = transform.position;
        endPos = transform.position;
    }

    private void GetEndPos(Vector3 pos)
    {
        endPos = pos;
        transform.LookAt(endPos);
    }

    private void Update()
    {
        float interpolationRatio = elapsedTime / interpolationTimeCount;

        transform.position = Vector3.Lerp(startPos, endPos, interpolationRatio);

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interpolationTimeCount)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ShowTurnAction.OnEn_positionLoc -= GetEndPos;
    }
}
