using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public GameObject[] prefabs; // масив обектов для создания их на сцене.
    public Text tKilled;
    public Text tDeaths;

    private int Killed = -1; // щотчик убийст
    private int Deaths = -1;// щотчик смертей
    private int currenMoncters = 1;// количество монстров вживых
    private int MaxStatik = 30;
    private int MaxMonsters = 10;
    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < MaxStatik; j++) // создание припятствий 
        {
            int i = (int)Random.Range(2.0f, 3.9f);
            Instantiate(prefabs[i], new Vector3(Random.Range(10.0f, 100.0f), 0, Random.Range(10.0f, 100.0f)), prefabs[i].transform.rotation);
        }

        CreateMonsters(MaxMonsters);
        SetKilledText();
        SetDeathsText();

    }


     private void CreateMonsters(int x)
    {
        for (int i = 0; i < x; i++)// создание монстров
        {
            Instantiate(prefabs[1], new Vector3(Random.Range(10.0f, 100.0f), 0, Random.Range(10.0f, 100.0f)), prefabs[1].transform.rotation);
            currenMoncters++;
        }
    }
    public void SetKilledText()
    {
        Killed++;
        tKilled.text = "Killed = " + Killed;
        currenMoncters--;
        if(currenMoncters < 1)
        {
            CreateMonsters(MaxMonsters);
        }
    }

    public void SetDeathsText()
    {
        Deaths++;
        tDeaths.text = "Deaths = " + Deaths;
    }
    /* // Update is called once per frame
     void Update()
     {

     }*/
}
