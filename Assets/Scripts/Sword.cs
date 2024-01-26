using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Vector3 endPos;

    [SerializeField] float lerpInclination = 3f;
    private void Awake()
    {
        ShowTurnAction.OnEn_positionLoc += GetEndPos;
        endPos = transform.position;
    }

    private void GetEndPos(Vector3 pos)
    {
        endPos = pos;
    }

    private void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        Vector3 smoothPos = Vector3.Lerp(currentPos, endPos, lerpInclination * Time.deltaTime);
        transform.position = smoothPos;
    }

    private void OnDestroy()
    {
        ShowTurnAction.OnEn_positionLoc -= GetEndPos;
    }
}
