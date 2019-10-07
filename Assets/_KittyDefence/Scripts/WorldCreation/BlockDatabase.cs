using System;
using System.Linq;
using UnityEngine;

public class BlockDatabase : MonoBehaviour
{
    [SerializeField]
    public Block[] Blocks;

    public Block GetBlockById(string id)
    {
        return Blocks.FirstOrDefault(x => x.Id == id);
    }

    [Serializable]
    public class Block
    {
        public string Id;
        public GameObject Item;
    }
}
