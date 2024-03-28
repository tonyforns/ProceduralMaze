using System.Collections.Generic;
using UnityEngine;
using static Dungeon;

public class RoomMaker : DungeonSegmentMaker
{

    [SerializeField] private List<DungeonSegmentDef> roomDefList;
    private Dictionary<string, DungeonSegmentDef> roomDefDictionary = new Dictionary<string, DungeonSegmentDef>();
    private List<DungeonSegmentDef> collumnDefList;
    private Dictionary<string, DungeonSegmentDef> collumnDefDictionary = new Dictionary<string, DungeonSegmentDef>();


    public RoomMaker(byte roomId, DungeonSegmentStruct[,] mapDef, Vector2Int size, int scale, byte[,] map, List<DungeonSegmentDef> roomDefList, List<DungeonSegmentDef> collumnDefList) : base(roomId, mapDef, size, scale)
    {
        this.map = map;
        this.roomDefList = roomDefList;
        this.collumnDefList = collumnDefList;
        CreateDictionary();
    }

    private void CreateDictionary()
    {
        foreach (DungeonSegmentDef def in roomDefList)
        {
            roomDefDictionary.Add(def.id, def);
        }

        foreach (DungeonSegmentDef def in collumnDefList)
        {
            collumnDefDictionary.Add(def.id, def);
        }
    }

    public void Generate(int amoutRooms, int maxSize, int minSize)
    {
        for (int i = 0; i < amoutRooms; i++)
        {
            int xStart = Random.Range(2, size.x - maxSize);
            int zStart = Random.Range(2, size.y - maxSize);
            int xSize = Random.Range(minSize, maxSize);
            int zSize = Random.Range(minSize, maxSize);

            for (int x = 0; x < xSize && xStart + x < size.x - 1; x++)
            {
                for (int z = 0; z < zSize && zStart + z < size.y - 1; z++)
                {
                    SetMapValue(xStart + x, zStart + z, segmentId);
                }
            }
        }

        RecalulateRooms();
    }

    public void RecalulateRooms()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (IsMapElementA(x, z, segmentId) && CountSquareNeighbours(x, z, segmentId) > 0)
                {
                    RecalculateRooms(x + 1, z);
                    RecalculateRooms(x - 1, z);
                    RecalculateRooms(x, z + 1);
                    RecalculateRooms(x, z - 1);
                }
            }
        }
    }
    public void RecalculateRooms(int x, int z)
    {
        if (IsMapElementA(x, z, segmentId) || map[x, z] == 0) return;

        if (CountSquareNeighbours(x, z, segmentId) != 1 || CountAllNeighbours(x, z, segmentId) > 2)
        {
            SetMapValue(x, z, segmentId);

            RecalculateRooms(x + 1, z);
            RecalculateRooms(x - 1, z);
            RecalculateRooms(x, z + 1);
            RecalculateRooms(x, z - 1);
        }
    }

    private void DrawCollumns() {
        int rotationAmount = 0;
        DungeonSegmentDef collumnDef;
        int[,] id = new int[3, 3];
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (IsMapElementA(x, z, segmentId))
                {
                    rotationAmount = 0;

                    id[0, 0] = map[x - 1, z - 1];
                    id[1, 0] = map[x, z - 1];
                    id[2, 0] = map[x + 1, z - 1];

                    id[0, 1] = map[x - 1, z];
                    id[1, 1] = map[x, z];
                    id[2, 1] = map[x + 1, z];

                    id[0, 2] = map[x - 1, z + 1];
                    id[1, 2] = map[x, z + 1];
                    id[2, 2] = map[x + 1, z + 1];

                    string idString = $"{id[0, 0]}{id[1, 0]}{id[2, 0]}" +
                                      $"{id[0, 1]}{id[1, 1]}{id[2, 1]}" +
                                      $"{id[0, 2]}{id[1, 2]}{id[2, 2]}";



                    while (!collumnDefDictionary.TryGetValue(idString, out collumnDef) && rotationAmount < 4)
                    {
                        rotationAmount++;
                        int aux1 = id[0, 0];
                        int aux2 = id[1, 0];

                        id[0, 0] = id[2, 0];
                        id[1, 0] = id[2, 1];

                        id[2, 0] = id[2, 2];
                        id[2, 1] = id[1, 2];

                        id[2, 2] = id[0, 2];
                        id[1, 2] = id[0, 1];

                        id[0, 2] = aux1;
                        id[0, 1] = aux2;
                        idString = $"{id[0, 0]}{id[1, 0]}{id[2, 0]}" +
                                   $"{id[0, 1]}{id[1, 1]}{id[2, 1]}" +
                                   $"{id[0, 2]}{id[1, 2]}{id[2, 2]}";

                    }
                    if (collumnDef != null)
                    {
                        Instantiate(x, z, collumnDef, -90 * rotationAmount, collumnDef.name  + " " + GetCellId(x, z) + "idFound: " + idString + " rotation: " + (-90 * rotationAmount));
                    }
                    else
                    {
                    }
                }
            }
        }
    }
    public void Draw()
    {
        int rotationAmount = 0;
        DungeonSegmentDef roomDef;
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
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {

                            id[i, j] = id[i, j] == 1 ? 2 : id[i, j];
                        }
                    }

                    string idString = $"{id[0, 0]}{id[1, 0]}{id[2, 0]}" +
                                      $"{id[0, 1]}{id[1, 1]}{id[2, 1]}" +
                                      $"{id[0, 2]}{id[1, 2]}{id[2, 2]}";



                    while (!roomDefDictionary.TryGetValue(idString, out roomDef) && rotationAmount < 4)
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
                    if (roomDef != null)
                    {
                        Instantiate(x, z, roomDef, -90 * rotationAmount, roomDef.name + " " + GetCellId(x, z) + "idFound: " + idString + " rotarion " + (-90 * rotationAmount));
                    }
                    else
                    {
                        Debug.LogError("Room match didnt find. Id:  " + idString);
                    }
                }
            }
        }

        DrawCollumns();
    }

}
