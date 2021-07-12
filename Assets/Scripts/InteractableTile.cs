using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : Tile
{
    GameManager gameManager;
    public bool settled {set; get;}
    public Vector2 pos {set; get;}

    InteractableTile() {

    }
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        settled = false;
    }

    public void initData(Vector2 _pos, string _direction, string _family) {
        pos = _pos;
        direction = _direction;
        family = _family;
    }
    void OnMouseEnter() {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
    }

    void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void OnMouseUp() {
        gameManager.TileMove(this);
    }

    public InteractableTileData StateFactory() {
        return new InteractableTileData(pos, direction, family, settled);
    }
}

public class InteractableTileData {
    public Vector2 pos;
    public string direction;
    public string family;
    public bool settled;
    public InteractableTileData(Vector2 _pos, string _direction, string _family, bool _settled) {
        pos = _pos;
        direction = _direction;
        family = _family;
        settled = _settled;

    }
}
