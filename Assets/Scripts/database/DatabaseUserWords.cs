using System;
using System.Collections.Generic;

[Serializable]
public class DatabaseUserWords
{
    public int saved;
    public string gameID;
    public string word;
    public int correct;

    public void Saved()
    {
        saved = 1;
    }
}
