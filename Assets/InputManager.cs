using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject playerCharacter;

    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        World world = World.world;
        if (world != null)
        {
            Vector2 pos = world.pcPos;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) pos.x -= 1;
            if (Input.GetKeyDown(KeyCode.RightArrow)) pos.x += 1;
            if (Input.GetKeyDown(KeyCode.DownArrow)) pos.y -= 1;
            if (Input.GetKeyDown(KeyCode.UpArrow)) pos.y += 1;
            if (world.simpleMap[(int)pos.x, (int)pos.y] == "wall") pos = world.pcPos; // Don't move.
            if (world.simpleMap[(int)pos.x, (int)pos.y] == "spikes")
            {
                SplashScreens.splashScreens.ShowDeathScreen();
                AudioSource.PlayClipAtPoint(deathSound, playerCharacter.transform.position);
                World.world = null; // End level
            }
            if (world.simpleMap[(int)pos.x, (int)pos.y] == "down")
            {
                World.world = null;
                SplashScreens.splashScreens.TryStartLevel(world.currentLevel + 1);
            }
            world.pcPos = pos;

            playerCharacter.transform.position = new Vector3(world.pcPos.x, world.pcPos.y, 0);
        }
    }
}
