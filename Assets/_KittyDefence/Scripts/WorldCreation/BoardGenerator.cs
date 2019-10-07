using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.AI;

public class BoardGenerator : MonoBehaviour
{
    //[SerializeField]
    //public string BoardId = Guid.NewGuid().ToString();
    public bool GenerateOnLoad = false;
    public float BlockAnimateTime = 0.03f;
    public static Action<Vector3> OnCompleted;
    public static Action OnLoading;
    public int BoardWidth = 8;
    public int BoardHeight = 8;
    
    public BlockDatabase Database;
    public BoardRandomizer Randomizer;
    public float UnitSpacing = 1f;
    
    [SerializeField]
    public BoardInformation Board;
    
    private Vector3 _playerStartPosition;
    private NavMeshSurface _navMesh;


    private void OnEnable()
    {
        if (!GenerateOnLoad)
            return;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        SetupGrid();
    }

    private void SetupGrid()
    {
        if (Board == null || Board.Rows == null || Board.Rows.Length == 0)
        {
            Board = Randomizer.GenerateBoard(BoardWidth, BoardHeight);
            //Board.Id = BoardId;
        }
        StartCoroutine(GenerateBoardWithAnimationFromKey(Board));
    }

    public void LoadBoard(BoardInformation board)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(GenerateBoardWithAnimationFromKey(board));
    }

    public void LoadBoard()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Board = Randomizer.GenerateBoard(BoardWidth, BoardHeight);
        StartCoroutine(GenerateBoardWithAnimationFromKey(Board));
    }

    public void CleanupBoard()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public Vector3 GetStartPosition()
    {
        return new Vector3(_playerStartPosition.x, 0f, _playerStartPosition.z);
    }

    private IEnumerator GenerateBoardWithAnimationFromKey(BoardInformation board)
    {
        if (OnLoading != null)
            OnLoading.Invoke();

        var xCenterOffset = (board.Width * UnitSpacing) / 2;
        var yCenterOffset = (board.Height * UnitSpacing) / 2;

        for (int i = 0; i < board.Rows.Length; i++)
        {
            for (int j = 0; j < board.Rows[i].Tiles.Length; j++)
            {
                var tile = Database.GetBlockById(board.Rows[i].Tiles[j].Key);
                var newPos = new Vector3(i * UnitSpacing - xCenterOffset, 0, j * UnitSpacing - yCenterOffset);

                var block = InstantiateAndAnimateBlock(tile.Item, newPos);
                
                if (tile.Id == "playerspawn")
                    _playerStartPosition = newPos;

                yield return new WaitForSeconds(BlockAnimateTime);
            }
        }

        yield return new WaitForSeconds(1f);
        //StaticBatchingUtility.Combine(gameObject);
        SetupBoard();
        if (OnCompleted != null)
            OnCompleted.Invoke(GetStartPosition());
    }

    private void SetupBoard()
    {
        if (_navMesh == null)
            _navMesh = GetComponent<NavMeshSurface>();
        _navMesh.BuildNavMesh();
    }

    private GameObject InstantiateAndAnimateBlock(GameObject item, Vector3 newPos)
    {
        var block = Instantiate(item, new Vector3(newPos.x, -0.5f, newPos.z), Quaternion.identity, transform);

        block.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        block.transform.DOMoveY(0, 1).SetEase(Ease.OutElastic);
        block.transform.DOScale(1, 1).SetEase(Ease.OutElastic);
        return block;
    }
}

[Serializable]
public class BoardInformation
{
    public string Id;
    public int Width;
    public int Height;
    public RowInfo[] Rows;
}

[Serializable]
public class RowInfo
{
    public TileInfo[] Tiles;
}

[Serializable]
public class TileInfo
{
    public string Key;
    public string Config;
}
