using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    bool isActive = false;
    [SerializeField] float interpolationTimeCount = 1; // Number of frames to completely interpolate between the 2 positions
    float elapsedTime = 0;

    [SerializeField] private MeshRenderer mesh;
    private Material mat;
    [SerializeField] private float appearSpeed = 20f;

    private void Awake()
    {
        ShowTurnAction.OnEn_positionLoc += GetEndPos;
        startPos = transform.position;
        endPos = transform.position;
        mat = mesh.material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0f);
    }

    private void GetEndPos(Vector3 pos)
    {
        endPos = pos;
        transform.LookAt(endPos);
        isActive = true;
        elapsedTime = 0;
    }

    private void FixedUpdate()
    {
        Color currentColor = mat.color;
        Color smoothColor = new(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, 1, appearSpeed * Time.deltaTime));
        mat.color = smoothColor;

        if(isActive)
        {
            float interpolationRatio = elapsedTime / interpolationTimeCount;

            interpolationRatio = interpolationRatio * interpolationRatio * (3f - 2f * interpolationRatio);

            transform.position = Vector3.Lerp(startPos, endPos, interpolationRatio);

            if (elapsedTime >= interpolationTimeCount)
            {
                Destroy(gameObject);
            }
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interpolationTimeCount + 0.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ShowTurnAction.OnEn_positionLoc -= GetEndPos;
    }
}
