using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Vector3 LocalCenterOfMass { get; private set; } = new Vector3 (0, 95, 0);
    [field: SerializeField] public float GizmosLength { get; private set; } = 1;

    void Start()
    {
        Rigidbody.centerOfMass = LocalCenterOfMass;
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Vector3 point = Rigidbody.position + Rigidbody.rotation * Rigidbody.centerOfMass;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(point + Vector3.left * GizmosLength, point + Vector3.right * GizmosLength);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point + Vector3.down * GizmosLength, point + Vector3.up * GizmosLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point + Vector3.back * GizmosLength, point + Vector3.forward * GizmosLength);
    }

    private void Reset()
    {
        if (Rigidbody == null)
            Rigidbody = GetComponentInChildren<Rigidbody>();
    }
}