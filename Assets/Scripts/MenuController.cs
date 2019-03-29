using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> pageList = new List<GameObject>();
    [SerializeField] private GameObject levelButtonPrefab;
    private int page = 0;

    private void Awake()
    {
        pageList[0].SetActive(true);
        CreateLevelsPage();
    }

    public void SetActivePage(int index)
    {
        foreach (var page in pageList)
        {
            page.SetActive(false);
        }
        pageList[index].SetActive(true);
    }

    private void CreateLevelsPage()
    {
        //i - номер сцены-уровня 
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            float x, y;
            x = -350+150*(i-1);
            y = -250;

            GameObject go = Instantiate(levelButtonPrefab);
            go.transform.parent = pageList[1].transform;


            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,y);

            var button = go.GetComponent<Button>();
            go.GetComponentInChildren<Text>().text = i.ToString();
            button.onClick.AddListener(() => SetLevelForButton(int.Parse(go.GetComponentInChildren<Text>().text)));
        }
    }

    private void SetLevelForButton(int index)
    {
        SceneManager.LoadScene(index);
    }
}
