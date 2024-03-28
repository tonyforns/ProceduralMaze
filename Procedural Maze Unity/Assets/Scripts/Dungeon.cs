using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Dungeon : Singleton<Dungeon>
{
    [SerializeField] private Transform character;

    internal List<Vector2Int> direcctions = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

    [SerializeField] internal Vector2Int size = new Vector2Int(30, 30);
    [SerializeField] internal byte[,] map;
    [SerializeField] internal DungeonSegmentStruct[,] mapDef;
    [SerializeField] internal int scale = 6;


    private CorridorMaker corridorMaker;
    [SerializeField] private List<DungeonSegmentDef> corridorDefList = new List<DungeonSegmentDef>();
    [SerializeField] private DungeonSegmentDef door;

    private RoomMaker roomMaker;
    [SerializeField] private List<DungeonSegmentDef> roomDefList = new List<DungeonSegmentDef>();
    [SerializeField] private List<DungeonSegmentDef> collumnDefList = new List<DungeonSegmentDef>();

    [SerializeField] private Transform wallLight;
    [SerializeField] private Transform cellingLiht;

    [SerializeField] private Transform treasureChest;

    [SerializeField] private Vector2Int characterPosition;

    public struct DungeonSegmentStruct
    {
        public byte id;
        public string idString;
        public DungeonSegmentDef dungeonSegmentDef;
        public int rotation;
        public Transform segmentInScene;
    }

    public enum MapPlane
    {
        Complete,
        Corridors,
        Rooms,
    }
    private void Start()
    {
        InitialiseMap();

        mapDef = new DungeonSegmentStruct[size.x, size.y];

        corridorMaker = new CorridorMaker(1, mapDef, size, scale, map ,corridorDefList, door);
        corridorMaker.Generate();

        roomMaker = new RoomMaker(2, mapDef, size, scale, map, roomDefList, collumnDefList);
        roomMaker.Generate(5, 3, 10);

        corridorMaker.Draw();
        roomMaker.Draw();

        DrawMap();

        DrawLights();
        SetTreasureChest();

        //HideMap();

        SetChracterPosition();
    }

    private void Update()
    {
        //HandleMap();
    }

    private void HandleMap()
    {
        int x = (int)character.position.x / scale;
        int z = (int)character.position.z / scale;
        Vector2Int currentPosition = new Vector2Int(x, z);
        if(characterPosition == currentPosition) { return; }
        Vector2Int direction = characterPosition - currentPosition;
        /*
        for (int i = x - 3; i < x + 3; i++)
        {
            if (z - 4 < 0) break;
            if (z + 4 >= size.x) break;
            if (i < 0) continue;
            if (i >= size.y) continue;
            mapDef[i, z + direction.y * 4].segmentInScene.gameObject.SetActive(false);
            mapDef[i, z - direction.y * 4].segmentInScene.gameObject.SetActive(true);
        }

        for (int i = z - 3; i < z + 3; i++)
        {
            if (x - 4 < 0) break;
            if (x + 4 >= size.x) break;
            if (i < 0) continue;
            if (i >= size.x) continue;
            mapDef[x + direction.y * 4, i ].segmentInScene.gameObject.SetActive(false);
            mapDef[x - direction.y * 4, i ].segmentInScene.gameObject.SetActive(true);
        }*/
        ShowMapSgment(x, z);

    }

    private void SetChracterPosition()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if(map[x, z] == 1)
                {
                    ShowMapSgment(x, z);
                    characterPosition = new Vector2Int(x, z);
                    character.position = new Vector3(x * scale, 3, z* scale);
                    return;
                }
            }
        }
    }

    private void InitialiseMap()
    {
        map = new byte[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                map[x, z] = 0;
            }
        }
    }

    private void SetTreasureChest()
    {
        for (int x = 1; x < size.x; x++)
        {
            for (int z = 1; z < size.y; z++)
            {
                if (map[x, z] == 0) continue;
                if (mapDef[x,z].dungeonSegmentDef.name.Contains("Dead_End"))
                {
                    Transform treasure = Instantiate(treasureChest);
                    treasure.position = new Vector3(x * scale, 0, z * scale);
                    treasure.rotation = Quaternion.Euler(0, mapDef[x, z].rotation, 0);
                }

            }
        }
    }

    private void DrawLights()
    {
        bool drawLight = true;
        for (int x = 1; x < size.x; x++)
        {
            for (int z = 1; z < size.y; z++)
            {

                if (map[x,z] != 0 && drawLight)
                {
                    Transform lightPrefab = map[x,z] == 1 ? wallLight : cellingLiht;
                    Transform light = Instantiate(lightPrefab);
                    light.position = new Vector3(x * scale, 0, z * scale );
                    light.rotation = Quaternion.Euler(0, mapDef[x,z].rotation + mapDef[x, z].dungeonSegmentDef.lightRotationOffset, 0);
                }
                drawLight = !drawLight;

            }
        }
    }

    internal virtual void DrawMap()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if(map[x, z] == 0)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(x * scale, 0, z * scale );
                    wall.transform.localScale *= scale * 0.9f;
                } else if(map[x, z] == 2)
                {
                }
            }
        }
    }

    private void HideMap()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                if (map[x, z] == 0) continue;
                mapDef[x,z].segmentInScene.gameObject.SetActive(false);
            }
        }
    }

    private void ShowMapSgment(int x, int z)
    {
        for (int i = x - 3; i < x + 3; i++)
        {
            for (int j = z - 3; j < z + 3; j++)
            {
                if (i < 0) continue;
                if (j < 0) continue;
                if (i >= size.x) continue;
                if (j >= size.y) continue;
                if (map[i, j] == 0) continue;
                mapDef[i, j].segmentInScene.gameObject.SetActive(true);
            }
        }
    }


    public Vector2Int WorldPositionToDungeon(Vector3 position)
    {
        return new Vector2Int((int)position.x / scale , (int)position.z / scale);
    }

    public Vector3 DungeonPositionToWorld(Vector2Int position)
    {
        return new Vector3(position.x * scale, transform.position.y, position.y * scale);
    }
    
   private Vector2Int GetNextDungeonSegmentWithId(string id, Vector2Int position, Vector2Int direction)
   {
        if (mapDef[position.x + direction.x, position.y + direction.y].idString == id)
        {

        }
        return position;
   }
}
