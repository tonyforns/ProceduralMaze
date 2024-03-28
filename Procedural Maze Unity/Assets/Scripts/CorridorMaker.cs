using System.Collections.Generic;
using UnityEngine;
using static Dungeon;

public class CorridorMaker : DungeonSegmentMaker
{
    [SerializeField] private List<DungeonSegmentDef> corridorDefList;
    [SerializeField] private DungeonSegmentDef door;
    private Dictionary<string, DungeonSegmentDef> corridorDefDictionary;

    public CorridorMaker(byte corridorId, DungeonSegmentStruct[,] mapDef, Vector2Int size, int scale, byte[,] map, List<DungeonSegmentDef> corridorDefList, DungeonSegmentDef door) : base(corridorId, mapDef, size, scale)
    {
        this.map = map;
        this.corridorDefList = corridorDefList;
        this.door = door;
        CreateDictionary();
    }
    private void CreateDictionary()
    {
        corridorDefDictionary = new Dictionary<string, DungeonSegmentDef>();
        foreach (DungeonSegmentDef def in corridorDefList)
        {
            corridorDefDictionary.Add(def.id, def);
        }
    }

    public void Generate()
    {
        Generate(Random.Range(1, size.x), Random.Range(1, size.y));
    }

    private void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z, segmentId) >= 2) return;

        map[x, z] = segmentId;

        direcctions.Shuffle();

        Generate((x + direcctions[0].x), z + direcctions[0].y);
        Generate((x + direcctions[1].x), z + direcctions[1].y);
        Generate((x + direcctions[2].x), z + direcctions[2].y);
        Generate((x + direcctions[3].x), z + direcctions[3].y);

    }

    public void Draw()
    {
        int rotationAmount = 0;
        DungeonSegmentDef corridorDef;
        int[,] id = new int[3, 3];
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (IsMapElementA(x, z, segmentId))
                {
                    rotationAmount = 0;
                    id[1, 0] = map[x, z - 1];
                    id[0, 1] = map[x - 1, z];
                    id[1, 1] = map[x, z];
                    id[2, 1] = map[x + 1, z];
                    id[1, 2] = map[x, z + 1];
                    string idString = $"{id[0, 0]}{id[1, 0]}{id[2, 0]}" +
                                      $"{id[0, 1]}{id[1, 1]}{id[2, 1]}" +
                                      $"{id[0, 2]}{id[1, 2]}{id[2, 2]}";

                    if (id[1, 0] == 2)
                    {
                        Instantiate(x, z, door, 180, idString);
                        id[1, 0] = 1;
                    }
                    if (id[0, 1] == 2)
                    {
                        Instantiate(x, z, door, -90);
                        
                        id[0, 1] = 1;

                    }
                    if (id[2, 1] == 2)
                    {
                        Instantiate(x, z, door, 90);
                        id[2, 1] = 1;
                    }
                    if (id[1, 2] == 2)
                    {
                        Instantiate(x, z, door, 0);
                        id[1, 2] = 1;
                    }


                    while (!corridorDefDictionary.TryGetValue(idString, out corridorDef) && rotationAmount < 4)
                    {
                        rotationAmount++;
                        int aux = id[1, 0];
                        id[1, 0] = id[2, 1];
                        id[2, 1] = id[1, 2];
                        id[1, 2] = id[0, 1];
                        id[0, 1] = aux;
                        idString = $"{id[0, 0]}{id[1, 0]}{id[2, 0]}" +
                                   $"{id[0, 1]}{id[1, 1]}{id[2, 1]}" +
                                   $"{id[0, 2]}{id[1, 2]}{id[2, 2]}";
                    }
                    if (corridorDef != null)
                    {
                        Instantiate(x, z, corridorDef, -90 * rotationAmount);
                    }
                    else
                    {
                        Debug.LogError("Corridor match didnt find. Id:  " + idString);
                    }
                }
            }
        }
    }
}
