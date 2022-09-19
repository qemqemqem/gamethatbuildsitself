using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level2 : MonoBehaviour
{
    public static void WorldCreation(World world)
    {
        //world.levelNotImplemented = true;
        Random.InitState(2); // Set random seed

        GameObject l1 = GameObject.Find("Level2");
        Level2 loc = l1.GetComponent<Level2>();



        if (world.currentLevel == 2)
        {

            for (int i = 0; i < 5; ++i)
            {
                int x = Random.Range(4, 17);
                int y = Random.Range(3, 9);
                if (world.simpleMap[x, y] != null)
                    continue;
                GoblinManager.SpawnGoblin(x, y);
            }
            for (int i = 0; i < 5; i++)
            {
                int x = Random.Range(4, 17);
                int y = Random.Range(3, 9);
                if (world.simpleMap[x, y] != null)
                    continue;
                world.simpleMap[x,y] = "spikes";
            }

        }

        MapSystem.RenderWorldMap(world);
    }
}