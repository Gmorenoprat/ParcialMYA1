using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntTranslate : MonoBehaviour
{
    public string ID;

    public LangManager manager;

    public Text myView;

    string _myText = "";

    public int value;

    void Awake()
    {
        _myText = myView.GetComponent<Text>().text;
        manager.OnUpdate += ChangeLang;
    }

    void ChangeLang()
    {
        value = int.Parse(manager.GetTranslate(ID));

        myView.text = manager.GetTranslate(ID);
    }
}
