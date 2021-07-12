using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8, height = 7;
    [SerializeField] private string spritePath = "Sprites_Game/";
    [SerializeField] private float offsetX, offsetY;

    [SerializeField] private Tile defaultTile;
    [SerializeField] private InteractableTile defaultInteractableTile;
    public Dictionary<Vector2, Tile> grid;
    public Dictionary<Vector2, InteractableTile> tokens;

    public void GenerateGrid(LevelInfo level) {
        grid = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y<height; y++) {
                var posX = offsetX + x * defaultTile.transform.localScale.x;
                var posY = offsetY + y * defaultTile.transform.localScale.y;
                var currentTile = Instantiate(defaultTile, new Vector3(posX, posY), Quaternion.identity);
                currentTile.name = $"Tile({x},{y})";
                grid[new Vector2(x, y)] = currentTile;
            }
        }

        tokens = new Dictionary<Vector2, InteractableTile>();
        foreach (var item in level.items) {
            if (item.type != "start") {
                Vector2 coordinate = new Vector2(item.x, item.y);
                string identity = (item.type == "destination")? item.family : item.direction;
                grid[coordinate].GetComponent<SpriteRenderer>().sprite = GetSprite(item.type, identity);
                grid[coordinate].initData(item.direction, item.family, item.type);
                // Debug.Log($"Drawing {item.x},{item.y} with {item.type},{identity}");
            } else {
                var posX = offsetX + item.x * defaultTile.transform.localScale.x;
                var posY = offsetY + item.y * defaultTile.transform.localScale.y;
                var currentTile = Instantiate(defaultInteractableTile, new Vector3(posX, posY), Quaternion.identity);
                currentTile.name = $"Token({item.family})";
                currentTile.GetComponent<SpriteRenderer>().sprite = GetSprite(item.type, item.family);
                currentTile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GetSprite("arrow", item.direction);
                currentTile.initData(new Vector2(item.x, item.y), item.direction, item.family);
                tokens[new Vector2(item.x, item.y)] = currentTile;
            }
        }
    }

    public void GenerateToken(List<InteractableTileData> tokenList) {
        tokens = new Dictionary<Vector2, InteractableTile>();
        foreach (InteractableTileData item in tokenList)
        {
            var posX = offsetX + item.pos.x * defaultTile.transform.localScale.x;
            var posY = offsetY + item.pos.y * defaultTile.transform.localScale.y;
            var currentTile = Instantiate(defaultInteractableTile, new Vector3(posX, posY), Quaternion.identity);
            currentTile.name = $"Token({item.family})";
            string prefix = "";
            if (item.settled) {
                prefix = "complete";
            } else {
                prefix = "start";
            }
            currentTile.GetComponent<SpriteRenderer>().sprite = GetSprite(prefix, item.family);
            currentTile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GetSprite("arrow", item.direction);
            currentTile.initData(new Vector2(item.pos.x, item.pos.y), item.direction, item.family);
            tokens[new Vector2(item.pos.x, item.pos.y)] = currentTile;
        }
    }

    public void ClearGrid() {
        foreach (KeyValuePair<Vector2, Tile> entry in grid) {
            Destroy(entry.Value.gameObject);
        }
        grid.Clear();
        foreach (KeyValuePair<Vector2, InteractableTile> entry in tokens) {
            Destroy(entry.Value.gameObject);
        }
        tokens.Clear();
    }

    public void ClearToken() {
        foreach (KeyValuePair<Vector2, InteractableTile> entry in tokens) {
            Destroy(entry.Value.gameObject);
        }
        tokens.Clear();
    }

    public Sprite GetSprite(string type, string identity) {
        string path = spritePath + $"{type}_{identity}";
        return Resources.Load<Sprite>(path);
    }
}
