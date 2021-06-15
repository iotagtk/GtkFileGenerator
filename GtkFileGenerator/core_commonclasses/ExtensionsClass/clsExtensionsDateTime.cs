using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

    public static class DateTimeExtensions {


    /// <summary>
    /// 日曜日始まりの場合
    /// </summary>
    public static DateTime _getThisWeek_sunday(this DateTime d) {
        return d.AddDays(DayOfWeek.Sunday - d.DayOfWeek);
    }

    /// <summary>
    /// 月曜日始まりの場合
    /// </summary>
    public static DateTime _getThisWeek_monday(this DateTime d) {
        int diff = DayOfWeek.Monday - d.DayOfWeek;
        if (diff > 0)
            diff -= 7;
        return d.AddDays(diff);

    }

    /// <summary>
    /// 時間を比較する
    /// </summary>
    public static int _compareTime(this DateTime time, DateTime nowTime) {

        int result = -1;
        switch (time.CompareTo(nowTime)) {
            case -1:
               // Console.WriteLine(" time は nowTime より古い -1");
                result = -1;
                break;
            case 0:
              //  Console.WriteLine(" time と nowTime は等しい 0");
                result = 0;
                break;
            case 1:
              //  Console.WriteLine(" time は nowTime より新しい 1");
                result = 1;
                break;
        }
        return result;

    }


    /// <summary>
    /// タイムスタンプをDateTimeに変換する
    /// </summary>
    public static DateTime _unixTimeStampToDateTime(this long unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }


    /// <summary>
    /// DateTimeをタイムスタンプに変換する
    /// </summary>
    public static long _mkUnixTimeStamp(this DateTime targetTime_t) {

        DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // UTC時間に変換
        DateTime targetTime = targetTime_t.ToUniversalTime();

        // UNIXエポックからの経過時間を取得
        TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

        //経過秒数に変換
        return (long)elapsedTime.TotalSeconds;
    }


    /// <summary>
    /// 何年後　+-
    /// </summary>
    public static DateTime _addYears(this DateTime date, int year) {
       return date.AddYears(year);
    }

    /// <summary>
    /// 何ヵ月後　+-
    /// </summary>
    public static DateTime _addMonths(this DateTime date, int months) {
        return date.AddMonths(months);
    }

    /// <summary>
    /// 何日後　+-
    /// </summary>
    public static DateTime _addDays(this DateTime date, int days) {
        return date.AddDays(days);
    }
    /// <summary>
    /// 何時間後　+-
    /// </summary>
    public static DateTime _addHours(this DateTime date, int hours) {
        return date.AddHours(hours);
    }
    /// <summary>
    /// 何分後　+-
    /// </summary>
    public static DateTime _addMinutes(this DateTime date, int minitus) {
        return date.AddMinutes(minitus);
    }
    

                   


}


