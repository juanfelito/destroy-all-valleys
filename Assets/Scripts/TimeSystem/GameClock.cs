using TMPro;
using UnityEngine;

public class GameClock : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private TextMeshProUGUI dateText = null;
    [SerializeField] private TextMeshProUGUI seasonText = null;
    [SerializeField] private TextMeshProUGUI yearText = null;

    private void OnEnable() {
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    private void OnDisable() {
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    private void UpdateGameTime(Date date) {
        if (date.minute % 10 == 0) {
            string ampm;
            if (date.hour >= 12) {ampm = "pm";} else {ampm = "am";}

            timeText.text = date.hour + ":" + date.minute/10 + "0 " + ampm;
            dateText.text = date.dayOfWeek.ToString() + ". " + date.day;
            seasonText.text = date.season.ToString();
            yearText.text = "Year " + date.year;
        }
    }
}
