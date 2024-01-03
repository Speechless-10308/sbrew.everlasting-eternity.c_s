using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Animations;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class DropPart4 : StoryboardObjectGenerator
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
        [Configurable]
        public double BeatDivisor = 16;
        [Group("Bars")]
        [Configurable] public int BarCount = 96;
        [Configurable] public int LogScale = 600;
        [Configurable] public OsbEasing FftEasing = OsbEasing.InExpo;
        [Configurable] public float MinimalHeight = 0.05f;

        [Group("Optimization")]
        [Configurable] public double Tolerance = 0.2;
        [Configurable] public int CommandDecimals = 1;
        [Configurable] public int FrequencyCutOff = 16000;
        public override void Generate()
        {
            string seaPath = @"sb\sea.jpg";
            string bubblePath = @"sb\bubble.png";
            string flarePath = @"sb\flare.png";
            string lightPath = @"sb\light.png";
            string light2Path = @"sb\light2.png";
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

            var bgShowTime = 295689;
            var bgEndShowTime = 339574;
            var sea = bgLayer.CreateSprite(seaPath, OsbOrigin.Centre);
            sea.Fade(OsbEasing.InOutSine, bgShowTime, bgShowTime + beatDuration, 0, 0.5);
            sea.Scale(bgShowTime, 1.05 * 480.0f / seaBitmap.Height);
            sea.Color(bgShowTime, SeaColor);
            sea.Fade(bgEndShowTime - 100, bgEndShowTime, 0.5, 0);
            BGFlowRand(sea, bgShowTime, bgEndShowTime, 8, new Vector2(320, 240), OsbEasing.InOutSine, OsbEasing.InOutSine);

            var furinaShowTime = bgShowTime;
            var furinaEndShowTime = bgEndShowTime;

            var furinaGray = furinaLayer.CreateSprite(furinaGrayPath, OsbOrigin.Centre, new Vector2(500, 210));
            furinaGray.Fade(OsbEasing.InOutSine, furinaShowTime, furinaShowTime + beatDuration, 0, 0.1);
            furinaGray.Scale(furinaShowTime, 600.0f / furinaBitmap.Height);
            furinaGray.Fade(furinaEndShowTime - 100, furinaEndShowTime, 0.1, 0);
            furinaGray.Additive(furinaShowTime);

            var furina = furinaLayer.CreateSprite(furinaPath, OsbOrigin.Centre);
            furina.Fade(OsbEasing.InOutSine, furinaShowTime, furinaShowTime + beatDuration, 0, 1);
            furina.Scale(furinaShowTime, 480.0f / furinaBitmap.Height);
            furina.Fade(furinaEndShowTime - 100, furinaEndShowTime, 1, 0);

            var lightShowTime = bgShowTime;
            var lightEndShowTime = bgEndShowTime;
            // furina upper light
            for (int i = 0; i < 6; i++)
            {
                var light = effectLayer.CreateSprite(lightPath, OsbOrigin.CentreLeft, new Vector2(220, -10));
                light.Additive(lightShowTime);
                light.Color(lightShowTime, LightColor);
                var loopCount = Random(6, 14);
                var randomStartTime = Random(lightShowTime, lightShowTime + 1 / 6 * (lightEndShowTime - lightShowTime));
                var moveTime = (lightEndShowTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.4, 0.9), Random(0.2, 0.3), Random(0.4, 1.3), Random(0.4, 0.8));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.2);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(90 - Random(30f, 60f)), MathHelper.DegreesToRadians(90 - Random(10f, 80f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.2, 0);
                light.EndGroup();
            }


            // Vision light for furina
            for (int j = 0; j < 5; j++)
            {
                var flare = effectLayer.CreateSprite(flarePath, OsbOrigin.Centre, FlarePosition);
                flare.Additive(lightShowTime);
                flare.Color(lightShowTime, LightColor);
                var loopCount = Random(6, 12);
                var randomStartTime = Random(lightShowTime, lightShowTime + 1 / 6 * (lightEndShowTime - lightShowTime));
                var moveTime = (lightEndShowTime - randomStartTime) / loopCount;
                flare.StartLoopGroup(randomStartTime, loopCount);
                flare.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 2, Random(0.1, 0.14), Random(0.2, 0.3), Random(0.1, 0.13), Random(0.2, 0.3));
                flare.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.35);
                flare.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(0f, 180f)), MathHelper.DegreesToRadians(Random(0f, 360f)));
                flare.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.35, 0);
                flare.EndGroup();
            }


            // test the around spectrum
            var barStateList = barsAroundPerson("sb/white.png", 0.7, bgShowTime, bgEndShowTime, 5000, 2.0, 100, BarCount, 400, 80, new Vector2(320, 400), 200, 296250);

            generateStaticBar(294317, 295346, 200, barStateList, 0.7);

            ParticlesFlow(@"sb\dot.png", bgShowTime, bgEndShowTime, 100, new Vector2(50, 510), 50, OsbEasing.InOutSine);
            ParticlesFlow(@"sb\dot.png", bgShowTime, bgEndShowTime, 100, new Vector2(590, 510), 50, OsbEasing.InOutSine);

            romaParticles(@"sb\roma\", bgShowTime, bgEndShowTime, 100, new Vector2(50, 510), 50, OsbEasing.InOutSine);
            romaParticles(@"sb\roma\", bgShowTime, bgEndShowTime, 100, new Vector2(590, 510), 50, OsbEasing.InOutSine);


            var burstStartTime = bgShowTime;
            var burstEndTime = 338203;
            var burstList = new List<double>();
            var whistleList = new List<double>();
            foreach (var hitObj in Beatmap.HitObjects)
            {
                if (hitObj.StartTime < burstStartTime || hitObj.StartTime > burstEndTime) continue;
                Log($"hitObj.StartTime: {hitObj.StartTime}  hitObj.SamplePath: {hitObj.SamplePath}  hitObj.Additions: {hitObj.Additions}");
                if (hitObj.Additions == HitSoundAddition.Clap || hitObj.SamplePath == "Snare2.wav")
                {
                    burstList.Add(hitObj.StartTime);
                }
                if (hitObj.Additions == HitSoundAddition.Whistle || hitObj.SamplePath == "soft-hitwhistle.wav")
                {
                    whistleList.Add(hitObj.StartTime);
                }
            }

            // some optimize things
            var burstSpritePool = new OsbSpritePool(furniaLightBurstLayer, FurinaLightBurstPath, OsbOrigin.Centre, (sprite, startTime, endTime) =>
            {
                sprite.Additive(startTime);
            });
            using (burstSpritePool)
            {
                foreach (var time in burstList)
                {
                    var burst = burstSpritePool.Get(time, time + 300);
                    burst.Fade(OsbEasing.InOutSine, time, time + 300, 0.4, 0);
                    burst.Scale(time, time + 300, 480.0f / furinaBitmap.Height, 1.2 * 480.0f / furinaBitmap.Height);
                }
            }

            var light2SpritePool = new OsbSpritePool(effectLayer, light2Path, OsbOrigin.Centre, (sprite, startTime, endTime) =>
            {
                sprite.Additive(startTime);
                sprite.Color(startTime, LightColor);
                sprite.Scale(startTime, 480.0f / lightBitmap.Height);
            });
            using (light2SpritePool)
            {
                foreach (var time in burstList)
                {
                    var light = light2SpritePool.Get(time, time + 300);
                    light.Fade(OsbEasing.InOutSine, time, time + 300, 0.15, 0);
                    light.Move(time, 747, 0);
                }
                foreach (var time in whistleList)
                {
                    var light = light2SpritePool.Get(time, time + 300);
                    light.Fade(OsbEasing.InOutSine, time, time + 300, 0.15, 0);
                    light.Move(time, -107, 480);
                }
            }
            // idk why this part lack of hs, so add a white to balance the mood of the song.
            var white = effectLayer.CreateSprite("sb/white.png", OsbOrigin.Centre);
            var whiteBitmap = GetMapsetBitmap("sb/white.png");
            white.Fade(OsbEasing.OutExpo ,328603, 333403, 0, 0.4);
            white.Fade(OsbEasing.InExpo ,333403, 334089, 0.4, 0);
            white.ScaleVec(328603, 854.0f / whiteBitmap.Height, 480.0f / whiteBitmap.Height);
            white.Additive(328603);

            var gradient = effectLayer.CreateSprite(@"sb\gradient.png", OsbOrigin.Centre);
            gradient.Scale(bgShowTime, 10);
            gradient.Fade(bgShowTime, bgEndShowTime, 0.1, 0.2);
            gradient.Rotate(bgShowTime, bgEndShowTime, 0, Math.PI / 8);
            gradient.Additive(bgShowTime);
            gradient.MoveX(bgShowTime, bgEndShowTime, -1600, -1500);

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

        public List<List<double>> barsAroundPerson(string barPath, double opacity, double totalStartTime, double totalEndTime, double T, double xScaleMax, double yScaleMax, int barCount, double radius, double tilt, Vector2 barCentrePosition, double xDomain, double replayTime = 0)
        {
            // get fft
            var fftTimeStep = Beatmap.GetTimingPointAt((int)totalStartTime).BeatDuration / BeatDivisor;
            var fftOffset = fftTimeStep * 0.2;
            var barStateList = new List<List<double>>();

            var bitmap = GetMapsetBitmap(barPath);
            var heightLists = new List<double>[barCount];
            for (var i = 0; i < barCount; i++)
                heightLists[i] = new List<double>();

            for (var time = (double)totalStartTime; time < totalEndTime; time += fftTimeStep)
            {
                var fft = GetFft(time + fftOffset, barCount, null, FftEasing, FrequencyCutOff);
                for (var i = 0; i < barCount; i++)
                {
                    var height = (float)Math.Log10(1 + fft[i] * LogScale) * yScaleMax / bitmap.Height;
                    if (height < MinimalHeight) height = MinimalHeight;
                    heightLists[i].Add(height);
                }
            }

            // create bars
            var layer = GetLayer("Bars");

            var barIndex = 0;
            for (var time = (double)totalStartTime - T * 2; time < totalStartTime - T; time += T / barCount)
            {
                var barState = new List<double>();

                // there sometimes will break the index.
                if (barIndex == barCount) break;
                var bar = layer.CreateSprite(barPath, OsbOrigin.BottomCentre);
                singleBarRotateAround(bar, time, totalEndTime, T, radius, tilt, barCentrePosition, opacity);

                var scaleKeyframes = new KeyframedValue<Vector2>(null);
                var xScaleList = new List<double>();

                for (var time2 = totalStartTime; time2 <= totalStartTime + T; time2 += fftTimeStep)
                {
                    var currentX = bar.PositionAt(time2).X;

                    if (time2 >= replayTime - fftTimeStep && time2 <= replayTime)
                    {
                        barState.Add(currentX);
                        var currentY = bar.PositionAt(time2).Y;
                        barState.Add(currentY);
                    }
                    var xScale = GetScaleFromPosition(currentX, barCentrePosition.X, radius, xScaleMax);
                    xScaleList.Add(xScale);
                }

                for (int j = 0; j < heightLists[barIndex].Count; j++)
                {
                    var xScale = xScaleList[j % xScaleList.Count];
                    var yScale = heightLists[barIndex][j];
                    scaleKeyframes.Add(totalStartTime + j * fftTimeStep, new Vector2((float)xScale, (float)yScale));

                    if (totalStartTime + j * fftTimeStep >= replayTime - fftTimeStep && totalStartTime + j * fftTimeStep <= replayTime)
                    {
                        barState.Add(xScale);
                        barState.Add(yScale);
                    }
                }

                scaleKeyframes.Simplify2dKeyframes(Tolerance, h => h);

                scaleKeyframes.ForEachPair(
                    (start, end) =>
                    {
                        bar.ScaleVec(OsbEasing.None, start.Time, end.Time, start.Value, end.Value);
                    },
                    new Vector2((float)xScaleMax, MinimalHeight),
                    s => new Vector2((float)Math.Round(s.X, CommandDecimals), (float)Math.Round(s.Y, CommandDecimals))
                );

                bar.ScaleVec(totalStartTime - 1, 0, 0);
                bar.ScaleVec(totalEndTime + 1, 0, 0);

                var H = (barIndex * 360.0 / barCount) + Random(-10.0, 10.0);
                var S = 0.6 + Random(0.4);
                bar.ColorHsb(totalStartTime, H, S, opacity);

                barState.Add(H);
                barState.Add(S);
                bar.Additive(totalStartTime, totalEndTime);
                barIndex++;

                barStateList.Add(barState);
            }
            return barStateList;
        }

        public void singleBarRotateAround(OsbSprite bar, double spawnTime, double totalEndTime, double T, double radius, double tilt, Vector2 centrePosition, double opacity = 1.0)
        {
            // this is responsible for the rotation movement of the bar, it returns the start time of the birth of the bar.
            var loopCount = (int)Math.Ceiling((totalEndTime - spawnTime) / T);

            bar.StartLoopGroup(spawnTime, loopCount);
            bar.MoveX(OsbEasing.InOutSine, 0, T / 2, centrePosition.X - radius, centrePosition.X + radius);
            bar.MoveX(OsbEasing.InOutSine, T / 2, T, centrePosition.X + radius, centrePosition.X - radius);
            bar.EndGroup();

            loopCount = (int)Math.Ceiling((totalEndTime - spawnTime - T / 4) / T);
            bar.StartLoopGroup(spawnTime + T / 4, loopCount);
            bar.MoveY(OsbEasing.InOutSine, 0, T / 2, centrePosition.Y + tilt, centrePosition.Y - tilt);
            bar.MoveY(OsbEasing.InOutSine, T / 2, T, centrePosition.Y - tilt, centrePosition.Y + tilt);
            bar.EndGroup();

            loopCount = (int)Math.Ceiling((totalEndTime - spawnTime - T / 16 - T / 2) / T);
            bar.StartLoopGroup(spawnTime + T / 16 + T / 2, loopCount);
            bar.Fade(OsbEasing.InOutSine, 0, T / 16, opacity, 0);
            bar.Fade(OsbEasing.InOutSine, T / 2 - 3 * T / 16, T / 2 - T / 8, 0, opacity);
            bar.Fade(T, opacity);
            bar.EndGroup();
        }

        public double GetScaleFromPosition(double x, double centreXPosition, double radius, double xScaleMax)
        {
            var angle = Math.Asin((x - centreXPosition) / radius);
            var xScale = Math.Cos(angle) * xScaleMax;
            return xScale;
        }

        public void ParticlesFlow(string particlePath, double StartTime, double EndTime, float Offset, Vector2 initPosition, int particleCount, OsbEasing easing)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;


            for (int j = 0; j < particleCount; j++)
            {

                var spawnPosition = new Vector2(initPosition.X + Random(-Offset, Offset), initPosition.Y + Random(-Offset / 10, Offset / 10));
                var randomAngle = Random(Math.PI / 4, 3 * Math.PI / 4);
                var endPostion = spawnPosition + new Vector2(540.0f / (float)Math.Tan(randomAngle), -540.0f);
                var moveTime = Random(6 * beatDuration, 9 * beatDuration);
                var startTime = Random(StartTime - 1 / 1.3 * (EndTime - StartTime), StartTime + 1 / 2 * (EndTime - StartTime));
                int loopCount = (int)Math.Ceiling((EndTime - startTime) / (moveTime));
                var randomScale = Random(1.0, 3.0);


                var roma = GetLayer("Roma").CreateSprite(particlePath, OsbOrigin.Centre);
                roma.Fade(StartTime, StartTime + 100, 0, 0.3);
                roma.Fade(EndTime - 100, EndTime, 0.3, 0);
                roma.Additive(StartTime);
                roma.Color(StartTime, RomaColor);

                roma.StartLoopGroup(startTime, loopCount);
                roma.MoveX(OsbEasing.InOutSine, 0, moveTime, spawnPosition.X, endPostion.X);
                roma.MoveY(OsbEasing.OutSine, 0, moveTime, spawnPosition.Y, endPostion.Y);
                roma.Rotate(OsbEasing.InOutSine, 0, moveTime, 0, randomAngle);
                roma.EndGroup();
                roma.StartLoopGroup(startTime, loopCount * 2);
                roma.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 4, 0.5 * randomScale, randomScale, randomScale, randomScale);
                roma.ScaleVec(OsbEasing.InOutSine, moveTime / 4, moveTime / 2, randomScale, randomScale, 0.5 * randomScale, randomScale);
                roma.EndGroup();

            }
        }

        public void generateStaticBar(double startTime, double endTime, double animationTime, List<List<double>> barStateList, double opacity)
        {
            var barCount = barStateList.Count;
            for (int i = 0; i < barCount; i++)
            {
                var barState = barStateList[i];
                var bar = GetLayer("Bars").CreateSprite("sb/white.png", OsbOrigin.BottomCentre);
                Log($"barX: {barState[0]}  barY: {barState[1]}  scaleX: {barState[2]}  scaleY: {barState[3]}  H: {barState[4]}  S: {barState[5]}");
                bar.Move(startTime, barState[0], barState[1]);
                bar.ScaleVec(startTime, barState[2], barState[3]);
                bar.ColorHsb(startTime, barState[4], barState[5], opacity);
                bar.Additive(startTime);
                bar.Fade(startTime + i * (endTime - startTime) / barCount, startTime + i * (endTime - startTime) / barCount + animationTime, 0, opacity);
            }
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
                var startTime = Random(StartTime - 1/1.2 * (EndTime - StartTime), StartTime + 1/1.2 * (EndTime - StartTime));
                int loopCount = (int)Math.Ceiling((EndTime - startTime) / (moveTime));
                var randomScale = Random(0.1, 0.2);


                var roma = GetLayer("Roma").CreateSprite(romaNumberPath, OsbOrigin.Centre);
                roma.Fade(StartTime, StartTime + 100, 0, 0.1);
                roma.Fade(EndTime - 100, EndTime, 0.1, 0);
                roma.Additive(StartTime);

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
