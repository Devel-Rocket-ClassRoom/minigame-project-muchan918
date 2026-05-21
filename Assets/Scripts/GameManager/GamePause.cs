public static class GamePause
{
    public static bool IsPaused { get; private set; }

    public static void Pause()
    {
        IsPaused = true;
        UnityEngine.Time.timeScale = 0f;
    }

    public static void Resume()
    {
        IsPaused = false;
        UnityEngine.Time.timeScale = 1f;
    }

    public static void Toggle()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }
}
