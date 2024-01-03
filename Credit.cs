using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;


namespace StorybrewScripts
{
    // predefined clock class for clock effect
    public class StoryboardClock
    {
        // private osbsprite
        private OsbSprite ClockPlate;
        private OsbSprite ClockHour;
        private OsbSprite ClockMinute;
        private OsbSprite ClockCentreDot;

        // constructor
        public StoryboardClock(StoryboardLayer layer, string platePath, string hourPath, string dotPath, string minutePath, Vector2 position, OsbOrigin clockOrigin)
        {
            // the clock plate is centre origin
            ClockPlate = layer.CreateSprite(platePath, OsbOrigin.Centre, position);
            // others use clockOrigin since it depends on the image itself
            ClockHour = layer.CreateSprite(hourPath, clockOrigin, position);
            ClockMinute = layer.CreateSprite(minutePath, clockOrigin, position);
            ClockCentreDot = layer.CreateSprite(dotPath, OsbOrigin.Centre, position);
        }

        public void ClockScale(OsbEasing easing, double startTime, double endTime, double startScale, double endScale, double multiplierForDot = 1, double multiplier = 1)
        {
            ClockPlate.Scale(easing, startTime, endTime, startScale, endScale);
            ClockHour.Scale(easing, startTime, endTime, startScale * multiplier, endScale * multiplier);
            ClockMinute.Scale(easing, startTime, endTime, startScale * multiplier, endScale * multiplier);
            ClockCentreDot.Scale(easing, startTime, endTime, startScale * multiplierForDot, endScale * multiplierForDot);
        }

        public void ClockScale(double startTime, double endTime, double startScale, double endScale) => ClockScale(OsbEasing.None, startTime, endTime, startScale, endScale);

        public void ClockTimer(List<double> timingList, List<int> hourList, List<int> minuteList, List<OsbEasing> easing)
        {
            foreach (var time in timingList)
            {
                var indexOfTime = timingList.IndexOf(time);
                if (indexOfTime == timingList.Count - 1) break;

                var hour = hourList[indexOfTime];
                var minute = minuteList[indexOfTime];
                var nextHour = hourList[indexOfTime + 1];
                var nextMinute = minuteList[indexOfTime + 1];
                var nextTime = timingList[indexOfTime + 1];

                var clockAngle = GetClockAngle(hour, minute, nextHour, nextMinute);

                ClockHour.Rotate(easing[indexOfTime % easing.Count], time,  nextTime, clockAngle.Item1, clockAngle.Item3);
                ClockMinute.Rotate(easing[indexOfTime % easing.Count], time, nextTime, clockAngle.Item2, clockAngle.Item4);
            }
        }

        public void ClockFade(OsbEasing easing, double startTime, double endTime, double startOpacity, double endOpacity)
        {
            ClockPlate.Fade(easing, startTime, endTime, startOpacity, endOpacity);
            ClockHour.Fade(easing, startTime, endTime, startOpacity, endOpacity);
            ClockMinute.Fade(easing, startTime, endTime, startOpacity, endOpacity);
            ClockCentreDot.Fade(easing, startTime, endTime, startOpacity, endOpacity);
        }

        public void ClockFade(double startTime, double endTime, double startOpacity, double endOpacity) => ClockFade(OsbEasing.None, startTime, endTime, startOpacity, endOpacity);

        public void ClockAdditive(double startTime, double endTime)
        {
            ClockPlate.Additive(startTime, endTime);
            ClockHour.Additive(startTime, endTime);
            ClockMinute.Additive(startTime, endTime);
            ClockCentreDot.Additive(startTime, endTime);
        }



        private Tuple<double, double, double, double> GetClockAngle(int hour, int minute, int nextHour, int nextMinite)
        {
            var hourAngle = MathHelper.DegreesToRadians((hour % 12) * 30 + minute * 0.5);
            var minuteAngle = MathHelper.DegreesToRadians(minute * 6);
            var nextHourAngle = MathHelper.DegreesToRadians((nextHour % 12) * 30 + nextMinite * 0.5);
            var nextMinuteAngle = MathHelper.DegreesToRadians(nextMinite * 6.0);
            // need to consider the period.
            var deltaHour = nextHour - hour;
            nextMinuteAngle += deltaHour * 2d * Math.PI;
            return new Tuple<double, double, double, double>(hourAngle, minuteAngle, nextHourAngle, nextMinuteAngle);
        }

    }

    public class Credit : StoryboardObjectGenerator
    {
        [Configurable]
        public string FontName = "Hollirood-Personal use";
        [Configurable]
        public string FontPath = "sb/fonts";
        [Configurable]
        public int FontSize = 26;
        [Configurable]
        public Vector2 Padding = Vector2.Zero;
        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;
        [Configurable]
        public Color4 FontColor = Color4.White;
        public override void Generate()
        {
            var beatDuration = Beatmap.GetTimingPointAt(0).BeatDuration;
            var whitePath = @"sb\white.png";
            var whiteHeight = GetMapsetBitmap(whitePath).Height;

            var ludicinStartTime = 1348;
            var ludicinEndTime = 4090;


            // show the credit things
            var creditLayer = GetLayer("Credit");

            var ludicinPath = @"sb\Ludicin\Ludicin_.png";
            var ludicin = creditLayer.CreateAnimation(ludicinPath, 20, 1000 / 20, OsbLoopType.LoopOnce, OsbOrigin.Centre);
            ludicin.Fade(OsbEasing.OutSine, ludicinStartTime, ludicinStartTime + 1000, 0, 1);
            ludicin.Fade(OsbEasing.OutSine, ludicinEndTime - 1000, ludicinEndTime, 1, 0);
            ludicin.Scale(OsbEasing.InOutSine, ludicinStartTime, ludicinEndTime, 0.4, 1);
            ludicin.Additive(ludicinStartTime, ludicinEndTime);


            // font config
            var font = LoadFont($"{FontPath}/{FontName}", new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = System.Drawing.FontStyle.Regular,
                TrimTransparency = false,
                EffectsOnly = false,
            });

            var mapsetStartTime = 5462;
            var mapsetEndTime = 12319;
            generatePerCharacter(font, creditLayer, true, new Vector2(320, 210), "Map", mapsetStartTime, mapsetEndTime, 100, 0.2f);

            // the author should be divided considering map
            var authorLayer = GetLayer("Author");
            var mapperName = "Critical_Star";
            if (Beatmap.Name.Contains("Infinity"))
            {
                mapperName = "Critical_Star & Schopfer";
            }
            else if (Beatmap.Name.Contains("Solitude"))
            {
                mapperName = "Critical_Star & tyrcs";
            }

            generatePerCharacter(font, authorLayer, true, new Vector2(320, 240), mapperName, mapsetStartTime, mapsetEndTime, 30, 0.3f);

            var storyboardStartTime = 16433;
            var storyboardEndTime = 21747;
            generatePerCharacter(font, creditLayer, true, new Vector2(320, 210), "Storyboard", storyboardStartTime, storyboardEndTime, 60, 0.2f);
            generatePerCharacter(font, creditLayer, true, new Vector2(320, 240), "RiceSS", storyboardStartTime, storyboardEndTime, 100, 0.3f);

            // a noise is shown when the drum appears
            ShowNoise(beatDuration, creditLayer);

            // the big drum shown at 27405
            var drumStartTime = 27405;
            var drumEndTime = drumStartTime + 4 * beatDuration;
            var blackFadeTime = drumEndTime + 4 * beatDuration;
            var black = creditLayer.CreateSprite(whitePath);
            black.ScaleVec(drumStartTime, 854.0f / whiteHeight, 480.0f / whiteHeight);
            black.Fade(drumStartTime, drumEndTime, 1, 1);
            black.Fade(OsbEasing.InSine, drumEndTime, blackFadeTime, 1, 0);
            black.Color(drumStartTime, Color4.Black);

            generatePerCharacter(font, creditLayer, true, new Vector2(320, 210), "Hitsound", drumStartTime + 200, blackFadeTime, 0, 0.2f);
            generatePerCharacter(font, creditLayer, true, new Vector2(320, 240), "Mirsaaa & Hidden is fun", drumStartTime + 200, blackFadeTime, 0, 0.3f);

            // next let the clock show!
            var clock = new StoryboardClock(GetLayer("Clock"), @"sb\clock\roman clock.png", @"sb\clock\ch2.png", @"sb\clock\c.png", @"sb\clock\ch1.png", new Vector2(320, 240), OsbOrigin.BottomCentre);

            var clockStartTime = 30148;

            clock.ClockFade(OsbEasing.InSine, clockStartTime, clockStartTime + 4 * beatDuration, 0, 0.4);
            clock.ClockScale(OsbEasing.InOutSine, clockStartTime, clockStartTime + 4 * beatDuration, 0.3, 0.5, 0.4, 0.4);

            // generate the clock timer
            var timingList = new List<double> {};
            var hourList = new List<int> {};
            var minuteList = new List<int> {};

            var rotateStartTime = 33576;
            var rotateEndTime = 49347;
            int i = 0;
            for (double time = rotateStartTime; time < rotateEndTime; time += beatDuration * 4)
            {
                timingList.Add(time);
                
                var hour = i * 5 / 60;
                var minute = i * 5 % 60;
                hourList.Add(hour);
                minuteList.Add(minute);
                i++;
            }

            clock.ClockTimer(timingList, hourList, minuteList, new List<OsbEasing> {OsbEasing.OutElastic});

            var clockFadeTime = 45233;
            clock.ClockFade(OsbEasing.OutSine, clockFadeTime, clockFadeTime + 10 * beatDuration, 0.4, 0);
            clock.ClockAdditive(clockStartTime, clockFadeTime + 10 * beatDuration);
            
            var titlePath = @"sb\title\title_.png";
            var title = creditLayer.CreateAnimation(titlePath, 45, 1500 / 45, OsbLoopType.LoopOnce, OsbOrigin.Centre);
            title.Fade(OsbEasing.InOutSine, 46605, 46605+beatDuration, 0, 1);
            title.Scale(OsbEasing.InCubic, 46605, 46605 + 1500, 0.9, 0.6);
            title.Fade(OsbEasing.InOutSine, 48319, 48319 + beatDuration, 1, 0);

            var titleShadowPath = @"sb\title\title_44.png";
            var titleShadow = creditLayer.CreateSprite(titleShadowPath, OsbOrigin.Centre);
            titleShadow.Scale(OsbEasing.OutCubic,46605 + 1500 - 100, 48319, 0.6, 0.7);
            titleShadow.Fade(OsbEasing.OutSine, 46605 + 1500 - 100, 46605 + 1500 + beatDuration, 0.3, 0);

            generatePerCharacter(font, creditLayer, true, new Vector2(320, 290), "Beat Saber World Cup 2023 Original", 46605, 48319 + 100, 10, 0.2f);
            var sideBar = creditLayer.CreateSprite(@"sb\side_bar.png", OsbOrigin.Centre, new Vector2(320, 320));
            sideBar.Fade(46605, 46605+100, 0, 0.8);
            sideBar.Additive(46605, 48319 + 100);
            sideBar.ScaleVec(OsbEasing.OutSine, 46605, 46605+300, 0.8, 0, 0.8, 0.8);
            sideBar.Rotate(46605, -Math.PI/2);
            sideBar.Fade(48319, 48319+170, 0.8, 0);

            // in 383460 the mapper wants to also use clock
            var clock2 = new StoryboardClock(GetLayer("Clock"), @"sb\clock\roman clock.png", @"sb\clock\ch2.png", @"sb\clock\c.png", @"sb\clock\ch1.png", new Vector2(320, 240), OsbOrigin.BottomCentre);
            clock2.ClockFade(OsbEasing.InSine, 383460, 383460 + 1 * beatDuration, 0, 0.4);
            clock2.ClockScale(OsbEasing.None, 383460, 383460 + 1 * beatDuration, 0.5, 0.5, 0.4, 0.4);

            rotateStartTime = 395137;
            rotateEndTime  = 406988;
            int rotateTimes = (int)Math.Floor((rotateEndTime-rotateStartTime)/beatDuration);    // though it is not so correct but for now it works at least

            timingList.Clear();
            hourList.Clear();
            minuteList.Clear();
            
            timingList.Add(383460);
            timingList.Add(386889);
            timingList.Add(394432);
            hourList.Add(20);
            hourList.Add(0);
            hourList.Add(0);
            minuteList.Add(30);
            minuteList.Add((rotateTimes) * 5 % 60 + 30);
            minuteList.Add((rotateTimes) * 5 % 60);
            clock2.ClockTimer(timingList, hourList, minuteList, new List<OsbEasing>{OsbEasing.OutSine, OsbEasing.InSine});
            timingList.Clear();
            hourList.Clear();
            minuteList.Clear();

            

            var currentTime = (double)rotateStartTime - beatDuration * 2;
            for (int j=0;j<rotateTimes;j++)
            {
                var beatDuration2 = Beatmap.GetTimingPointAt((int)currentTime).BeatDuration;
                

                timingList.Add(currentTime  + beatDuration2 * 2);
                var hour = 0;
                var minute = (rotateTimes - j) * 5 % 60;
                hourList.Add(hour);
                minuteList.Add(minute);

                currentTime += 4 * beatDuration2;
            }

            clock2.ClockTimer(timingList, hourList, minuteList,new List<OsbEasing> {OsbEasing.OutElastic});
            
            clock2.ClockFade(OsbEasing.OutSine, 401533, 407787, 0.4, 0);
        }

        private void ShowNoise(double beatDuration, StoryboardLayer creditLayer)
        {
            var noiseOpacity = 0.3f;
            var noiseTime = 12319;
            var noise = creditLayer.CreateAnimation(@"sb\noise\noise_.png", 4, 100 / 4, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var noiseBitmap = GetMapsetBitmap(@"sb\noise\noise_0.png");
            noise.Scale(noiseTime, 480.0f / noiseBitmap.Height);
            noise.Additive(noiseTime);

            var noiseStartTime = 12319;
            var noiseEndTime = 12319 + 1 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 16005;
            noiseEndTime = 16262;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 21747;
            noiseEndTime = 22090;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 22262;
            noiseEndTime = 22776;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 30148;
            noiseEndTime = noiseStartTime + 4 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 39062;
            noiseEndTime = noiseStartTime + 1 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 40433;
            noiseEndTime = noiseStartTime + 1 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 41747;
            noiseEndTime = noiseStartTime + 1 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);

            noiseStartTime = 43062;
            noiseEndTime = noiseStartTime + 1 * beatDuration;
            noise.Fade(noiseStartTime, noiseEndTime, noiseOpacity, noiseOpacity);
            noise.Fade(noiseEndTime, 0);


        }

        public void generatePerCharacter(FontGenerator font, StoryboardLayer layer, bool additive, Vector2 textPosition, string text, double startTime, double endTime, double intervalTime, float fontScale)
        {
            var lineWidth = 0f;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * fontScale;
            }

            var letterX = textPosition.X - lineWidth * 0.5f;

            int i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, textPosition.Y)
                        + texture.OffsetFor(Origin) * fontScale;
                    var realStartTime = startTime + i * intervalTime;
                    var realEndTime = endTime + i * intervalTime;

                    var sprite = layer.CreateSprite(texture.Path, Origin, position);
                    sprite.Scale(realStartTime, fontScale);
                    sprite.Fade(realStartTime - 200, realStartTime, 0, 1);
                    sprite.Fade(realEndTime - 200, realEndTime, 1, 0);
                    if (additive) sprite.Additive(realStartTime - 200, realEndTime);
                    if (FontColor != Color4.White) sprite.Color(realStartTime, FontColor);
                }
                letterX += texture.BaseWidth * fontScale;
                i++;
            }
        }
    }
}
