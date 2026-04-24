public static class PlayerSettings
{
    //TODO: Use PlayerPrefs or whatever persistent system to save these values and let the user choose.
    public static float MasterVolume = 0.7f;
    public static float MusicVolume = 0.7f;
    public static float SfxVolume = 0.7f;

    public static string MasterVolumeName = "";
    public static string MusicVolumeName = "";

    public static string SfxVolumeName = "";


    public static void SetSpecificValue(string param, float newValue) //TODO: param should be an enum.
    {
        string lcParam = param.Split(',')[0].ToLower();
        if (lcParam == MasterVolumeName.ToLower())
        {
            MasterVolume = newValue;
            return;
        }
        if (lcParam == MusicVolumeName.ToLower())
        {
            MusicVolume = newValue;
            return;
        }
        if (lcParam == SfxVolumeName.ToLower())
        {
            SfxVolume = newValue;
            return;
        }
    }
    public static float GetSpecificValue(string param)
    {
        string lcParam = param.Split(',')[0].ToLower();
        if (lcParam == MasterVolumeName.ToLower()) return MasterVolume;
        if (lcParam == MusicVolumeName.ToLower()) return MusicVolume;
        if (lcParam == SfxVolumeName.ToLower()) return SfxVolume;
        return 0f;
    }
}

