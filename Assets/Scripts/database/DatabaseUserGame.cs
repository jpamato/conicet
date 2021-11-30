using System;
using System.Collections.Generic;

[Serializable]
public class DatabaseUserGame
{
    public int saved;
    public string userID;
    public string gameID;
    public int duration;
    public int correct;
    public int incorrect;

    public void Saved()
    {
        saved = 1;
    }
}
