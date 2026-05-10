using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(LineRenderer))]
public class LineCollider : MonoBehaviour
{
    public LineRenderer  lineRenderer;
    public BoxCollider   boxCollider;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider  = GetComponent<BoxCollider>();
    }

    void Update()
    {
        float   LineLength   = Vector3.Distance( lineRenderer.GetPosition(1), lineRenderer.GetPosition(0) );
        Vector3 CenterOfLine = ( lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0) ) / 2f;

        //Scale Collider
        boxCollider.center = CenterOfLine;
        boxCollider.size   = new Vector3(LineLength, 0.1f, 0.1f);
    }
}