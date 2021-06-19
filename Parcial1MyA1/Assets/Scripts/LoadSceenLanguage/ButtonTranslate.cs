using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTranslate : MonoBehaviour
{
    public string ID;

    public LangManager manager;

    public Text myView;

    string _myText = "";

    void Awake()
    {
        _myText = myView.GetComponent<Text>().text;

        if (!manager)
        {
            manager = GameObject.Find("LanguageManager").GetComponent<LangManager>().GetInstance;
        }
            
       manager.OnUpdate += ChangeLang;
        
    }
    private void Start()
    {
        try { ChangeLang(); }
        catch { return; }
    }


    void ChangeLang()
    {
        myView.text = manager.GetTranslate(ID);
    }

    private void OnDestroy()
    {
        manager.OnUpdate -= ChangeLang;
    }
}
