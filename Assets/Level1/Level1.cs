using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level1 : MonoBehaviour
{
    public static void WorldCreation(World world)
    {
        Random.InitState(1); // Set random seed

        GameObject l1 = GameObject.Find("Level1");
        Level1 loc = l1.GetComponent<Level1>();

        Camera.main.gameObject.transform.position = new Vector3(10, 6, -10);

        int wide = 20;
        int tall = 12;
        world.simpleMap = new string[wide, tall];
        int buffer = 2;
        for (int i = 0; i < wide; i++)
        {
            for (int j = 0; j < tall; j++)
            {
                if (i < buffer || j < buffer || i >= wide - buffer || j >= tall - buffer) world.simpleMap[i, j] = "wall";
            }
        }

        // Stairs
        world.simpleMap[5, 5] = "up";
        world.simpleMap[14, 8] = "down";

        world.pcPos = new Vector2(5, 5);

        if (world.currentLevel == 1)
        {
            for (int i = 0; i < 10; i++)
            {
                world.simpleMap[Random.Range(4, 17), Random.Range(3, 9)] = "spikes";
            }
        }

        MapSystem.RenderWorldMap(world);
    }

    public static void WorldCreationSpecial(World world)
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}