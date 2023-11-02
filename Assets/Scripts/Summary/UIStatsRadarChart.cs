using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatsRadarChart : MonoBehaviour
{
    [SerializeField] private Material radarMaterial;

    private CanvasRenderer radarMeshCanvasRenderer;

    Pokemon pokemon;
    private void Awake()
    {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
    }

    public void Setup(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        Vector2[] uv = new Vector2[7];
        int[] triangles = new int[3 * 6];

        float angleIncrement = 360f / 6;
        float radarChartSize = 185f;

        Vector3 healthVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * pokemon.GetHPNormalized();
        int healthVertexIndex = 1;
        
        Vector3 attackVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * pokemon.GetStatNormalized(Stat.Attack);
        int attackVertexIndex = 2;
        
        Vector3 defenseVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * pokemon.GetStatNormalized(Stat.Defense);
        int defenseVertexIndex = 3;

        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * pokemon.GetStatNormalized(Stat.Speed);
        int speedVertexIndex = 4;

        Vector3 spDefenseVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * pokemon.GetStatNormalized(Stat.SpDefense);
        int spDefenseVertexIndex = 5;

        Vector3 spAttackVertex = Quaternion.Euler(0, 0, -angleIncrement * 5) * Vector3.up * radarChartSize * pokemon.GetStatNormalized(Stat.SpAttack);
        int spAttackVertexIndex = 6;

        vertices[0] = Vector3.zero;
        vertices[healthVertexIndex] = healthVertex;
        vertices[attackVertexIndex] = attackVertex;
        vertices[defenseVertexIndex] = defenseVertex;
        vertices[speedVertexIndex] = speedVertex;
        vertices[spDefenseVertexIndex] = spDefenseVertex;
        vertices[spAttackVertexIndex] = spAttackVertex;

        uv[0] = Vector2.zero;
        uv[healthVertexIndex] = Vector2.one;
        uv[attackVertexIndex] = Vector2.one;
        uv[defenseVertexIndex] = Vector2.one;
        uv[speedVertexIndex] = Vector2.one;
        uv[spDefenseVertexIndex] = Vector2.one;
        uv[spAttackVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = healthVertexIndex;
        triangles[2] = attackVertexIndex;

        triangles[3] = 0;
        triangles[4] = attackVertexIndex;
        triangles[5] = defenseVertexIndex;
        
        triangles[6] = 0;
        triangles[7] = defenseVertexIndex;
        triangles[8] = speedVertexIndex;

        triangles[9] = 0;
        triangles[10] = speedVertexIndex;
        triangles[11] = spDefenseVertexIndex;

        triangles[12] = 0;
        triangles[13] = spDefenseVertexIndex;
        triangles[14] = spAttackVertexIndex;

        triangles[15] = 0;
        triangles[16] = spAttackVertexIndex;
        triangles[17] = healthVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, null);
    }
}
