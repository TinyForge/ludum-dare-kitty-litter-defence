using System;
using System.Linq;
using UnityEngine;

public class BoardRandomizer : MonoBehaviour
{
    [TextArea(1,10)]
    public string Key;
    public string BoardId = Guid.NewGuid().ToString();
    public int BoardWidth = 8;
    public int BoardHeight = 8;
    public string DefaultBlockId = "g1";
    [SerializeField]
    public ItemRange[] Items;

    public GridOverrides[] Overrides = new GridOverrides[] {
        new GridOverrides() { BlockId = "start", Position = new Vector2(7,4) },
        new GridOverrides() { BlockId = "end", Position = new Vector2(0, 3) } };

    private void Awake()
    {
        var newBoard = GenerateBoard(8, 8);
        Key = JsonUtility.ToJson(newBoard);
    }

    public string GetRandomBlock()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            var rand = UnityEngine.Random.Range(0, Items[i].Rate);
            if (rand == 0)
                return Items[i].BlockId;
        }
        return DefaultBlockId;
    }

    public BoardInformation GenerateBoard()
    {
        return GenerateBoard(BoardWidth, BoardHeight);
    }

    public BoardInformation GenerateBoard(int width, int height)
    {
        var board = new BoardInformation();
        board.Id = BoardId;
        board.Width = width;
        board.Height = height;
        board.Rows = new RowInfo[width];

        for (int i = 0; i < board.Rows.Length; i++)
        {
            board.Rows[i] = new RowInfo()
            {
                Tiles = new TileInfo[height]
            };
            for (int j = 0; j < board.Rows[i].Tiles.Length; j++)
            {
                board.Rows[i].Tiles[j] = new TileInfo();
                var overrideBlock = Overrides.FirstOrDefault(o => i == (int)o.Position.x && j == (int)o.Position.y);
                if (overrideBlock != null)
                {
                    board.Rows[i].Tiles[j].Key = overrideBlock.BlockId;
                    board.Rows[i].Tiles[j].Config = overrideBlock.BlockConfig;
                }
                else
                    board.Rows[i].Tiles[j].Key = GetRandomBlock();
            }
        }

        return board;
    }

    [Serializable]
    public class ItemRange
    {
        public string BlockId;
        public int Rate = 10;
    }

    [Serializable]
    public class GridOverrides
    {
        public Vector2 Position;
        public string BlockId;
        public string BlockConfig;
    }
}
