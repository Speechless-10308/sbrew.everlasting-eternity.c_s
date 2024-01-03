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
    public class GeoEffectPart2 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 LightColor = Color4.WhiteSmoke;
        public override void Generate()
        {
            var pxPath = @"sb\p.png";
            var dotPath = @"sb\dot.png";
            var beatDuration = Beatmap.GetTimingPointAt(339574).BeatDuration;

            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;
            var angle = MathHelper.DegreesToRadians(10f);

            var totalStartTime = 339574;
            var totalEndTime = 383117;

            var bg = GetLayer("BG").CreateSprite(@"sb\blue.jpg", OsbOrigin.Centre);
            bg.Fade(totalStartTime, 0.3);
            bg.Fade(totalEndTime, 0);

            var intervals = 50f;
            var scale = new Vector2(10, intervals);
            for (int i = 0; i < 4; i++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre, new Vector2(320 - intervals - intervals / 2 + i * intervals, 240));
                px.ScaleVec(OsbEasing.OutExpo, totalStartTime + i * beatDuration / 4, totalStartTime + beatDuration + i * beatDuration / 4, new Vector2(0, 0), scale / 2 / pxLength);
                px.ScaleVec(OsbEasing.OutExpo, totalStartTime + beatDuration * 2 + i * beatDuration / 4, totalStartTime + beatDuration * 3 + i * beatDuration / 4, scale / 2 / pxLength, scale / pxLength);
                px.Rotate(OsbEasing.OutExpo, totalStartTime + 4 * beatDuration + i * beatDuration / 4, totalStartTime + 5 * beatDuration + i * beatDuration / 4, 0, Math.PI / 2);
                px.Fade(OsbEasing.OutSine, totalStartTime + 6 * beatDuration + i * beatDuration / 4, totalStartTime + 7 * beatDuration + i * beatDuration / 4, 1, 0);
            }
            totalStartTime = 342317;
            for (int i = 0; i < 4; i++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.TopCentre, new Vector2(320 - intervals - intervals / 2 + i * intervals, 240));
                px.ScaleVec(OsbEasing.OutExpo, totalStartTime + i * beatDuration / 4, totalStartTime + beatDuration + i * beatDuration / 4, new Vector2(0, 0), scale / 2 / pxLength);
                px.ScaleVec(OsbEasing.OutExpo, totalStartTime + beatDuration * 2 + i * beatDuration / 4, totalStartTime + beatDuration * 3 + i * beatDuration / 4, scale / 2 / pxLength, scale / pxLength);
                px.Rotate(OsbEasing.OutExpo, totalStartTime + 4 * beatDuration + i * beatDuration / 4, totalStartTime + 5 * beatDuration + i * beatDuration / 4, 0, Math.PI / 2);
                px.Fade(OsbEasing.OutSine, totalStartTime + 6 * beatDuration + i * beatDuration / 4, totalStartTime + 7 * beatDuration + i * beatDuration / 4, 1, 0);
            }
            var startTime = 345060d;
            var transTime = 347803d;
            var endTime = 349174d;
            var particleCount = 100;
            var opacity = 0.5f;
            for (int i = 0; i < particleCount; i++)
            {
                var randomStartPos = new Vector2(Random(0, 640f), Random(0, 480f));
                var randomMoveDistance = Random(80f, 140f);
                var bitmap = GetMapsetBitmap(@"sb\s.png");

                var px = GetLayer("Effect").CreateSprite(@"sb\s.png", OsbOrigin.Centre, randomStartPos);
                px.Fade(OsbEasing.OutSine, startTime, startTime + beatDuration / 2, 0, opacity);
                px.MoveY(OsbEasing.OutExpo, startTime, transTime, randomStartPos.Y, randomStartPos.Y - randomMoveDistance);
                px.MoveY(OsbEasing.InExpo, transTime, endTime + beatDuration / 2, randomStartPos.Y - randomMoveDistance, randomStartPos.Y - 2 * randomMoveDistance);
                px.Fade(OsbEasing.InSine, endTime, endTime + beatDuration / 2, opacity, 0);
                px.Scale(startTime, Random(4f, 10f) / bitmap.Height);
            }

            var path = dotPath;
            startTime = 350889d;
            endTime = 383117d;
            var T = 6000f;
            var radius = 150;
            var positionCenterY = 240f;
            var moveY = 20f;
            var particleScale = 0.4f;
            particleCount = 20;
            var particleList = ParticleRotate(startTime, endTime, particleCount, path, T, radius, positionCenterY, moveY, particleScale);

            var spritePool = new OsbSpritePool(GetLayer("Effect"), path, OsbOrigin.Centre, (sprite, starttime, endtime) =>
            {
                sprite.Additive(starttime, endtime);
            });

            using (spritePool)
            {
                for (var time = startTime; time < endTime; time += beatDuration/6)
                {
                    foreach (var particle in particleList)
                    {
                        var endPosition = (Vector2)particle.PositionAt(time);
                        var deltaPos = (Vector2)particle.PositionAt(time - 5) - endPosition;

                        var sprite = spritePool.Get(time, time + 1400);
                        sprite.Move(OsbEasing.OutSine ,time, time + 1400, endPosition + new Vector2(500 * deltaPos.X, 10000 * deltaPos.Y), endPosition);
                        sprite.Fade(OsbEasing.InSine, time, time + 1400, 1, 0);
                    }
                }
            }

            string light2Path = @"sb\light2.png";
            var lightBitmap = GetMapsetBitmap(light2Path);
            var light2SpritePool = new OsbSpritePool(GetLayer("Effect"), light2Path, OsbOrigin.Centre, (sprite, starttime, endtime) =>
            {
                sprite.Additive(starttime);
                sprite.Color(starttime, LightColor);
                sprite.Scale(starttime, 480.0f / lightBitmap.Height);
            });

            var burstStartTime = startTime;
            var burstEndTime = endTime;
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
            using(light2SpritePool)
            {
                foreach (var time in burstList)
                {
                    var light = light2SpritePool.Get(time, time + 500);
                    light.Fade(OsbEasing.InOutSine, time, time + 500, 0.2, 0);
                    light.Move(time, 680, 20);
                }
                foreach (var time in whistleList)
                {
                    var light = light2SpritePool.Get(time, time + 500);
                    light.Fade(OsbEasing.InOutSine, time, time + 500, 0.2, 0);
                    light.Move(time, 0, 460);
                }
            }
            var white = GetLayer("Effects").CreateSprite(@"sb\white.png", OsbOrigin.Centre);
            white.Fade(383117, 383117 + beatDuration * 4, 1, 1);
            white.Color(383117, Color4.Black);
            white.ScaleVec(383117, 854.0f / 5, 480.0f / 5);

        }

        private List<OsbSprite> ParticleRotate(double startTime, double endTime, int particleCount, string path, float T, int radius, float positionCenterY, float moveY, float particleScale)
        {
            var spriteList = new List<OsbSprite>();
            for (var spawnTime = startTime - T * 2; spawnTime < startTime - T; spawnTime += T / particleCount)
            {
                var particle = singleParticleRotate(startTime, endTime, path, T, spawnTime, radius, positionCenterY, moveY, particleScale);
                particle.Fade(startTime, startTime + 500, 0, 1);
                particle.Fade(endTime - 500, endTime, 1, 0);
                spriteList.Add(particle);
            }
            return spriteList;
        }

        private OsbSprite singleParticleRotate(double startTime, double endTime, string path, float T, double spawnTime, int radius, float positionCenterY, float moveY, float particleScale)
        {
            var particle = GetLayer("Effect").CreateSprite(path, OsbOrigin.Centre);
            particle.Additive(startTime, endTime);

            int loopCount = (int)Math.Ceiling((endTime - spawnTime) / T);
            particle.StartLoopGroup(spawnTime, loopCount);
            particle.MoveX(OsbEasing.InOutSine, 0, T / 2, 320 - radius, 320 + radius);
            particle.MoveX(OsbEasing.InOutSine, T / 2, T, 320 + radius, 320 - radius);
            particle.EndGroup();

            loopCount = (int)Math.Ceiling((endTime - spawnTime - T / 4) / T);
            particle.StartLoopGroup(spawnTime + T / 4, loopCount);
            particle.MoveY(OsbEasing.InOutSine, 0, T / 2, positionCenterY + moveY, positionCenterY - moveY);
            particle.MoveY(OsbEasing.InOutSine, T / 2, T, positionCenterY - moveY, positionCenterY + moveY);
            particle.EndGroup();

            particle.Scale(startTime, particleScale);

            return particle;
        }
    }
}
