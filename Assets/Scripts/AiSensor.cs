using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class AiSensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1;
    public LayerMask layers;
    public LayerMask occlusionlayers;
    public Material startmaterial;
    public Material CanSeePlayermaterial;
    public Transform startPoint;
    public bool iCanSeePlayer = false;
    private Mesh mesh;
    private Collider[] colliders = new Collider[50];
    private int count;

    private void Update()
    {
        Scan();
        if (iCanSeePlayer)
        {
            Graphics.DrawMesh(mesh, startPoint.position,  transform.rotation, CanSeePlayermaterial,0);
        }
        else
        {
            Graphics.DrawMesh(mesh, startPoint.position,  transform.rotation, startmaterial,0);
        }

    }

    private void Scan()
    {
        iCanSeePlayer = false;
        count = Physics.OverlapSphereNonAlloc(startPoint.position, distance, colliders, layers,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            if (!colliders[i].CompareTag("Player"))
            {
                return;
            }
            GameObject obj = colliders[i].gameObject;
            if (InSight(obj))
            {
                iCanSeePlayer = true;
            }
        }
    }

    private bool InSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 distancePlayer = dest - origin;
        
        if (distancePlayer.y > height)
        {
            return false;
        }
        
        distancePlayer.y = 0;
        float deltaAngle = Vector3.Angle(distancePlayer, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }
        
        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin,dest,occlusionlayers))
        {
            Debug.Log(Physics.Linecast(origin,dest,occlusionlayers));
            return false;
        }
        return true;
    }

    private Mesh CreatWidgetMesh()
    {
        Mesh creatWidgetMesh = new Mesh();

        int segments = 10;
        int numberTriangles = (segments * 4) + 4;
        int numberVertices = numberTriangles * 3;

        Vector3[] vertices = new Vector3[numberVertices];
        int[] triangles = new int[numberVertices];
        
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;
        
        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        
        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            
            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
        
            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            
            currentAngle += deltaAngle;
        }
        

        for (int i = 0; i < numberVertices; i++)
        {
            triangles[i] = i;
        }

        creatWidgetMesh.vertices = vertices;
        creatWidgetMesh.triangles = triangles;
        creatWidgetMesh.RecalculateNormals();
        
        return creatWidgetMesh;
    }

    private void OnValidate()
    {
        mesh = CreatWidgetMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawMesh(mesh,startPoint.position,transform.rotation);
        }
    }
}
