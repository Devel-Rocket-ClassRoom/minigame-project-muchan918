using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Ground,
    GrassGround,
    Water,
    Resource,
    Border,
}

public class MapData
{
    public readonly int Width;
    public readonly int Height;

    public readonly List<Vector2Int> NearTiles = new();
    public readonly List<Vector2Int> MidTiles = new();
    public readonly List<Vector2Int> FarTiles = new();

    private readonly TileType[,] tiles;
    private readonly int baseSize = 30;

    private readonly float nearZoneRadius;
    private readonly float midZoneRadius;

    public MapData(int width, int height, int seed, float nearZoneRadius, float midZoneRadius)
    {
        Width = width;
        Height = height;
        tiles = new TileType[height, width];
        this.nearZoneRadius = nearZoneRadius;
        this.midZoneRadius = midZoneRadius;
        Generate(seed);
        CacheTiles();
    }

    private void Generate(int seed)
    {
        System.Random random = new System.Random(seed);

        // 1단계: 전체 Ground 초기화
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            tiles[y, x] = TileType.Ground;

        // 2단계: 물 웅덩이 Spread
        int waterSeedCount = (Width * Height) / 2000;
        for (int i = 0; i < waterSeedCount; i++)
        {
            int tx = random.Next(0, Width);
            int ty = random.Next(0, Height);
            Spread(tx, ty, 20, TileType.Water, 0.65f, random);
        }

        // 3단계: Ground 중 일부를 GrassGround로
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            if (tiles[y, x] == TileType.Ground && random.NextDouble() < 0.3)
                tiles[y, x] = TileType.GrassGround;
    }

    private void CacheTiles()
    {
        int halfWidth = Width / 2;
        int halfHeight = Height / 2;
        int baseHalf = baseSize / 2;

        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
        {
            int wx = x - halfWidth;
            int wy = y - halfHeight;

            if (wx >= -baseHalf && wx < baseHalf && wy >= -baseHalf && wy < baseHalf)
                continue;
            if (tiles[y, x] != TileType.Ground && tiles[y, x] != TileType.GrassGround)
                continue;

            float dist = Mathf.Sqrt(wx * wx + wy * wy);
            var coord = new Vector2Int(wx, wy);

            if (dist < nearZoneRadius)
                NearTiles.Add(coord);
            else if (dist < midZoneRadius)
                MidTiles.Add(coord);
            else
                FarTiles.Add(coord);
        }
    }

    public void SetTile(Vector2Int worldCoord, TileType type)
    {
        int x = worldCoord.x + Width / 2;
        int y = worldCoord.y + Height / 2;
        if (InBounds(x, y))
            tiles[y, x] = type;
    }

    private void Spread(
        int tx,
        int ty,
        int depth,
        TileType type,
        float spreadChance,
        System.Random random
    )
    {
        if (depth <= 0 || !InBounds(tx, ty))
            return;
        if (tiles[ty, tx] == type)
            return;
        if (IsBaseArea(tx, ty))
            return;

        tiles[ty, tx] = type;

        if (random.NextDouble() < spreadChance)
            Spread(tx + 1, ty, depth - 1, type, spreadChance, random);
        if (random.NextDouble() < spreadChance)
            Spread(tx - 1, ty, depth - 1, type, spreadChance, random);
        if (random.NextDouble() < spreadChance)
            Spread(tx, ty + 1, depth - 1, type, spreadChance, random);
        if (random.NextDouble() < spreadChance)
            Spread(tx, ty - 1, depth - 1, type, spreadChance, random);
    }

    public TileType GetTile(int x, int y)
    {
        if (!InBounds(x, y))
            return TileType.Ground;
        return tiles[y, x];
    }

    public bool IsWater(int x, int y) => GetTile(x, y) == TileType.Water;

    public float GetWaterRatio(int chunkX, int chunkY, int chunkSize)
    {
        int startX = chunkX * chunkSize;
        int startY = chunkY * chunkSize;
        int waterCount = 0;

        for (int y = startY; y < startY + chunkSize; y++)
        for (int x = startX; x < startX + chunkSize; x++)
            if (InBounds(x, y) && tiles[y, x] == TileType.Water)
                waterCount++;

        return (float)waterCount / (chunkSize * chunkSize);
    }

    private bool IsBaseArea(int x, int y)
    {
        int cx = Width / 2;
        int cy = Height / 2;
        int half = baseSize / 2;
        return x >= cx - half && x <= cx + half && y >= cy - half && y <= cy + half;
    }

    public TileType GetTileWorld(Vector2Int worldCoord)
    {
        int x = worldCoord.x + Width / 2;
        int y = worldCoord.y + Height / 2;
        return GetTile(x, y);
    }

    private bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
}
