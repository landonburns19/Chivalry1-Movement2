using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGen : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        HOLE,
        STAIR,
        EMPTY
    }

    public Grid[,] gridHandler;

    public List<WalkerObject> Walkers;

    public Tilemap tileMap;

    public Tile Floor;
    public Tile Wall;
    public Tile Wallmiddle;
    public Tile WallLR;
    public Tile Hole;
    public Tile Pillar;
    public Tile Stair;
    private Tile currTile;

    public int MapWidth = 30;

    public int MapHeight = 30;

    public int MaximumWalkers = 10;
    public int TileCount = default; 

    public float FillPercentage = 0.4f;

    public float WaitTime = 0.05f;



    // Start is called before the first frame update
    void Start()
    {
        createGrid();   
    }

    void createGrid(){
        gridHandler = new Grid[MapWidth, MapHeight];

        for(int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for(int y = 0; y <  gridHandler.GetLength(1); y++)
            {
                gridHandler[x,y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);
        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        tileMap.SetTile(TileCenter, Floor);
        Walkers.Add(curWalker);


        TileCount++;

        StartCoroutine(CreateFloors()); 
    }


    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }


    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(curPos, Floor);
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        StartCoroutine(CreateWalls());
    }

// Chance
    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }


// Updating the walker
    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }


    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;


                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        currTile = getTile(x + 1 , y);
                        tileMap.SetTile(new Vector3Int(x + 1, y, 0), currTile);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        currTile = getTile(x - 1 , y);
                        tileMap.SetTile(new Vector3Int(x - 1, y, 0), currTile);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1 ] == Grid.EMPTY)
                    {
                        currTile = getTile(x, y + 1);
                        tileMap.SetTile(new Vector3Int(x, y + 1, 0), currTile);
                        gridHandler[x, y ] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y ] == Grid.EMPTY)
                    {
                        currTile = getTile( x, y - 1);
                        tileMap.SetTile(new Vector3Int(x, y - 1, 0), currTile);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }
    }

    Tile getTile(int x, int y)
    {
        bool upEmpty = false;
        bool leftEmpty = false;
        bool rightEmpty = false;
        bool downEmpty = false;
        if(gridHandler.GetLength(0) <= x || gridHandler[x + 1, y] == Grid.EMPTY)
        {
            rightEmpty = true;
        }
        if(x < 0 || gridHandler[x - 1, y] == Grid.EMPTY)
        {
            leftEmpty = true;
        }
        if(gridHandler.GetLength(1) <= y || gridHandler[x, y + 1] == Grid.EMPTY)
        {
            downEmpty = true;
        }
        if (y < 0 || gridHandler[x, y - 1] == Grid.EMPTY)
        {
            upEmpty = true;
        }


        if(upEmpty && downEmpty && (!leftEmpty || !rightEmpty))
        {
            return WallLR;
        }
        if (leftEmpty && rightEmpty && (!upEmpty || !downEmpty))
        {
            return Wall;
        }
        if (!leftEmpty && !rightEmpty && !upEmpty && !downEmpty)
        {
            return Wallmiddle;
        }
        return Wallmiddle; 
    }


}
