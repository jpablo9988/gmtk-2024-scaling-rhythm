using Unity.VisualScripting;

public static class ScoreTally
{
    private static int perfect;
    private static int good;
    private static int meh;
    private static int globalScore;
    private static int universalScore;
    public static int Perfect { get { return perfect; } private set { perfect = value; } }
    public static int Good { get { return good; } private set { good = value; } }
    public static int Meh { get { return meh; } private set { meh = value; } }
    public static int TotalScore { get { return (perfect + good + meh); } private set { meh = value; } }
    public static int SessionScore { get { return globalScore; } private set { } }
    public static int AllSessionsScore => universalScore;
    public static bool IsTrackingScore = true;

    public static void AddToScore(ScoreType type)
    {
        if (!IsTrackingScore) return;
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
    public static void ResetScore(bool addToUniversal = true)
    {
        if (addToUniversal) universalScore += globalScore;
        globalScore = 0;
        perfect = 0;
        good = 0;
        meh = 0;
    }
    public static ScoreType ScoreResults(int allBeatCount, int _perfect, int _good, int _meh)
    {
        int _totalScore = _perfect + _good + _meh;
        if (allBeatCount - _perfect <= 5) //The Best Possible Score.
        {
            return ScoreType.Perfect;
        }
        else if (meh <= 2 && _perfect > _meh + _good &&
        allBeatCount - _totalScore <= 10)
        {
            return ScoreType.Good;
        }
        else if (allBeatCount - _totalScore <= 20)
        {
            return ScoreType.Meh;
        }
        else
        {
            return ScoreType.Miss;
        }
    }
    public static ScoreType ScoreResults(int allBeatCount)
    {
        return ScoreResults(allBeatCount, Perfect, Good, Meh);
    }
    public static bool GotPerfectResults(int allBeatCount)
    {
        return allBeatCount == Perfect;
    }

}

