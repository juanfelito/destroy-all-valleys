using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager> {
    private Date date = new() {
        year = 1,
        season = Season.Spring,
        day = 1,
        dayOfWeek = DayOfTheWeek.Mon,
        hour = 6,
        minute = 30,
        second = 0
    };

    private bool gameClockPaused = false;
    private float gameTick = 0f;

    private void Start() {
        EventHandler.CallAdvanceGameMinuteEvent(date);
    }

    private void Update() {
        if (!gameClockPaused) {
            GameTick();
        }
    }

    private void GameTick() {
        gameTick += Time.deltaTime;

        while (gameTick >= Settings.secondsPerGameSecond) {
            gameTick -= Settings.secondsPerGameSecond;

            UpdateGameSecond();
        }
    }

    private void UpdateGameSecond() {
        date.second++;

        if (date.second > 59) {
            date.second = 0;
            date.minute++;

            if (date.minute > 59) {
                date.minute = 0;
                date.hour++;

                if (date.hour > 23) {
                    date.hour = 0;
                    date.day++;
                    date.dayOfWeek = GetDayOfWeek();

                    if (date.day > 30) {
                        date.day = 1;
                        
                        int gs = (int)date.season;
                        gs++;
                        date.season = (Season)gs;

                        if (gs > 3) {
                            date.season = Season.Spring;
                            date.year++;

                            if (date.year > 9999) { date.year = 1; }

                            EventHandler.CallAdvanceGameYearEvent(date);
                        }

                        EventHandler.CallAdvanceGameSeasonEvent(date);
                    }
                    EventHandler.CallAdvanceGameDayEvent(date);
                }
                EventHandler.CallAdvanceGameHourEvent(date);
            }
            EventHandler.CallAdvanceGameMinuteEvent(date);
        }
    }

    private DayOfTheWeek GetDayOfWeek() {
        int totalDays = (((int)date.season) * 30) + date.day + (120 * (date.year - 1));

        int dayOfWeek = totalDays % 7;

        return (DayOfTheWeek)dayOfWeek;
    }

    //TODO:Remove
    /// <summary>
    /// Advance 1 game minute
    /// </summary>
    public void TestAdvanceGameMinute() {
        for (int i = 0; i < 60; i++)
        {
            UpdateGameSecond();
        }
    }

    //TODO:Remove
    /// <summary>
    /// Advance 1 day
    /// </summary>
    public void TestAdvanceGameDay() {
        for (int i = 0; i < 86400; i++)
        {
            UpdateGameSecond();
        }
    }
} 