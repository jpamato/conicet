using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesData : DataLoader
{
    public List<Content> content;
    [HideInInspector] public Content activeContent;

    [System.Serializable]
    public class Content
    {
        public string id;
        public List<string> memotest;
        public List<string> questions;
        public List<string> simons;
        public List<string> fallingObjects;
        public List<string> unir;
        public List<string> loro_repeat;
        public List<string> loro_time;
        public List<string> memotestAudio;
        public List<string> escuchar;
        public List<string> repeat_with_card;
        public List<string> paintings;
        public List<string> elegir_frase;
        public List<string> labyrinth;
        public List<string> rompecabezas;
        public List<string> completar;
        public List<string> letra;
        public List<string> contar;
        public List<string> ordenar;
        public List<string> loro_multiple;

        public List<string> GetContentFor(GameData.types gameType, int id)
        {
            print("GetContentFor gameType: " + gameType + " id: " + id);

            switch (gameType)
            {
                case GameData.types.memotest: return GetTextsById(memotest, id);
                case GameData.types.questions: return GetTextsById(questions, id);

                case GameData.types.simon: return GetTextsById(simons, id);
                case GameData.types.falling_objects: return GetTextsById(fallingObjects, id);

                case GameData.types.unir: return GetTextsById(unir, id);
                case GameData.types.loro_repeat: return GetTextsById(loro_repeat, id);

                case GameData.types.memotest_audio: return GetTextsById(memotestAudio, id);
                case GameData.types.escuchar: return GetTextsById(escuchar, id);
                case GameData.types.repeat_with_card: return GetTextsById(repeat_with_card, id);
                case GameData.types.painting: return GetTextsById(paintings, id);
                case GameData.types.elegir_frase: return GetTextsById(elegir_frase, id);
                case GameData.types.labyrinth: return GetTextsById(labyrinth, id);
                case GameData.types.rompecabezas: return GetTextsById(rompecabezas, id);
                case GameData.types.completar: return GetTextsById(completar, id);
                case GameData.types.letra: return GetTextsById(letra, id);
                case GameData.types.contar: return GetTextsById(contar, id);
                case GameData.types.ordenar: return GetTextsById(ordenar, id);
                case GameData.types.loro_multiple: return GetTextsById(loro_multiple, id);

                default: return GetTextsById(memotestAudio, id);
            }
        }

        // agarra la lista por id de game (por si hay varios el mismo día:
        List<string> GetTextsById(List<string> arr, int id)
        {
            List<string> returnedArr = new List<string>();
            foreach (string s in arr)
            {
                string[] stringArr = s.Split(":"[0]);
                if (stringArr.Length==1 && id == 0)
                {
                    returnedArr.Add(s);
                }
                else if (stringArr.Length >1 && int.Parse(stringArr[1]) == id)
                    returnedArr.Add(stringArr[0]);
            }
            return returnedArr;
        }
    }
    public override void Reset()
    {
        content.Clear();
    }
    public override void OnLoaded(List<SpreadsheetLoader.Line> d)
    {
        OnDataLoaded(content, d);
        base.OnLoaded(d);
    }
    public void SetContent(Content content)
    {
        activeContent = content;
    }
    public Content GetContent(string storyID)
    {
        //print("GetContent " + storyID);
        return content.Find((x) => x.id == storyID);
    }
    void OnDataLoaded(List<Content> content, List<SpreadsheetLoader.Line> d)
    {
        int colID = 0;
        int rowID = 0;
        Content contentLine = null;
        foreach (SpreadsheetLoader.Line line in d)
        {
            foreach (string value in line.data)
            {
                //print("row: " + rowID + "  colID: " + colID + "  value: " + value);
                if (rowID >= 1)
                {
                    if (colID == 0)
                    {
                        if (value != "") // si está vacia la accion usa la anterior:
                        {
                            contentLine = new Content();
                            contentLine.id = value;

                            contentLine.memotest = new List<string>();
                            contentLine.questions = new List<string>();
                            contentLine.simons = new List<string>();
                            contentLine.fallingObjects = new List<string>();
                            contentLine.unir = new List<string>();
                            contentLine.loro_repeat = new List<string>();
                            contentLine.loro_time = new List<string>();
                            contentLine.memotestAudio = new List<string>();
                            contentLine.escuchar = new List<string>();
                            contentLine.repeat_with_card = new List<string>();
                            contentLine.paintings = new List<string>();
                            contentLine.elegir_frase= new List<string>();
                            contentLine.labyrinth = new List<string>();
                            contentLine.rompecabezas = new List<string>();
                            contentLine.completar = new List<string>();
                            contentLine.letra = new List<string>();
                            contentLine.contar = new List<string>();
                            contentLine.ordenar = new List<string>();
                            contentLine.loro_multiple = new List<string>();
                            content.Add(contentLine);
                        }
                    }
                    else
                    {
                        if (colID == 1 && value != "")
                            contentLine.memotest.Add(value);
                        if (colID == 2 && value != "")
                            contentLine.questions.Add(value);
                        if (colID == 3 && value != "")
                            contentLine.simons.Add(value);
                        if (colID == 4 && value != "")
                            contentLine.fallingObjects.Add(value);
                        if (colID == 5 && value != "")
                            contentLine.unir.Add(value);
                        if (colID == 6 && value != "")
                            contentLine.loro_time.Add(value);
                        if (colID == 7 && value != "")
                            contentLine.loro_repeat.Add(value);
                        if (colID == 8 && value != "")
                            contentLine.memotestAudio.Add(value);
                        if (colID == 9 && value != "")
                            contentLine.escuchar.Add(value);
                        if (colID == 10 && value != "")
                            contentLine.repeat_with_card.Add(value);
                        if (colID == 11 && value != "")
                            contentLine.paintings.Add(value);
                        if (colID == 12 && value != "")
                            contentLine.elegir_frase.Add(value);
                        if (colID == 13 && value != "")
                            contentLine.labyrinth.Add(value);
                        if (colID == 14 && value != "")
                            contentLine.rompecabezas.Add(value);
                        if (colID == 15 && value != "")
                            contentLine.completar.Add(value);
                        if (colID == 16 && value != "")
                            contentLine.letra.Add(value);
                        if (colID == 17 && value != "")
                            contentLine.contar.Add(value);
                        if (colID == 18 && value != "")
                            contentLine.ordenar.Add(value);
                        if (colID == 19 && value != "")
                            contentLine.loro_multiple.Add(value);
                    }
                }
                colID++;
            }
            colID = 0;
            rowID++;
        }

    }
}
