using System.Collections.Generic;
using UnityEngine;
using static Dungeon;

public abstract class  DungeonSegmentMaker
{

    internal List<Vector2Int> direcctions = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

    [SerializeField] internal byte segmentId;
    [SerializeField] internal Vector2Int size = new Vector2Int(30, 30);
    [SerializeField] internal byte[,] map;
    [SerializeField] internal DungeonSegmentStruct[,] mapDef;
    [SerializeField] internal int scale = 6;

    public DungeonSegmentMaker(byte segmentId, DungeonSegmentStruct[,] mapDef, Vector2Int size, int scale)
    {
        this.segmentId = segmentId;
        this.mapDef = mapDef;
        this.size = size;
        this.scale = scale;
    }

    public int CountDiagonalNeighbours(int x, int z, byte plane)
    {
        int count = 0;
        if (x <= 0 || x >= (size.x - 1) || z <= 0 || z >= size.y - 1) return 5;
        if (map[x - 1, z - 1] == plane) count++;
        if (map[x + 1, z + 1] == plane) count++;
        if (map[x + 1, z - 1] == plane) count++;
        if (map[x - 1, z + 1] == plane) count++;
        return count;
    }

    public int CountSquareNeighbours(int x, int z, byte plane)
    {
        int count = 0;
        if (x <= 0 || x >= (size.x - 1) || z <= 0 || z >= size.y - 1) return 5;
        if (map[x - 1, z] == plane) count++;
        if (map[x + 1, z] == plane) count++;
        if (map[x, z - 1] == plane) count++;
        if (map[x, z + 1] == plane) count++;
        return count;
    }

    public int CountAllNeighbours(int x, int z, byte plane)
    {

        return CountSquareNeighbours(x, z, plane) + CountDiagonalNeighbours(x, z, plane);
    }

    internal bool IsIndexValid(int x, int z)
    {
        return (x > 0 && x < size.x && z > 0 && z < size.y);
    }

    internal void SetMapValue(int x, int z, byte plane)
    {
        map[x, z] = plane;
    }

    internal bool IsMapElementA(int x, int z, int element)
    {
        return map[x, z] == element;
    }

    internal void Instantiate(int x, int z, DungeonSegmentDef segment, int eulerRotation, string name = null)
    {
        Transform segmentTransform = GameObject.Instantiate(segment.segment);
        segmentTransform.position = new Vector3(x * scale, 0, z * scale);
        //corridorTransform.localScale *= scale;
        segmentTransform.rotation = Quaternion.Euler(0, eulerRotation, 0);
        segmentTransform.name = name == null ? segmentTransform.name : name;

        mapDef[x, z].dungeonSegmentDef = segment;
        mapDef[x, z].id = map[x, z];
        mapDef[x, z].idString = GetCellId(x, z);
        mapDef[x, z].rotation = eulerRotation;
        mapDef[x, z].segmentInScene = segmentTransform;
    }

    internal string GetCellId(int x, int z)
    {
        return $"{map[x -1, z -1]}{map[x, z - 1]}{map[x + 1, z - 1]} " +
               $"{map[x - 1, z]}{map[x, z]}{map[x + 1, z]} " +
               $"{map[x - 1, z + 1]}{map[x, z + 1]}{map[x + 1, z + 1]}";
    }
    
}
