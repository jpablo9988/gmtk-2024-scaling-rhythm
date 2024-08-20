using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ScoreTally
{
    private static int perfect;
    private static int good;
    private static int meh;
    private static int globalScore;
    public static int Perfect { get { return perfect; } private set { perfect = value; } }
    public static int Good { get { return good; } private set { good = value; } }
    public static int Meh { get { return meh; } private set { meh = value; } }
    public static int TotalScore { get { return (perfect + good + meh); } private set { meh = value; } }
    public static int SessionScore { get { return globalScore; } private set { } }

    public static void AddToScore(ScoreType type)
    {
        switch (type)
        {
            case ScoreType.Perfect:
                perfect++;
                break;
            case ScoreType.Good:
                good++;
                break;
            case ScoreType.Meh:
                meh++;
                break;
        }
        globalScore++;
    }
    public static void ResetScore()
    {
        perfect = 0;
        good = 0;
        meh = 0;
    }

}

