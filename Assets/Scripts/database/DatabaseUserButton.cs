using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUserButton : MonoBehaviour
{
    [SerializeField] Text namefield;
    [SerializeField] Text ageField;
    [SerializeField] Text sexField;
    DatabaseUser data;

    public void Init(DatabaseUser data)
    {
        this.data = data;
        namefield.text = data.name;
        ageField.text = data.age.ToString();
        if (data.gender == "0")
        {
            sexField.text = "NENE";
            sexField.color = Color.blue;
        }
        else
        {
            sexField.text = "NENA";
            sexField.color = Color.red;
        }

        if (data.text != "")
        {
            namefield.text += "(" + data.text + ")";
        }
    }
    public void OnSelected()
    {
        GetComponentInParent<DatabaseUsersUI>().SelectUser(data);
    }
}
