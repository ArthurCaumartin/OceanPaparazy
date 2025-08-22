using System;

public static class ArrayUtils
{
    public static void LoopIn<T>(this T[,,] array, Action<int, int, int> toDo)
    {
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int z = 0; z < array.GetLength(2); z++)
                {
                    toDo(x, y, z);
                }
            }
        }
    }
}