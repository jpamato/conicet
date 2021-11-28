using System;

[Serializable]
public class DatabaseUser
{
    public string id;
    public string name;
    public int age;
    public string text;
    public string sex; // nene, nena

    public void GenerateID()
    {
        id = DateTime.Today.Year.ToString();
        id += DateTime.Today.Month.ToString();
        id += DateTime.Today.Day.ToString();
        id += DateTime.Today.Hour.ToString();
        id += DateTime.Today.Minute.ToString();
        id += DateTime.Today.Second.ToString();
        id += DateTime.Today.Millisecond.ToString();
        id += (UnityEngine.Random.Range(0, 1000)).ToString();
    }
}
