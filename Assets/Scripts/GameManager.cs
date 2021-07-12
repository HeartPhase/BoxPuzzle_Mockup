using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    LevelData levelData;
    [SerializeField] int levelID;
    private List<int> passedLevels = new List<int>();
    private bool gameWin = false;
    [SerializeField] int loadLevelOverride = 999;
    private List<List<InteractableTileData>> timeMachine = new List<List<InteractableTileData>>();
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();

        levelData = DataParser.CreateFromJSON();

        int levelCount = levelData.levels.Count;
        levelID = Random.Range(0, levelCount);
        if (loadLevelOverride != 999) levelID = loadLevelOverride;
        // Debug.Log(levelData.levels[1]);
        initializeLevel();
    }

    void initializeLevel() {
        gridManager.GenerateGrid(levelData.levels[levelID]);
    }

    void LoadNextLevel() {
        // int levelCount = levelData.levels.Count;
        // int completeLevelCount = passedLevels.Count;
        // int tempLevelID = Random.Range(0, levelCount - completeLevelCount) + 1;
        // for (int i = 0; i < levelCount; i++) {
        //     if (passedLevels.IndexOf(i) == -1) tempLevelID--;
        //     if (tempLevelID == 0) levelID = i;
        // }
        while (passedLevels.IndexOf(levelID) != -1) {
            levelID = Random.Range(0, levelData.levels.Count);
        }
        gridManager.ClearGrid();
        initializeLevel();
        Debug.Log($"Load level {levelID}");
    }

    public void ReloadCurrentLevel() {
        gridManager.ClearGrid();
        initializeLevel();
    }

    public void Rewind() {
        List<InteractableTileData> targetState = timeMachine[timeMachine.Count - 1];
        gridManager.ClearToken();
        gridManager.GenerateToken(targetState);
        timeMachine.RemoveAt(timeMachine.Count - 1);
    }

    public void TileMove(InteractableTile tile) {

        List<InteractableTileData> currentState = new List<InteractableTileData>();
        foreach (KeyValuePair<Vector2, InteractableTile> item in gridManager.tokens)
        {
            currentState.Add(item.Value.StateFactory());
        }
        timeMachine.Add(currentState);

        LevelInfo data = levelData.levels[levelID];

        Vector2 newPos = tile.pos + DirectionVec.directionVec[tile.direction];
        if (newPos.x < 0 || newPos.x >= gridManager.width || newPos.y < 0 || newPos.y >= gridManager.height) {
            Debug.Log("Out of bound");
            return;
        }
        tile.pos = tile.pos + DirectionVec.directionVec[tile.direction];
        foreach (KeyValuePair<Vector2, InteractableTile> token in gridManager.tokens)
        {
            if (token.Value.pos == tile.pos && token.Value.family != tile.family) {
                if (!TileMove(token.Value, tile.direction)) {
                    tile.pos = tile.pos - DirectionVec.directionVec[tile.direction];
                    return;
                }
            }
        }
        if (tile.settled) {
            tile.settled = false;
            tile.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("start", tile.family);
        }
        if (gridManager.grid.ContainsKey(tile.pos)) {
            Tile targetItem = gridManager.grid[tile.pos];
            if (targetItem.type == "arrow") {
                tile.direction = targetItem.direction;
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("arrow", tile.direction);
            } else {
                if (targetItem.family == tile.family) {
                    tile.settled = true;
                    tile.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("complete", tile.family);
                    gameWin = true;
                    foreach (KeyValuePair<Vector2, InteractableTile> token in gridManager.tokens)
                        {
                            if (!token.Value.settled) gameWin = false;
                        }
                    if (gameWin) CompleteLevel();
                }
            }
        }
        tile.transform.position = gridManager.grid[tile.pos].transform.position;
    }

    public bool TileMove(InteractableTile tile, string direction) {
        LevelInfo data = levelData.levels[levelID];

        Vector2 newPos = tile.pos + DirectionVec.directionVec[direction];
        if (newPos.x < 0 || newPos.x >= gridManager.width || newPos.y < 0 || newPos.y >= gridManager.height) {
            Debug.Log("Out of bound");
            return false;
        }
        tile.pos = tile.pos + DirectionVec.directionVec[direction];
        foreach (KeyValuePair<Vector2, InteractableTile> token in gridManager.tokens)
        {
            if (token.Value.pos == tile.pos && token.Value.family != tile.family) {
                if (!TileMove(token.Value, direction)) {
                    tile.pos = tile.pos - DirectionVec.directionVec[direction];
                    return false;
                }
            }
        }
        if (tile.settled) {
            tile.settled = false;
            tile.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("start", tile.family);
        }
        if (gridManager.grid.ContainsKey(tile.pos)) {
            Tile targetItem = gridManager.grid[tile.pos];
            if (targetItem.type == "arrow") {
                tile.direction = targetItem.direction;
                tile.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("arrow", tile.direction);
            } else {
                if (targetItem.family == tile.family) {
                    tile.settled = true;
                    tile.GetComponent<SpriteRenderer>().sprite = gridManager.GetSprite("complete", tile.family);
                    gameWin = true;
                    foreach (KeyValuePair<Vector2, InteractableTile> token in gridManager.tokens)
                        {
                            if (!token.Value.settled) gameWin = false;
                        }
                    if (gameWin) CompleteLevel();
                }
            }
        }
        tile.transform.position = gridManager.grid[tile.pos].transform.position;
        return true;
    }

    void CompleteLevel() {
        passedLevels.Add(levelID);
        if (passedLevels.Count == levelData.levels.Count) {
            Debug.Log("Game Beat");
            //TODO: show win screen
        } else {
            LoadNextLevel();
        }
    }
}

public static class DirectionVec {
    public static Dictionary<string, Vector2> directionVec = new Dictionary<string, Vector2> {
        {"up", new Vector2(0, 1)},
        {"down", new Vector2(0, -1)},
        {"left", new Vector2(-1, 0)},
        {"right", new Vector2(1, 0)}
    };
}
