using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters
{   
    // Player
    public static float playerMinVelocity = 2f;
    public static float playerMaxVelocity = 12f;
    public static float playerVelocityIncrease = 0.1f;
    public static float playerSpeed = 50f;
    public static float playerDeathSpeed = 2f;
    public static float minSpawnTime = 2f;
    public static float maxSpawnTime = 4f;
    public static int PlayerLives = 3;

    // Goals
    public static float goalExplosionMinLife = 3f;
    public static float goalExplosionMaxLife = 20f;
    public static float DebrisSize = 0.2f;
    public static int goalSpawnRate = 2; // Every x normal goals a special will spawn
    public static float starGoalDestructionRadius = 16f;
    public static float deathGoalLifeTime = 10f;

    // Stars
    public static int maxStars = 5;

    // Map
    public static float border = 10;

    // Colours
    public static Color32 orange = new Color32(239, 151, 0, 1);
    public static Color32 blue = new Color32(85, 115, 204, 1);
    public static Color32 purple = new Color32(110, 53, 155, 1);
    public static Color32 green = new Color32(41, 188, 90, 1);
    public static Color32 red = new Color32(204, 64, 0, 1);

    // Goal colour lis
    public static List<Color32> colorList = new List<Color32>()
     {
        blue,
        purple,
        green,
        red
     };

    // HUD
    public static int normalScoreSize = 300;
    public static int largeScoreSize = 400;
    public static float textResetTime = .5f;
    public static float floatingTextLifeTime = 1f;
    public static float menuSpeed = .5f;
    public static int normalMultiplierSize = 120;
    public static int largeMultiplierSize = 200;

    // Scoring
    public static int startPoints = 0;
    public static int goalValue = 1;
    public static int startMultiplier = 1;
 

}
