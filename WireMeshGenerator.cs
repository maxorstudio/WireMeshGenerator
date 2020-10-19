using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WireMeshGenerator {
    
    private static Mesh mesh = new Mesh();
    private const float TAU = 6.28318530718f;

    private static Vector2 GetVectorByAngle(float _angRad) {
        return new Vector2(
            Mathf.Sin(_angRad),
            Mathf.Cos(_angRad)
        );
    }

    public static Mesh GenerateWireMesh(List<GameObject> _wirePoints, float _wireRadius, int _wireLOD) {

        int _vertexCount = _wireLOD * 2;

        List<Vector3> _vertices = new List<Vector3>();
        for (int i = 0; i < _wireLOD; i++) {
            float t = i / (float)_wireLOD;
            float _angRad = t * TAU;

            Vector2 _vectorPoint = GetVectorByAngle(_angRad) * _wireRadius;

            _vertices.Add(_wirePoints[0].transform.position + _wirePoints[0].transform.rotation * _vectorPoint);
            _vertices.Add(_wirePoints[1].transform.position + _wirePoints[1].transform.rotation * _vectorPoint);
        }

        List<int> _triangles = new List<int>();
        for (int i = 0; i < _wireLOD; i++) {
            int indexRoot = i * 2;
            _triangles.Add(indexRoot % _vertexCount);
            _triangles.Add((indexRoot + 3) % _vertexCount);
            _triangles.Add((indexRoot + 1) % _vertexCount);

            _triangles.Add(indexRoot % _vertexCount);
            _triangles.Add((indexRoot + 2) % _vertexCount);
            _triangles.Add((indexRoot + 3) % _vertexCount);
        }

        mesh.Clear();
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;

    }
}