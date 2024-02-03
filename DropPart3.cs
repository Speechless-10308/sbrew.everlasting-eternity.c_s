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
    public class DropPart3 : StoryboardObjectGenerator
    {
        [Group("Colors")]
        [Configurable]
        public Color4 SeaColor = Color4.Blue;
        [Configurable]
        public Color4 SeaColor2 = Color4.DarkCyan;
        [Configurable]
        public Color4 SeaColor3 = Color4.Red;
        [Configurable]
        public Color4 RomaColor = Color4.White;
        [Configurable]
        public Color4 RomaColor2 = Color4.White;
        [Configurable]
        public Color4 RomaColor3 = Color4.White;
        [Configurable]
        public Color4 LightColor = Color4.WhiteSmoke;
        [Configurable]
        public Color4 LightColor2 = Color4.WhiteSmoke;
        [Configurable]
        public Color4 LightColor3 = Color4.WhiteSmoke;

        [Configurable]
        public Color4 FurinaColor = Color4.White;
        [Configurable]
        public Color4 FurinaColor2 = Color4.White;
        [Configurable]
        public Color4 FurinaColor3 = Color4.White;


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

            var bgShowTime = 191611;
            var bgEndShowTime = 230917;
            var styleChangeTime = 212383;
            var sea = bgLayer.CreateSprite(seaPath, OsbOrigin.Centre);
            sea.Fade(OsbEasing.InOutSine, bgShowTime, bgShowTime + beatDuration, 0, 0.5);
            sea.Scale(bgShowTime, 1.05 * 480.0f / seaBitmap.Height);
            sea.Color(OsbEasing.InSine ,bgShowTime, styleChangeTime, SeaColor, SeaColor2);
            sea.Color(OsbEasing.OutSine, styleChangeTime, bgEndShowTime, SeaColor2, SeaColor3);
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
            furina.Color(OsbEasing.InSine, furinaShowTime, styleChangeTime, FurinaColor, FurinaColor2);
            furina.Color(OsbEasing.OutSine, styleChangeTime, furinaEndShowTime, FurinaColor2, FurinaColor3);

            var lightShowTime = bgShowTime;
            var lightEndShowTime = bgEndShowTime;
            // furina upper light
            for (int i = 0; i < 6; i++)
            {
                var light = effectLayer.CreateSprite(lightPath, OsbOrigin.CentreLeft, new Vector2(220, -10));
                light.Additive(lightShowTime);
                light.Color(OsbEasing.InSine ,lightShowTime, styleChangeTime, LightColor, LightColor2);
                light.Color(OsbEasing.OutSine, styleChangeTime, lightEndShowTime, LightColor2, LightColor3);
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
            for (int j = 0; j < 6; j++)
            {
                var flare = effectLayer.CreateSprite(flarePath, OsbOrigin.Centre, FlarePosition);
                flare.Additive(lightShowTime);
                flare.Color(OsbEasing.InSine ,lightShowTime, styleChangeTime, LightColor, LightColor2);
                flare.Color(OsbEasing.OutSine, styleChangeTime, lightEndShowTime, LightColor2, LightColor3);
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

            
            ParticlesRotateAroundPerson(@"sb\t.png", bgShowTime, styleChangeTime, 5000, 20, 400, 70, 3, 0.1, 310, 0.4, false, true, true);
            ParticlesRotateAroundPerson(@"sb\t.png", bgShowTime, styleChangeTime, 5000, 20, 310, 70, 3, 0.1, 390, 0.4, false, true, true);
            ParticlesRotateAroundPerson(@"sb\white.png", bgShowTime, styleChangeTime, 5000, 20, 355, 70, 3, 5.0, 350, 0.4, false, true, true);
            ParticlesRotateAroundPerson(@"sb\dot.png", bgShowTime, styleChangeTime, 5000, 20, 445, 70, 3, 3.0, 300, 0.4, false, false, true);
            ParticlesRotateAroundPerson(@"sb\dot.png", bgShowTime, styleChangeTime, 5000, 20, 490, 70, 3, 3.0, 260, 0.4, false, false, true);

            ParticlesFlow(@"sb\dot.png", bgShowTime, bgEndShowTime, 100, new Vector2(50, 510), 50, OsbEasing.InOutSine);
            ParticlesFlow(@"sb\dot.png", bgShowTime, bgEndShowTime, 100, new Vector2(590, 510), 50, OsbEasing.InOutSine);

            romaParticles(@"sb\roma\", bgShowTime, bgEndShowTime, styleChangeTime, 100, new Vector2(50, 510), 50, OsbEasing.InOutSine);
            romaParticles(@"sb\roma\", bgShowTime, bgEndShowTime, styleChangeTime, 100, new Vector2(590, 510), 50, OsbEasing.InOutSine);

            ParticlesRotateAroundPerson(@"sb\t.png",  styleChangeTime, bgEndShowTime, 5000, 20, 400, 70, 3, 0.1, 310, 0.4, true, true, false);
            ParticlesRotateAroundPerson(@"sb\t.png", styleChangeTime, bgEndShowTime, 5000, 20, 310, 70, 3, 0.1, 390, 0.4, true, true, false);
            ParticlesRotateAroundPerson(@"sb\white.png", styleChangeTime, bgEndShowTime, 5000, 20, 355, 70, 3, 5.0, 350, 0.4, true, true, false);
            ParticlesRotateAroundPerson(@"sb\dot.png", styleChangeTime, bgEndShowTime, 5000, 20, 445, 70, 3, 3.0, 300, 0.4, true, false, false);
            ParticlesRotateAroundPerson(@"sb\dot.png", styleChangeTime, bgEndShowTime, 5000, 20, 490, 70, 3, 3.0, 260, 0.4, true, false, false);

            var burstStartTime = bgShowTime;
            var burstEndTime = bgEndShowTime;
            var burstList = new List<double>();
            var whistleList = new List<double>();
            foreach (var hitObj in Beatmap.HitObjects)
            {
                if (hitObj.StartTime < burstStartTime || hitObj.StartTime > burstEndTime) continue;
                Log($"hitObj.StartTime: {hitObj.StartTime}  hitObj.SamplePath: {hitObj.SamplePath}  hitObj.Additions: {hitObj.Additions}");
                if (hitObj.Additions == HitSoundAddition.Clap || hitObj.SamplePath == "Snare3.ogg")
                {
                    burstList.Add(hitObj.StartTime);
                }
                if (hitObj.Additions == HitSoundAddition.Whistle)
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
                    if (time > styleChangeTime)
                    {
                        burst.Color(time, time + 300, RomaColor, RomaColor2);
                    }
                }
            }

            var light2SpritePool = new OsbSpritePool(effectLayer, light2Path, OsbOrigin.Centre, (sprite, startTime, endTime) =>
            {
                sprite.Additive(startTime);
                sprite.Scale(startTime, 480.0f / lightBitmap.Height);
            });
            using (light2SpritePool)
            {
                foreach (var time in burstList)
                {
                    var light = light2SpritePool.Get(time, time + 300);
                    light.Fade(OsbEasing.InOutSine, time, time + 300, 0.2, 0);
                    light.Move(time, 747, 0);

                    var scaleParameter = (time-burstStartTime)/(burstEndTime-burstStartTime);
                    if (time < styleChangeTime)
                        light.Color(time, SeaColor.R + scaleParameter * (SeaColor2.R-SeaColor.R), SeaColor.G + scaleParameter * (SeaColor2.G-SeaColor.G), SeaColor.B + scaleParameter * (SeaColor2.B-SeaColor.B));
                    else
                        light.Color(time, SeaColor2.R + scaleParameter * (SeaColor3.R-SeaColor2.R), SeaColor2.G + scaleParameter * (SeaColor3.G-SeaColor2.G), SeaColor2.B + scaleParameter * (SeaColor3.B-SeaColor2.B));
                }
                foreach (var time in whistleList)
                {
                    var light = light2SpritePool.Get(time, time + 300);
                    light.Fade(OsbEasing.InOutSine, time, time + 300, 0.2, 0);
                    light.Move(time, -107, 480);

                    var scaleParameter = (time-burstStartTime)/(burstEndTime-burstStartTime);
                    if (time < styleChangeTime)
                        light.Color(time, SeaColor.R + scaleParameter * (SeaColor2.R-SeaColor.R), SeaColor.G + scaleParameter * (SeaColor2.G-SeaColor.G), SeaColor.B + scaleParameter * (SeaColor2.B-SeaColor.B));
                    else
                        light.Color(time, SeaColor2.R + scaleParameter * (SeaColor3.R-SeaColor2.R), SeaColor2.G + scaleParameter * (SeaColor3.G-SeaColor2.G), SeaColor2.B + scaleParameter * (SeaColor3.B-SeaColor2.B));
                }
            }

            var gradient = effectLayer.CreateSprite(@"sb\gradient.png", OsbOrigin.Centre);
            gradient.Scale(bgShowTime, 10);
            gradient.Fade(bgShowTime, styleChangeTime, 0.1, 0.13);
            gradient.Rotate(bgShowTime, styleChangeTime, 0, Math.PI/8);
            gradient.Additive(bgShowTime);
            gradient.MoveX(bgShowTime, styleChangeTime, 2000, 2200);
            gradient.Fade(styleChangeTime, bgEndShowTime, 0.13, 0.23);
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

        public void ParticlesRotateAroundPerson(string path, double totalStartTime, double totalEndTime, double T, int particleCount, float positionCenterY, double moveY, double division, double scale, double radius, double opacity, bool isFlash, bool isRotate, bool isFlow, bool isAdditive = true)
        {
            var ClapList = new List<double>();
            var WhistleList = new List<double>();
            if (isFlash)
            {
                var previousTime = 0d;
                // read the hit object here
                foreach (var hitObject in Beatmap.HitObjects)
                {
                    if (hitObject.StartTime < totalStartTime || hitObject.StartTime > totalEndTime) continue;
                    if (hitObject.Additions == HitSoundAddition.Clap || hitObject.SamplePath == "Snare3.ogg")
                    {
                        if (previousTime == hitObject.StartTime) continue;
                        ClapList.Add(hitObject.StartTime);
                    }
                    else if (hitObject.Additions == HitSoundAddition.Whistle)
                    {
                        if (previousTime == hitObject.StartTime) continue;
                        WhistleList.Add(hitObject.StartTime);
                    }
                    previousTime = hitObject.StartTime;
                }
            }

            for (var time = totalStartTime - T * 2; time < totalStartTime - T; time += T / particleCount)
            {
                if (!isFlash)
                    SingleParticleRotateAroundPerson(path, totalStartTime, totalEndTime, time, T, positionCenterY, moveY, division, scale, radius, opacity, isFlash, isRotate, isFlow, isAdditive);
                else
                    SingleParticleRotateAroundPerson(path, totalStartTime, totalEndTime, time, T, positionCenterY, moveY, division, scale, radius, opacity, isFlash, isRotate, isFlow, isAdditive, ClapList, WhistleList);
            }
        }

        public void SingleParticleRotateAroundPerson(string path, double totalStartTime, double totalEndTime, double spawnTime, double T, float positionCenterY, double moveY, double division, double scale, double radius, double opacity, bool isFlash, bool isRotate, bool isFlow, bool isAdditive) => SingleParticleRotateAroundPerson(path, totalStartTime, totalEndTime, spawnTime, T, positionCenterY, moveY, division, scale, radius, opacity, isFlash, isRotate, isFlow, isAdditive, new List<double>(), new List<double>());

        public void SingleParticleRotateAroundPerson(string path, double totalStartTime, double totalEndTime, double spawnTime, double T, float positionCenterY, double moveY, double division, double scale, double radius, double opacity, bool isFlash, bool isRotate, bool isFlow, bool isAdditive, List<double> ClapList, List<double> WhistleList)
        {
            // this function is used to generate one particle that rotate around person.
            var layer = GetLayer("Effect");
            var beatDuration = Beatmap.GetTimingPointAt((int)totalStartTime).BeatDuration;

            var particle = layer.CreateSprite(path, OsbOrigin.Centre);
            if (isAdditive) particle.Additive(totalStartTime);

            if (isFlash)
            {
                foreach (var clapTime in ClapList)
                {
                    if (clapTime <= totalStartTime + beatDuration / 3 || clapTime >= totalEndTime) continue;
                    var currentBeatduration = Beatmap.GetTimingPointAt((int)clapTime).BeatDuration;
                    particle.Scale(OsbEasing.OutExpo, clapTime, clapTime + currentBeatduration / 8, scale * 2.0, scale);
                }

                foreach (var whistleTime in WhistleList)
                {
                    if (whistleTime <= totalStartTime + beatDuration / 3 || whistleTime >= totalEndTime) continue;
                    particle.Scale(OsbEasing.OutExpo, whistleTime, whistleTime + beatDuration / 8, scale * 1.5, scale);
                }
            }

            int LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime) / (T));
            particle.StartLoopGroup(spawnTime, LoopCount);
            particle.MoveX(OsbEasing.InOutSine, 0, T / 2, 320 - radius, 320 + radius);
            particle.MoveX(OsbEasing.InOutSine, T / 2, T, 320 + radius, 320 - radius);
            particle.EndGroup();

            LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime - T / division) / (T));
            particle.StartLoopGroup(spawnTime + T / division, LoopCount);
            particle.MoveY(OsbEasing.InOutSine, 0, T / 2, positionCenterY - moveY, positionCenterY + moveY);
            particle.MoveY(OsbEasing.InOutSine, T / 2, T, positionCenterY + moveY, positionCenterY - moveY);
            particle.EndGroup();

            if (isRotate)
            {
                LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime) / (T));
                particle.StartLoopGroup(spawnTime, LoopCount);
                particle.Rotate(OsbEasing.InOutSine, 0, T / 2, MathHelper.DegreesToRadians(0), MathHelper.DegreesToRadians(180));
                particle.Rotate(OsbEasing.InOutSine, T / 2, T, MathHelper.DegreesToRadians(180), MathHelper.DegreesToRadians(360));
                particle.EndGroup();
            }
            // will be overlapped if used with flash.
            if (isFlow)
            {
                var randomT = Random(T / 2, T);
                LoopCount = (int)Math.Floor((totalEndTime - totalStartTime - beatDuration / 3) / randomT);
                particle.StartLoopGroup(totalStartTime + beatDuration / 3, LoopCount);
                particle.ScaleVec(OsbEasing.InOutSine, 0, randomT / 2, 0, scale, scale, scale);
                particle.ScaleVec(OsbEasing.InOutSine, randomT / 2, randomT, scale, scale, 0, scale);
                particle.EndGroup();
            }
            if (!isFlow)
            {
                // will be incapable to implement with flow
                particle.Scale(OsbEasing.InOutSine, totalStartTime, totalStartTime + beatDuration / 3, 0, scale);
                particle.Scale(OsbEasing.InOutSine, totalEndTime, totalEndTime + beatDuration / 3, scale, 0);
            }
            else
            {
                particle.ScaleVec(OsbEasing.InOutSine, totalStartTime, totalStartTime + beatDuration / 3, 0, 0, 0, scale);

            }

            LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime - T / 8) / (T));
            particle.StartLoopGroup(spawnTime + T / 8, LoopCount);
            particle.Fade(OsbEasing.InOutSine, 0, T / 2, opacity, 0);
            particle.Fade(OsbEasing.InOutSine, 4 * T / 5, T, 0, opacity);
            particle.EndGroup();

        }
        public void ParticlesFlow(string particlePath, double StartTime, double EndTime, float Offset, Vector2 initPosition, int particleCount, OsbEasing easing) => ParticlesFlow(particlePath, StartTime, EndTime, 212382, Offset, initPosition, particleCount, easing);
        public void ParticlesFlow(string particlePath, double StartTime, double EndTime, double ChangeTime, float Offset, Vector2 initPosition, int particleCount, OsbEasing easing)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;


            for (int j = 0; j < particleCount; j++)
            {

                var spawnPosition = new Vector2(initPosition.X + Random(-Offset, Offset), initPosition.Y + Random(-Offset / 10, Offset / 10));
                var randomAngle = Random(Math.PI / 4, 3 * Math.PI / 4);
                var endPostion = spawnPosition + new Vector2(540.0f / (float)Math.Tan(randomAngle), -540.0f);
                var moveTime = Random(6 * beatDuration, 9 * beatDuration);
                var startTime = Random(StartTime, EndTime);
                int loopCount = (int)Math.Ceiling((EndTime - startTime) / (moveTime));
                var randomScale = Random(1.0, 3.0);


                var roma = GetLayer("Roma").CreateSprite(particlePath, OsbOrigin.Centre);
                roma.Fade(StartTime, StartTime + 100, 0, 0.3);
                roma.Fade(EndTime - 100, EndTime, 0.3, 0);
                roma.Additive(StartTime);
                roma.Color(OsbEasing.InSine ,StartTime, ChangeTime, RomaColor, RomaColor2);
                roma.Color(OsbEasing.OutSine, ChangeTime, EndTime, RomaColor2, RomaColor3);

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

        public void romaParticles(string romaPath, double StartTime, double EndTime, double ChangeTime, float Offset, Vector2 initPosition, int particleCount, OsbEasing easing)
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
                roma.Color(OsbEasing.InSine ,StartTime, ChangeTime, RomaColor, RomaColor2);
                roma.Color(OsbEasing.OutSine, ChangeTime, EndTime, RomaColor2, RomaColor3);

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
