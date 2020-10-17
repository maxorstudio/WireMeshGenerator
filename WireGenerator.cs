using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGenerator : MonoBehaviour
{
    public GameObject debugObject;
    public GameObject wireSnap1;
    public GameObject wireSnap2;

    [Range(0.1f, 1.0f)]
    public float wireRadius;

    [Range(3, 32)]
    public int wireLOD;

    Mesh mesh;

    private int vertexCount => wireLOD * 2;
    public bool showGizmos;
    

    void OnDrawGizmos() {
        if (showGizmos) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wireSnap1.transform.position, 0.015f);
            Gizmosfs.DrawWireVert(wireSnap1.transform.position, transform.rotation, wireRadius, wireLOD);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(wireSnap2.transform.position, 0.015f);
            Gizmosfs.DrawWireVert(wireSnap2.transform.position, transform.rotation, wireRadius, wireLOD);
        }
    }

    void Awake() {
        mesh = new Mesh();
        mesh.name = "WireMesh";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // void Start() => GenerateWireMesh(wireSnap1.transform.position, wireSnap2.transform.position, transform.rotation);
    void Start() => TestMesh();

    void TestMesh() {
        //
    }

    void GenerateWireMesh(Vector3 snap1, Vector3 snap2, Quaternion rot) {
        mesh.Clear();

        // index vertices
        int vCount = vertexCount;
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < wireLOD; i++) {
            float t = i / (float)wireLOD;
            float angRad = t * Mathfs.TAU;

            Vector2 points2D = Mathfs.GetVectorByAngle(angRad) * wireRadius;
            
            vertices.Add(snap1 + rot * points2D);
            vertices.Add(snap2 + rot * points2D);
        }

        // create mesh triangles
        List<int> triangles = new List<int>();
        for (int i = 0; i < wireLOD; i++)
        {
            int indexRoot = i * 2;

            int snap1Next = indexRoot + 2;
            int snap2Next = indexRoot + 3 % vCount;
            int snap2Root = indexRoot + 1 % vCount;


            triangles.Add(indexRoot);
            triangles.Add(snap1Next);
            triangles.Add(snap2Next); 

            triangles.Add(indexRoot);
            triangles.Add(snap2Next);
            triangles.Add(snap2Root); 
        }

        // add vertices && triangles to mesh then recalculate
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        // instantiate tiny sphere to debug (view_vertices)
        for (int i = 0; i < vertices.Count; i++) {
            GameObject _currentVertices;
            _currentVertices = Instantiate(debugObject, vertices[i], transform.rotation);
            _currentVertices.name = i.ToString();
        }
    }
}
