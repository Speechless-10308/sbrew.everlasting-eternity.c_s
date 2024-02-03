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
    public class BuildUpP1 : StoryboardObjectGenerator
    {
        [Group("Colors")]
        [Configurable]
        public Color4 SeaColor = Color4.Blue;
        [Configurable]
        public Color4 RomaColor = Color4.White;
        [Configurable]
        public Color4 LightColor = Color4.WhiteSmoke;

        [Group("FlarePosition")]
        [Configurable]
        public Vector2 FlarePosition = new Vector2(220, -10);
        [Group("Timing")]
        [Configurable]
        public int LoopLength = 16;

        public override void Generate()
        {
            string seaPath = @"sb\sea.jpg";
            string bubblePath = @"sb\bubble.png";
            string flarePath = @"sb\flare.png";
            string lightPath = @"sb\light.png";
            string furinaPath = @"sb\Character Effect\furina.png";
            string furinaGrayPath = @"sb\Character Effect\furina_gray.png";
            string FurinaLightBurstPath = @"sb\Character Effect\furina_light_burst.png";

            var bgLayer = GetLayer("BG");
            var furniaLightBurstLayer = GetLayer("FurinaLightBurst");
            var furinaLayer = GetLayer("Furina");
            var effectLayer = GetLayer("Effect");

            var seaBitmap = GetMapsetBitmap(seaPath);
            var bubbleBitmap = GetMapsetBitmap(bubblePath);
            var flareBitmap = GetMapsetBitmap(flarePath);
            var lightBitmap = GetMapsetBitmap(lightPath);
            var furinaBitmap = GetMapsetBitmap(furinaPath);

            var beatDuration = Beatmap.GetTimingPointAt(0).BeatDuration;

            var bgShowTime = 49347;
            var bgStartShowTime = 54833;
            var bgEndShowTime = 59976;
            var sea = bgLayer.CreateSprite(seaPath, OsbOrigin.Centre);
            sea.Fade(OsbEasing.InOutSine, bgShowTime, bgStartShowTime, 0, 0.5);
            sea.Scale(bgShowTime, 1.05 * 480.0f / seaBitmap.Height);
            sea.Color(bgShowTime, SeaColor);
            sea.Fade(bgEndShowTime - 100, bgEndShowTime, 0.5, 0);
            BGFlowRand(sea, bgShowTime, bgEndShowTime, 8, new Vector2(320, 240), OsbEasing.InOutSine, OsbEasing.InOutSine);

            var furinaShowTime = bgShowTime;
            var furinaEndShowTime = bgEndShowTime;
            var furinaStartShowTime = bgStartShowTime;

            var furinaGray = furinaLayer.CreateSprite(furinaGrayPath, OsbOrigin.Centre, new Vector2(500, 210));
            furinaGray.Fade(OsbEasing.InOutSine, furinaShowTime, furinaStartShowTime, 0, 0.1);
            furinaGray.Scale(furinaShowTime, 600.0f / furinaBitmap.Height);
            furinaGray.Fade(furinaEndShowTime - 100, furinaEndShowTime, 0.1, 0);
            furinaGray.Additive(furinaShowTime);

            var furina = furinaLayer.CreateSprite(furinaPath, OsbOrigin.Centre);
            furina.Fade(OsbEasing.InOutSine, furinaShowTime, furinaStartShowTime, 0, 1);
            furina.Scale(furinaShowTime, 480.0f / furinaBitmap.Height);
            furina.Fade(furinaEndShowTime - 100, furinaEndShowTime, 1, 0);

            var romaPath = "sb/roma/";
            romaParticles(romaPath, bgShowTime, bgEndShowTime, 100, new Vector2(50, 510), 30, OsbEasing.InOutSine);
            romaParticles(romaPath, bgShowTime, bgEndShowTime, 100, new Vector2(590, 510), 30, OsbEasing.InOutSine);

            var lightShowTime = bgShowTime;
            var lightEndShowTime = bgEndShowTime;
            // furina upper light
            for (int i = 0; i < 20; i++)
            {
                var light = effectLayer.CreateSprite(lightPath, OsbOrigin.CentreLeft, new Vector2(220, -10));
                light.Additive(lightShowTime);
                light.Color(lightShowTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(lightShowTime, (lightEndShowTime + lightShowTime) / 2);
                var moveTime = (lightEndShowTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.4, 0.9), Random(0.2, 0.3), Random(0.4, 1.3), Random(0.4, 0.8));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.2);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(90 - Random(30f, 60f)), MathHelper.DegreesToRadians(90 - Random(10f, 80f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.2, 0);
                light.EndGroup();
            }

            for (int i = 0; i < 10; i++)
            {
                var light = effectLayer.CreateSprite(lightPath, OsbOrigin.CentreLeft, new Vector2(-107 - 30, 480 - 90));
                light.Additive(lightShowTime);
                light.Color(lightShowTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(lightShowTime, (lightEndShowTime + lightShowTime) / 2);
                var moveTime = (lightEndShowTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.4, 0.9), Random(0.2, 0.3), Random(0.4, 0.8), Random(0.4, 0.9));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.1);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(0f, 10f)), MathHelper.DegreesToRadians(Random(0f, 40f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.1, 0);
                light.EndGroup();
            }

            for (int j = 0; j < 15; j++)
            {
                var flare = effectLayer.CreateSprite(flarePath, OsbOrigin.Centre, FlarePosition);
                flare.Additive(lightShowTime);
                flare.Color(lightShowTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(lightShowTime, (lightEndShowTime + lightShowTime) / 2);
                var moveTime = (lightEndShowTime - randomStartTime) / loopCount;
                flare.StartLoopGroup(randomStartTime, loopCount);
                flare.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 2, Random(0.1, 0.14), Random(0.2, 0.3), Random(0.1, 0.13), Random(0.2, 0.3));
                flare.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.35);
                flare.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(0f, 180f)), MathHelper.DegreesToRadians(Random(0f, 360f)));
                flare.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.35, 0);
                flare.EndGroup();
            }

            var burstStartTime = bgStartShowTime - 500;
            var burstEndTime = bgEndShowTime;
            var burstList = new List<double>();
            foreach (var hitObj in Beatmap.HitObjects)
            {
                if (hitObj.StartTime < burstStartTime || hitObj.StartTime > burstEndTime) continue;
                Log($"hitObj.StartTime: {hitObj.StartTime}  hitObj.SamplePath: {hitObj.SamplePath}  hitObj.Additions: {hitObj.Additions}");
                if (hitObj.Additions == HitSoundAddition.Clap || hitObj.SamplePath == "Snare3.ogg")
                {
                    burstList.Add(hitObj.StartTime);
                }
            }
            foreach (var time in burstList)
            {
                var burst = furniaLightBurstLayer.CreateSprite(FurinaLightBurstPath, OsbOrigin.Centre, new Vector2(320, 240));
                burst.Fade(OsbEasing.InOutSine, time, time + 200, 0.4, 0);
                burst.Scale(time, time + 200, 480.0f / furinaBitmap.Height, 1.2 * 480.0f / furinaBitmap.Height);
                burst.Additive(time);
            }

            var whiteShowTime = 58948;
            var whiteEndShowTime = 59976;
            // var white = effectLayer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            // white.ScaleVec(whiteShowTime, 854.0f / 5.0, 480.0f / 5.0);
            // white.Additive(whiteShowTime);
            // white.Fade(whiteShowTime, whiteEndShowTime, 0, 1);

            var gradient = effectLayer.CreateSprite("sb/gradient.png", OsbOrigin.Centre);
            gradient.Scale(whiteShowTime, 10);
            gradient.Fade(whiteShowTime, whiteShowTime + 300, 0, 0.2);
            gradient.Additive(whiteShowTime);
            gradient.MoveX(whiteShowTime, whiteEndShowTime, -1600, 1000);

            var noisePath = @"sb\noise\noise_.png";
            var noiseBitmap = GetMapsetBitmap(@"sb\noise\noise_1.png");

            var noise = effectLayer.CreateAnimation(noisePath, 4, 100/4, OsbLoopType.LoopForever, OsbOrigin.Centre);
            noise.ScaleVec(whiteEndShowTime, 854.0f / noiseBitmap.Width, 480.0f / noiseBitmap.Height);
            noise.Fade(OsbEasing.InSine ,whiteEndShowTime-100, whiteEndShowTime+0.5 * beatDuration, 1, 0);
        }

        public void BGFlowRand(OsbSprite bg, double StartTime, double EndTime, float Offset, Vector2 initPosition, OsbEasing easing1, OsbEasing easing2)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;
            int loopCount = (int)Math.Ceiling((EndTime - StartTime) / (16 * beatDuration));
            var posOffset = new Vector2(-Offset, Offset);
            bg.StartLoopGroup(StartTime, loopCount);
            bg.Move(easing1, 0, LoopLength * beatDuration / 2, initPosition, initPosition + posOffset);
            bg.Move(easing2, LoopLength * beatDuration / 2, LoopLength * beatDuration, initPosition + posOffset, initPosition);
            bg.Rotate(easing1, 0, LoopLength * beatDuration / 4, -0.01, -0.02);
            bg.Rotate(easing2, LoopLength * beatDuration / 4, 3 * LoopLength * beatDuration / 4, -0.02, 0.01);
            bg.Rotate(easing1, 3 * LoopLength * beatDuration / 4, LoopLength * beatDuration, 0.01, -0.01);
            bg.EndGroup();
        }

        public void romaParticles(string romaPath, double StartTime, double EndTime, float Offset, Vector2 initPosition, int particleCount, OsbEasing easing)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;


            for (int j = 0; j < particleCount; j++)
            {
                int randomNumber = Random(1, 13);
                var romaNumberPath = $"{romaPath}{randomNumber}.png";
                var spawnPosition = new Vector2(initPosition.X + Random(-Offset, Offset), initPosition.Y + Random(-Offset / 10, Offset / 10));
                var randomAngle = Random(Math.PI / 4, 3 * Math.PI / 4);
                var endPostion = spawnPosition + new Vector2(540.0f / (float)Math.Tan(randomAngle), -540.0f);
                var moveTime = Random(6 * beatDuration, 9 * beatDuration);
                var startTime = Random(StartTime, EndTime);
                int loopCount = (int)Math.Ceiling((EndTime - startTime) / (moveTime));
                var randomScale = Random(0.1, 0.2);


                var roma = GetLayer("Roma").CreateSprite(romaNumberPath, OsbOrigin.Centre);
                roma.Fade(StartTime, StartTime + 100, 0, 0.1);
                roma.Fade(EndTime - 100, EndTime, 0.1, 0);
                roma.Additive(StartTime);
                roma.Color(StartTime, RomaColor);

                roma.StartLoopGroup(startTime, loopCount);
                roma.MoveX(OsbEasing.InOutSine, 0, moveTime, spawnPosition.X, endPostion.X);
                roma.MoveY(OsbEasing.OutSine, 0, moveTime, spawnPosition.Y, endPostion.Y);
                roma.Rotate(OsbEasing.InOutSine, 0, moveTime, 0, randomAngle);
                roma.EndGroup();
                roma.StartLoopGroup(startTime, loopCount * 2);
                roma.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 4, 0, randomScale, randomScale, randomScale);
                roma.ScaleVec(OsbEasing.InOutSine, moveTime / 4, moveTime / 2, randomScale, randomScale, 0, randomScale);
                roma.EndGroup();

            }
        }
    }
}
