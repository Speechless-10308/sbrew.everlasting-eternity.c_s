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
    public class DropPart1 : StoryboardObjectGenerator
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

            var bgShowTime = 60319;
            var bgEndShowTime = 108319;
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
            // we need roma text to scroll around furina.
            double romaShowTime = bgShowTime;
            double romaEndShowTime =  87062;
            double T = 5000;
            for (int i = 1; i<13; i++)
            {
                var romaNumberPath = $"sb/roma/{i}.png";
                var spawnTime = romaShowTime - 2 * T + i * T / 12;
                SingleParticleRotateAroundPerson(romaNumberPath, romaShowTime, romaEndShowTime, spawnTime, T, 400, 70, 3, 0.1, 350, 0.5, false, true, true, true);
            }

            var lightShowTime = bgShowTime;
            var lightEndShowTime = bgEndShowTime;
            // furina upper light
            for (int i = 0; i < 20; i++)
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
            for (int j = 0; j < 15; j++)
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

            ParticlesRotateAroundPerson(@"sb\white.png", 60319, 87062, 5000, 12, 400, 70, 3, 5.0, 420,0.4, false, true, true);
            ParticlesRotateAroundPerson(@"sb\white.png", 87748, 108319, 5000, 20, 400, 70, 3, 3.0, 310, 0.4, true, true, false);
            ParticlesRotateAroundPerson(@"sb\t.png", 87748, 108319, 5000, 20, 310, 70, 3, 0.1, 390, 0.4, true, true, false);
            ParticlesRotateAroundPerson(@"sb\white.png", 87748, 108319, 5000, 20, 355, 70, 3, 5.0, 350, 0.4, true, true, false);
            
            ParticlesFlow(@"sb\dot.png", 60319, 108319, 100, new Vector2(50, 510), 100, OsbEasing.InOutSine);
            ParticlesFlow(@"sb\dot.png", 60319, 108319, 100, new Vector2(590, 510), 100, OsbEasing.InOutSine);

            var burstStartTime = bgShowTime;
            var burstEndTime = bgEndShowTime;
            var burstList = new List<double>();
            var whistleList = new List<double>();
            foreach (var hitObj in Beatmap.HitObjects)
            {
                if (hitObj.StartTime < burstStartTime || hitObj.StartTime > burstEndTime) continue;
                Log($"hitObj.StartTime: {hitObj.StartTime}  hitObj.SamplePath: {hitObj.SamplePath}  hitObj.Additions: {hitObj.Additions}");
                if (hitObj.Additions == HitSoundAddition.Clap || hitObj.SamplePath == "Snare3.wav")
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
                    var burst = burstSpritePool.Get(time, time + 500);
                    burst.Fade(OsbEasing.InOutSine, time, time + 500, 0.4, 0);
                    burst.Scale(time, time + 500, 480.0f / furinaBitmap.Height, 1.2 * 480.0f / furinaBitmap.Height);
                }
            }
            
            var light2SpritePool = new OsbSpritePool(effectLayer, light2Path, OsbOrigin.Centre, (sprite, startTime, endTime) =>
            {
                sprite.Additive(startTime);
                sprite.Color(startTime, LightColor);
                sprite.Scale(startTime, 480.0f / lightBitmap.Height);
            });
            using(light2SpritePool)
            {
                foreach (var time in burstList)
                {
                    var light = light2SpritePool.Get(time, time + 500);
                    light.Fade(OsbEasing.InOutSine, time, time + 500, 0.2, 0);
                    light.Move(time, 747, 0);
                }
                foreach (var time in whistleList)
                {
                    var light = light2SpritePool.Get(time, time + 500);
                    light.Fade(OsbEasing.InOutSine, time, time + 500, 0.2, 0);
                    light.Move(time, -107, 480);
                }
            }
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
            for(var time = totalStartTime - T * 2; time < totalStartTime - T; time += T/particleCount)
            {
                SingleParticleRotateAroundPerson(path, totalStartTime, totalEndTime, time, T, positionCenterY, moveY, division, scale, radius, opacity, isFlash, isRotate, isFlow, isAdditive);
            }
        }

        public void SingleParticleRotateAroundPerson(string path, double totalStartTime, double totalEndTime, double spawnTime, double T, float positionCenterY, double moveY, double division,  double scale, double radius, double opacity, bool isFlash, bool isRotate, bool isFlow, bool isAdditive)
        {
            // this function is used to generate one particle that rotate around person.
            var layer = GetLayer("Effect");
            var beatDuration = Beatmap.GetTimingPointAt((int)totalStartTime).BeatDuration;

            var particle = layer.CreateSprite(path, OsbOrigin.Centre);
            if (isAdditive) particle.Additive(totalStartTime);

            if (isFlash)
            {
                var loopCount = (int)Math.Floor((totalEndTime - totalStartTime) / (beatDuration / 3));
                // plus beatDuration/3 to make sure no overlap. 
                particle.StartLoopGroup(totalStartTime + beatDuration / 3, loopCount - 1);
                particle.Scale(OsbEasing.OutExpo, 0, beatDuration / 3, scale * 1.5, scale);
                particle.EndGroup();
            }

            int LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime) / (T));
            particle.StartLoopGroup(spawnTime, LoopCount);
            particle.MoveX(OsbEasing.InOutSine, 0, T / 2, 320-radius, 320 + radius);
            particle.MoveX(OsbEasing.InOutSine, T / 2, T, 320 + radius, 320-radius);
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
                var randomT = Random(T/2, T);
                LoopCount = (int)Math.Floor((totalEndTime - totalStartTime - beatDuration/3) / randomT);
                particle.StartLoopGroup(totalStartTime + beatDuration/3, LoopCount);
                particle.ScaleVec(OsbEasing.InOutSine, 0, randomT / 2, 0, scale, scale, scale);
                particle.ScaleVec(OsbEasing.InOutSine, randomT / 2, randomT, scale, scale, 0, scale);
                particle.EndGroup();
            }
            if (!isFlow)
            {
                // will be incapable to implement with flow
                particle.Scale(OsbEasing.InOutSine, totalStartTime, totalStartTime + beatDuration/3, 0, scale);
                particle.Scale(OsbEasing.InOutSine, totalEndTime , totalEndTime + beatDuration/3, scale, 0);
            }
            else
            {
                particle.ScaleVec(OsbEasing.InOutSine, totalStartTime, totalStartTime + beatDuration/3, 0, 0, 0, scale);
                
            }

            LoopCount = (int)Math.Ceiling((totalEndTime - spawnTime - T/8) / (T));
            particle.StartLoopGroup(spawnTime + T/8, LoopCount);
            particle.Fade(OsbEasing.InOutSine, 0, T / 2, opacity, 0);
            particle.Fade(OsbEasing.InOutSine, 4 * T / 5, T, 0, opacity);
            particle.EndGroup();
            
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
                var startTime = Random(StartTime, EndTime);
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
    }
}
