using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinManager : MonoBehaviour
{
    public Transform goblinPrefab;
    public static GoblinManager instance;
    private List<Transform> goblins = new List<Transform>();
    public AudioClip deathSound;
    // Start is called before the first frame update

    float timeSinceUpdate = 0f;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceUpdate += Time.deltaTime;
        if (timeSinceUpdate > .5)
        {
            timeSinceUpdate = 0;
            UpdateGoblins();
        }
    }

    void UpdateGoblins()
    {
        World world = World.world;
        if (world != null)
        {
            foreach (var goblin in goblins)
            {
                Vector2 offset =World.AsVector2(World.AsVector3(world.pcPos) - goblin.position).normalized;
                int deltaX = Mathf.RoundToInt(offset.x);
                int deltaY = Mathf.RoundToInt(offset.y);
                Vector2 newPos = new Vector2(goblin.position.x + deltaX, goblin.position.y + deltaY);
                if (World.GetIntPosition(newPos) == World.GetIntPosition(world.pcPos)){
                    SplashScreens.splashScreens.ShowDeathScreen();
                    AudioSource.PlayClipAtPoint(deathSound, goblin.position);
                    World.world = null; // End level
                }
                world.SetPosition(goblin, newPos);
            }
        }
    }

    public static void SpawnGoblin(int x, int y)
    {
        World world = World.world;
        if (world != null && instance != null)
        {
            Transform goblin = Transform.Instantiate(instance.goblinPrefab, new Vector3(x, y, 0), Quaternion.identity);
            world.SetPosition(goblin, new Vector2(x, y));
            instance.goblins.Add(goblin);
        }
    }
}
