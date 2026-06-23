using System;

public static class GameEvents
{
    // 동물 처치 시 발행
    public static event Action AnimalKilled;
    public static void RaiseAnimalKilled() => AnimalKilled?.Invoke();
}