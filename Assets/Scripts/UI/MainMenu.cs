using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image settings;
    [SerializeField]
    RecyclingListView theList;
    List<ControlsInfo> controlPanels;
    private void Start()
    {
        controlPanels = new List<ControlsInfo>();
    }
    public void OpenSettings()
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(true);
            UpdateKeys();
        }
    }
    public void CloseSettings() 
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(false);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    void UpdateKeys()
    {
        List<ControlsInfo> controls = new List<ControlsInfo>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new ControlsInfo() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Action = scoreboard.Rows[i][1].ToString(), Key = scoreboard.Rows[i][2].ToString() };
            info.OnChangeKey.AddListener(ChangeButton);
            controls.Add(info);
        }
        RetrieveData(controls);
    }
    public void RetrieveData(List<ControlsInfo> panels)
    {
        theList.ItemCallback = PopulateItem;
        controlPanels.Clear();
        if (panels.Count > 0)
        {
            controlPanels.AddRange(panels);
        }
        theList.RowCount = panels.Count;
        theList.Refresh();
    }
    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as Controlsitem;
        child.ControlsInfo = controlPanels[rowIndex];
    }
    void ChangeButton(int id)
    {
        StartCoroutine(WaitForKeyPress(id));
    }
    public void Apply()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new ControlsInfo() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Action = scoreboard.Rows[i][1].ToString(), Key = scoreboard.Rows[i][2].ToString() };
            if (!controlPanels[i].Equals(info) && controlPanels[i].Id == info.Id)
            {
                DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsControls SET Button = '{controlPanels[i].Key}' Where Id = {info.Id}");
            }
        }
        UpdateKeys();
    }
    IEnumerator WaitForKeyPress(int id)
    {
        var toChange = controlPanels.FirstOrDefault(p => p.Id == id);
        toChange.Key = "-";
        RetrieveData(new List<ControlsInfo>(controlPanels));
        // Ожидаем нажатия клавиши
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Проверяем, какая клавиша была нажата
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {            
                string keyStr = key.ToString();
                if (key == KeyCode.Escape)
                {
                    keyStr = "-";
                }
                toChange.Key = keyStr;
                RetrieveData(new List<ControlsInfo>(controlPanels));
                break;
            }
        }
    }

}
