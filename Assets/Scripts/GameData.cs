using System;

[Serializable] public class GameData
{
    public types type;
    public enum types
    {
        all_days,
        read_automatic,
        memotest,
        unir,
        questions,
        simon,
        falling_objects,
        rimas,
        longWords
    }
    public bool played;
}
