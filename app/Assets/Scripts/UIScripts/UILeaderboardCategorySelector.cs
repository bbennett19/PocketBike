using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderboardCategorySelector : MonoBehaviour {
    public GameObject leaderboardLinePrefab;
    public Text noConnectionText;
    public Button[] buttons;
    public int[] buttonToLevelIDMapping;
    public int defaultSelected = 0;

    private int _currentSelected = 0;

    // Use this for initialization
    void Start()
    {
        _currentSelected = defaultSelected;
        buttons[_currentSelected].interactable = false;
        noConnectionText.enabled = false;
    }

    private void OnEnable()
    {
        CategoryClicked(_currentSelected);
    }

    public void CategoryClicked(int category)
    {
        buttons[_currentSelected].interactable = true;
        _currentSelected = category;
        buttons[_currentSelected].interactable = false;

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        noConnectionText.enabled = false;
        NetworkOperations.Instance.GetHighScores(GotHighScores, buttonToLevelIDMapping[_currentSelected]);
    }

    private void GotHighScores(bool success, List<BestTimeData> data)
    {
        if(success)
        {
            int place = 1;
            foreach (BestTimeData item in data)
            {
                GameObject line = Instantiate(leaderboardLinePrefab, this.transform);
                string label = "s";

                if(buttonToLevelIDMapping[_currentSelected] == -1)
                {
                    label = "miles";
                }

                line.GetComponent<LeaderboardElement>().SetFields(item.PLAYER_id, place++, item.name, item.time, label);
            }
        }
        else
        {
            noConnectionText.enabled = true;
            Debug.Log("Error");
        }
    }
}
