using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TimerController : MonoBehaviour
{
    [SerializeField] private Text timerMinuteText;
    [SerializeField] private Text timerSecondext;
    [SerializeField] private Text timerMsecondText;
    [SerializeField] private Text bestMinuteText;
    [SerializeField] private Text bestSecondext;
    [SerializeField] private Text bestMsecondText;
    [SerializeField] private Text worldBestMinuteText;
    [SerializeField] private Text worldBestSecondext;
    [SerializeField] private Text worldBestMsecondText;
    [SerializeField] private Text messageText;
    [SerializeField] private string newRecordString;
    [SerializeField] private string lapWithoutRecodString;
    [SerializeField] private string startString;

    private int _minute, _second ,_msecond;
    private int _bestMinute, _bestSecond, _bestMsecond;
    private float timeCounter=0.01f;

    private float messageTimer = 0f;
    private bool showMessage = false;

    private bool start = false;
    private bool end = false;
    private Race bestWorldTime;

    void Awake()
    {
        _bestMinute = PlayerPrefs.GetInt("bestminute");
        _bestSecond = PlayerPrefs.GetInt("bestsecond");
        _bestMsecond = PlayerPrefs.GetInt("bestmsecond");
        bestWorldTime = new Race();
        StartCoroutine(GetRequest("http://sarafun.info:4200/race/getbesttime?trackid=1"));
    }

    void ShowMessage(string message)
    {
        showMessage = true;
        if (message != startString)
        {
            message = timerMinuteText.text + " : " + timerSecondext.text + " : " + timerMsecondText.text + " " + message;
        }
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }
    void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }

    public void StartLap()
    {
        _minute = 0;
        _second = 0;
        _msecond = 0;
        timeCounter = 0.01f;
        start = true;
        if (start && !end)
        {
            RestartScene();
        }
        end = false;
        if (_bestMinute.Equals(0) && _bestSecond.Equals(0) && _bestMsecond.Equals(0))
        {
            ShowMessage(startString);
        }
    }


    void SaveBestTime()
    {
        PlayerPrefs.SetInt("bestminute", _bestMinute);
        PlayerPrefs.SetInt("bestsecond", _bestSecond);
        PlayerPrefs.SetInt("bestmsecond", _bestMsecond);
        PlayerPrefs.Save();
    }
    public void EndLap()
    {
        int bestSum = _bestMsecond + _bestSecond * 60 + _bestMinute * 60 * 60;
        int curSum = _msecond + _second * 60 + _minute * 60 * 60;
        int worldSum = (int)bestWorldTime.Bestmsecond + (int)bestWorldTime.Bestsecond * 60 + (int)bestWorldTime.Bestminute * 60 * 60;
        end = true;
        if (curSum < 120) return;
        if (_bestMinute.Equals(0)&& _bestSecond.Equals(0) && _bestMsecond.Equals(0))
        {
            _bestMinute = _minute;
            _bestSecond = _second;
            _bestMsecond = _msecond;
            SaveBestTime();
            if (start)
            {
                ShowMessage(newRecordString);
                if (curSum < worldSum) SetNewWorldRecord();
            }
        }
        else
        {
            if (start)
            {
                if (curSum < bestSum)
                {
                    _bestMinute = _minute;
                    _bestSecond = _second;
                    _bestMsecond = _msecond;
                    ShowMessage(newRecordString);
                    SaveBestTime();
                    Debug.Log(curSum + " " + worldSum);
                    if (curSum < worldSum) SetNewWorldRecord();
                }
                else
                {
                    ShowMessage(lapWithoutRecodString);
                }
            }
        }
    }
    
    void Update()
    {
        if (showMessage)
        {
            messageTimer += Time.deltaTime;
            if (messageTimer >= 2)
            {
                HideMessage();
                showMessage = false;
                messageTimer = 0;
            }
        }
        if (start)
        {
            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0)
            {
                timeCounter = 0.01f;
                _msecond++;
            }
            if (_msecond >= 60)
            {
                _msecond = 0;
                _second++;
            }
            if (_second == 60)
            {
                _second = 0;
                _minute++;
            }
        }

        string minute;
        if (_minute < 10) minute = "0" + _minute.ToString(); else minute = _minute.ToString();
        timerMinuteText.text = minute;
        string second;
        if (_second < 10) second = "0" + _second.ToString(); else second = _second.ToString();
        timerSecondext.text = second;
        string msecond;
        if (_msecond < 10) msecond = "0" + _msecond.ToString(); else msecond = _msecond.ToString();
        timerMsecondText.text = msecond;

        if (_bestMinute < 10) minute = "0" + _bestMinute.ToString(); else minute = _bestMinute.ToString();
        bestMinuteText.text = minute;
        if (_bestSecond < 10) second = "0" + _bestSecond.ToString(); else second = _bestSecond.ToString();
        bestSecondext.text = second;
        if (_bestMsecond < 10) msecond = "0" + _bestMsecond.ToString(); else msecond = _bestMsecond.ToString();
        bestMsecondText.text = msecond;

        if (bestWorldTime.Bestminute < 10) minute = "0" + bestWorldTime.Bestminute.ToString(); else minute = bestWorldTime.Bestminute.ToString();
        worldBestMinuteText.text = minute;
        if (bestWorldTime.Bestsecond < 10) second = "0" + bestWorldTime.Bestsecond.ToString(); else second = bestWorldTime.Bestsecond.ToString();
        worldBestSecondext.text = second;
        if (bestWorldTime.Bestmsecond < 10) msecond = "0" + bestWorldTime.Bestmsecond.ToString(); else msecond = bestWorldTime.Bestmsecond.ToString();
        worldBestMsecondText.text = msecond;
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            Debug.Log(uri);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                if (!(webRequest.downloadHandler.text.Replace("\"","")=="succesful" || webRequest.downloadHandler.text.Replace("\"", "") == "pizdish" || webRequest.downloadHandler.text.Replace("\"", "") == "trying set empty record"))
                {
                    bestWorldTime = JsonUtility.FromJson<Race>(webRequest.downloadHandler.text);

                    int bestSum = _bestMsecond + _bestSecond * 60 + _bestMinute * 60 * 60;
                    int worldSum = (int)bestWorldTime.Bestmsecond + (int)bestWorldTime.Bestsecond * 60 + (int)bestWorldTime.Bestminute * 60 * 60;
                    Debug.Log(bestSum+" "+ worldSum);
                    if (!(_bestMinute.Equals(0) && _bestSecond.Equals(0) && _bestMsecond.Equals(0)) && (bestSum < worldSum || worldSum.Equals(0)))
                    {
                        Debug.Log("start new record");
                        SetNewWorldRecord();
                    }
                }
            }
        }
    }

    void SetNewWorldRecord()
    {
        bestWorldTime.Bestminute = _bestMinute;
        bestWorldTime.Bestsecond = _bestSecond;
        bestWorldTime.Bestmsecond = _bestMsecond;
        StartCoroutine(GetRequest("http://sarafun.info:4200/race/putbesttime?trackid=1&bestminute="+_bestMinute.ToString()+ "&bestsecond=" + _bestSecond.ToString() + "&bestmsecond=" + _bestMsecond.ToString()));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
