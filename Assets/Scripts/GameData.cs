using System;

[Serializable] public class GameData
{
    public types type;
    public enum types
    {
        read_automatic,
        memotest,
        unir
    }
    public int played;
    public int stars;
}
