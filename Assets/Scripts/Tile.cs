using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public string direction { get; set;}
    public string family { get; set;}
    public string type {get; set;}
    public void initData(string _direction, string _family, string _type) {
        direction = _direction;
        family = _family;
        type = _type;
    }
}
