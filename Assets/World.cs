using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World
{
    public static World world;

    public int currentLevel;
    public bool levelNotImplemented = false;

    public string[,] simpleMap;
    public Vector2 pcPos;
    private Dictionary<Vector2Int, Transform> position2Creature = new Dictionary<Vector2Int, Transform>();

    public Transform GetContent(Vector2Int pos)
    {
        return position2Creature[pos];
    }

    public List<Transform> GetNearby(Vector2 position, float range)
    {
        //TODO implement for sensing nearby stuff
        return new List<Transform>();
    }

    public void SetPosition(Transform unit, Vector2 position)
    {
        Vector2Int unitCell = GetIntPosition(AsVector2(unit.position));
        Vector2Int nextCell = GetIntPosition(position);
        Transform conntentNext;
        if(position2Creature.TryGetValue(nextCell, out conntentNext))
        {
            if (conntentNext != unit)
                return;
        }
        
        if (unitCell != nextCell){
            position2Creature.Remove(unitCell);
            position2Creature.Add(nextCell, unit);
        } 
        if (!position2Creature.Values.Contains(unit))
        {
            position2Creature.Add(nextCell, unit);
        }

        unit.position = new Vector3(position.x, position.y, 0);
    }

    public static Vector2Int GetIntPosition(Vector2 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    public static Vector2 AsVector2(Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static Vector3 AsVector3(Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }
}

public class PlayerCharacter
{

}