using System.Collections.Generic;

namespace PowerCreator.LiveClient.Core
{
    public static class FrameRateSetting
    {
        public static Dictionary<int, int> _dictionary = new Dictionary<int, int>()
        {
            { 2000,60},//1280*720
            { 3864,200},//2208*1656
            { 3000,200},//1920*1080
            { 4480,500},//25601920
            { 7728,1000},//44163312
            { 280,40},//160120
            { 560,40},//320240
            { 1410,40},//960450
            { 1328,40},//848480
            { 1000,40},//640360
            { 660,40},//424240
            { 640,40},//352288
            { 1120,40}//640480
        };
        public static int GetCameraFrameRate(int resolution)
        {
            if (_dictionary.ContainsKey(resolution))
            {
                return _dictionary[resolution];
            }
            if (resolution >= 2000)
            {
                return 100;
            }
            return 50;
        }
    }
}
