using System;

[Serializable] public class GameData
{
    public int gameID; // por si hay varios un mismo día
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
        loro_time,
        loro_repeat,
        endDay,
        memotest_audio,
        escuchar,
        repeat_with_card,
        books,
        painting,
        elegir_frase
    }
    public bool played;
    public string tip_id;

    public void SetPlayed(bool isPlayed)
    {
        this.played = isPlayed;
    }
}
