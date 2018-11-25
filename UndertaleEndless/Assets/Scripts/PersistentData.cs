using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData
{
    private static int lv, exp;
    private static string name = "TestName";
    private static Vector2 deathLoc;

    public static int Level
    {
        get
        {
            return lv;
        }
        set
        {
            lv = value;
        }
    }

    public static string PlayerName
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public static int Experience
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
        }
    }

    public static Vector2 LastDeathLocation
    {
        get
        {
            return deathLoc;
        }
        set
        {
            deathLoc = value;
        }
    }
}