using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gizmosfs {
    
    public static void DrawWireVert(Vector3 pos, Quaternion rot, float radius, int lod) {
        Vector3[] points3D = new Vector3[lod];
        for (int i = 0; i < points3D.Length; i++) {
            float t = i / (float)lod;
            float angRad = t * Mathfs.TAU;

            Vector2 point2D = Mathfs.GetVectorByAngle(angRad) * radius;

            points3D[i] = pos + rot * point2D;
        }

        for (int i = 0; i < points3D.Length-1; i++) {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(points3D[i], 0.02f);
            Gizmos.DrawLine(points3D[i], points3D[i + 1]);
        }
        Gizmos.DrawSphere(points3D[lod-1], 0.02f);
        Gizmos.DrawLine(points3D[lod-1], points3D[0]);

        // CreateWireMesh();
    }

}
