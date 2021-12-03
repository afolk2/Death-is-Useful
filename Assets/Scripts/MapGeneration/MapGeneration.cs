using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField]
    int minMapSize;
    [SerializeField]
    int maxMapSize;
    int[,] currentMap;

    int[,] newMap;
    [SerializeField]
    int seed;
    [SerializeField]
    bool useSeed;
    [SerializeField]
    bool randomExit;
    int minRoomSize = 3;

    List<Room> rooms;

    Vector2 exitDir;

    public int mapWidth { get; private set; }
    public int mapHeight { get; private set; }

    public void Init()
    {
        if (!useSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        if (randomExit)
        {
            int nextExit = Random.Range(0, 4);
            switch (nextExit)
            {
                case 0:
                    exitDir = Vector2.up;
                    break;
                case 1:
                    exitDir = Vector2.right;
                    break;
                case 2:
                    exitDir = Vector2.down;
                    break;
                case 3:
                    exitDir = Vector2.left;
                    break;
                default:
                    break;
            }
        }

        currentMap = new int[Random.Range(minMapSize, maxMapSize), Random.Range(minMapSize, maxMapSize)];

        mapWidth = currentMap.GetLength(0);
        mapHeight = currentMap.GetLength(1);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                currentMap[x, y] = 1;
            }
        }

        rooms = new List<Room>();

        GenerateMap();

        UpdateMap();

        DrawMap();

    }


    void GenerateMap()
    {
        int xSectionSize = mapWidth / 3;
        int ySectionSize = mapHeight / 3;

        for (int x = 1; x < xSectionSize * 3; x += xSectionSize)
        {
            for (int y = 1; y < ySectionSize * 3; y += ySectionSize)
            {
                int maxXSize = Mathf.Min(mapWidth - x, xSectionSize + 1);
                int maxYSize = Mathf.Min(mapHeight - y, ySectionSize + 1);
                GenerateRoom(x, y, maxXSize, maxYSize);
            }
        }

        MakeEntrance(exitDir);
    }

    private void MakeEntrance(Vector2 exitDir)
    {
        int opening;
        int wall;
        Room openingRoom;
        // Openings should be on the opposite side as the exit direction
        if (exitDir == Vector2.right)
        {
            opening = Random.Range(1, mapHeight - 2);
            openingRoom = new Room(1, 2, new Vector2(0, opening));
            exitDir = Vector2.left;
        }
        else if (exitDir == Vector2.left)
        {
            wall = currentMap.GetLength(0) - 1;
            opening = Random.Range(1, mapHeight - 2);
            openingRoom = new Room(1, 2, new Vector2(wall, opening));
            exitDir = Vector2.right;
        }
        else if (exitDir == Vector2.up)
        {
            opening = Random.Range(1, mapWidth - 2);
            openingRoom = new Room(2, 1, new Vector2(opening, 0));
            exitDir = Vector2.down;
        }
        else
        {
            wall = currentMap.GetLength(1) - 1;
            opening = Random.Range(1, mapWidth - 2);
            openingRoom = new Room(2, 1, new Vector2(opening, wall));
            exitDir = Vector2.up;
        }
        rooms.Add(openingRoom);

        List<Vector2> possibleExitDirs = new List<Vector2> { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        possibleExitDirs.Remove(exitDir);
        exitDir = possibleExitDirs[Random.Range(0, possibleExitDirs.Count)];
    }

    void UpdateMap()
    {
        List<Vector2> unconnected = new List<Vector2>();
        foreach (Room item in rooms)
        {
            for (int x = 0; x < item.xSize; x++)
            {
                for (int y = 0; y < item.ySize; y++)
                {
                    int xPos = (int)item.position.x + x;
                    int yPos = (int)item.position.y + y;
                    currentMap[xPos, yPos] = 0;

                    if (!unconnected.Contains(new Vector2(xPos, yPos)))
                    {
                        unconnected.Add(new Vector2(xPos, yPos));
                    }
                }
            }
        }

        List<NodeRoom> nodeRooms = new List<NodeRoom>();
        int nodeRoomIdx = 0;

        while (unconnected.Count > 0)
        {
            // init a new NodeRoom
            nodeRooms.Add(new NodeRoom());

            // start with the first unconnected node
            Vector2 node = unconnected[0];

            // add it to the nodeRoom and remove it from unconnected
            nodeRooms[nodeRoomIdx].AddFloorNode(node);
            unconnected.Remove(node);

            // list to iterate over all possible node connections for this NodeRoom
            List<Vector2> neighbours = GetNodeNeighbours(node);

            for (int i = 0; i < neighbours.Count; i++)
            {
                #region TEST
                Vector2 testItem = new Vector2(neighbours[i].x, neighbours[i].y);

                if (currentMap[(int)testItem.x, (int)testItem.y] == 1 && !nodeRooms[nodeRoomIdx].wallNodes.Contains(testItem))
                {
                    //if the neighbour is not a map border
                    if (testItem.x > 0 && testItem.x < mapWidth - 1 && testItem.y > 0 && testItem.y < mapHeight - 1)
                    {
                        // add to room walls
                        nodeRooms[nodeRoomIdx].AddWallNode(testItem);
                    }
                }
                else
                {
                    // if neighbour floor, part of unconnected, NOT part of room floors
                    if (currentMap[(int)testItem.x, (int)testItem.y] == 0 && unconnected.Contains(testItem) && !nodeRooms[nodeRoomIdx].floorNodes.Contains(testItem))
                    {
                        // add to room floors
                        nodeRooms[nodeRoomIdx].AddFloorNode(testItem);
                        // remove from unconnected
                        unconnected.Remove(testItem);

                        // add the item's neighbours
                        neighbours = neighbours.Union(GetNodeNeighbours(testItem)).ToList();
                    }
                }
                #endregion
                #region Not working
                // get neighbors from node
                //foreach (Vector2 item in GetNodeNeighbours(neighbours[i]))
                //{
                //    int x = (int)item.x;
                //    int y = (int)item.y;

                //    // if neighbour wall and not already part of room walls
                //    if (currentMap[x, y] == 1 && !nodeRooms[nodeRoomIdx].wallNodes.Contains(item))
                //    {
                //        //if the neighbour is not a map border
                //        if (x > 0 && x < mapWidth - 1 && y > 0 && y < mapHeight - 1)
                //        {
                //            // add to room walls
                //            nodeRooms[nodeRoomIdx].AddWallNode(item);
                //        }
                //    }
                //    else
                //    {
                //        // if neighbour floor, part of unconnected, NOT part of room floors
                //        if (currentMap[x, y] == 0 && unconnected.Contains(item) && !nodeRooms[nodeRoomIdx].floorNodes.Contains(item))
                //        {
                //            // add to room floors
                //            nodeRooms[nodeRoomIdx].AddFloorNode(item);
                //            // remove from unconnected
                //            unconnected.Remove(item);
                //            // add to neighbours to search nodeNeighbours
                //            neighbours.Add(item);
                //        }
                //    }
                //}
                #endregion
            }

            nodeRoomIdx++;
        }

        // carving path to connect rooms
        // until 1 room left
        #region Carving
        while (nodeRooms.Count > 1)
        {
            List<Vector2> sharedWalls = new List<Vector2>();
            // List of shared walls between room 0 and room 1
            sharedWalls = nodeRooms[0].wallNodes.Intersect(nodeRooms[1].wallNodes).ToList();

            // if they share at least 1 wall
            if (sharedWalls.Count > 0)
            {
                // pick random wall
                Vector2 wall = sharedWalls[Random.Range(0, sharedWalls.Count)];

                RemoveWallAndCombineRooms(nodeRooms, 0, 1, wall);
            }
            else
            {
                // pick room 0 or 1
                int room = Random.Range(0, 2);
                // pick a wall in the selected room
                Vector2 wall = nodeRooms[room].wallNodes[Random.Range(0, nodeRooms[room].wallNodes.Count)];
                // find if the cut wall is part of a different room. -1 for no
                int wallRoom = FindWall(nodeRooms, room, wall);
                if (wallRoom > -1)
                {
                    RemoveWallAndCombineRooms(nodeRooms, room, wallRoom, wall);
                }
                else
                {
                    RemoveWallFromRoom(nodeRooms, room, wall);
                }
            }
        }
        #endregion
    }

    private void RemoveWallFromRoom(List<NodeRoom> rooms, int room, Vector2 wall)
    {
        // cut wall
        currentMap[(int)wall.x, (int)wall.y] = 0;

        rooms[room].wallNodes.Remove(wall);

        // get the neighbouring walls from the old wall
        List<Vector2> possibleWalls = GetWallNeighbours(wall);

        // add floor(old wall) with new possible walls to room
        rooms[room].AddFloorWithPossibleWalls(wall, possibleWalls);
    }

    private int FindWall(List<NodeRoom> nodeRooms, int room, Vector2 wall)
    {
        int roomIdx = -1;
        for (int i = 0; i < nodeRooms.Count; i++)
        {
            if (nodeRooms[i].wallNodes.Contains(wall) && i != room)
            {
                roomIdx = i;
            }
        }
        return roomIdx;
    }

    private void RemoveWallAndCombineRooms(List<NodeRoom> rooms, int roomAIdx, int roomBIdx, Vector2 wall)
    {
        // cut wall
        currentMap[(int)wall.x, (int)wall.y] = 0;

        // remove that wall from both nodeRooms wallNodes lists
        rooms[roomAIdx].wallNodes.Remove(wall);
        rooms[roomBIdx].wallNodes.Remove(wall);

        // get the neighbouring walls from the old wall
        List<Vector2> possibleWalls = GetWallNeighbours(wall);

        // add floor(old wall) with new possible walls to first room
        rooms[roomAIdx].AddFloorWithPossibleWalls(wall, possibleWalls);
        // add second room to first room
        rooms[roomAIdx].CombineRoom(rooms[roomBIdx]);
        // remove second room
        rooms.RemoveAt(roomBIdx);
    }

    void DrawMap()
    {
        if (GameObject.Find("MapContainer"))
        {
            DestroyImmediate(GameObject.Find("MapContainer"));
        }
        Transform mapContainer = new GameObject("MapContainer").transform;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                if (currentMap[x, y] == 0)
                {
                    go.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                }
                go.transform.position = new Vector3(x, y);
                go.transform.parent = mapContainer;

            }
        }
    }





    private void GenerateRoom(int x, int y, int maxXSize, int maxYSize)
    {
        int roomXSize = Random.Range(minRoomSize, maxXSize);
        int roomYSize = Random.Range(minRoomSize, maxYSize);

        Vector2 start = new Vector2(Random.Range(x, x + (maxXSize - roomXSize)), Random.Range(y, y + (maxYSize - roomYSize)));
        rooms.Add(new Room(roomXSize, roomYSize, start));
    }

    private List<Vector2> GetWallNeighbours(Vector2 nodePos)
    {
        List<Vector2> neighbours = new List<Vector2>();

        foreach (Vector2 item in GetNodeNeighbours(nodePos))
        {
            if (currentMap[(int)item.x, (int)item.y] == 1)
            {
                if (item.x > 0 && item.y > 0 && item.x < mapWidth - 1 && item.y < mapHeight - 1)
                    neighbours.Add(item);
            }
        }
        return neighbours;
    }

    private List<Vector2> GetFloorNeighbours(Vector2 nodePos)
    {
        List<Vector2> neighbours = new List<Vector2>();

        foreach (Vector2 item in GetNodeNeighbours(nodePos))
        {
            if (currentMap[(int)item.x, (int)item.y] == 0)
            {
                neighbours.Add(item);
            }
        }
        return neighbours;
    }

    private List<Vector2> GetNodeNeighbours(Vector2 nodePos)
    {
        List<Vector2> neighbours = new List<Vector2>();

        if (nodePos.y < mapHeight - 1)
        {
            neighbours.Add(nodePos + Vector2.up);

        }
        if (nodePos.x < mapWidth - 1)
        {

            neighbours.Add(nodePos + Vector2.right);
        }
        if (nodePos.y > 0)
        {
            neighbours.Add(nodePos + Vector2.down);

        }
        if (nodePos.x > 0)
        {
            neighbours.Add(nodePos + Vector2.left);

        }

        return neighbours;
    }


}


public class NodeRoom
{
    public List<Vector2> floorNodes;
    public List<Vector2> wallNodes;

    public NodeRoom()
    {
        floorNodes = new List<Vector2>();
        wallNodes = new List<Vector2>();
    }

    public void AddFloorNode(Vector2 floor)
    {
        floorNodes.Add(floor);
    }
    public void AddFloorNodes(List<Vector2> floors)
    {
        floorNodes.AddRange(floors);
    }

    public void AddWallNode(Vector2 wall)
    {
        wallNodes.Add(wall);
    }
    public void AddWallNodes(List<Vector2> walls)
    {
        wallNodes.AddRange(walls);
    }
    public void AddFloorWithPossibleWalls(Vector2 floor, List<Vector2> possibleWalls)
    {
        floorNodes.Add(floor);
        wallNodes = wallNodes.Union(possibleWalls).ToList();

    }
    public void CombineRoom(NodeRoom room)
    {
        floorNodes = floorNodes.Union(room.floorNodes).ToList();
        wallNodes = wallNodes.Union(room.wallNodes).ToList();
    }
}
public class Room
{
    public Vector2 position { get; private set; }
    public int xSize { get; private set; }
    public int ySize { get; private set; }
    public int area { get; private set; }


    public Room(int xSize, int ySize, Vector2 position)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        area = xSize * ySize;
        this.position = position;
    }
}
