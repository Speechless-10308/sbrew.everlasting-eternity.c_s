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
    public class DeepSea : StoryboardObjectGenerator
    {
        [Group("Colors")]
        [Configurable]
        public Color4 SeaColor = Color4.Blue;
        [Configurable]
        public Color4 BubbleColor = Color4.White;
        [Configurable]
        public Color4 FlareColor = Color4.White;
        [Configurable]
        public Color4 LightColor = Color4.White;
        [Configurable]
        public Color4 Light2Color = Color4.White;

        [Group("Timing")]
        [Configurable]
        public double StartTime = 0;
        [Configurable]
        public double EndTime = 10000;
        [Configurable]
        public int LoopLength = 16;
        public override void Generate()
        {
            string seaPath = @"sb\sea.jpg";
            string bubblePath = @"sb\bubble.png";
            string flarePath = @"sb\flare.png";
            string lightPath = @"sb\light3.png";

            var seaLayer = GetLayer("Sea");
            var effectLayer = GetLayer("Effect");

            var seaBitmap = GetMapsetBitmap(seaPath);
            var bubbleBitmap = GetMapsetBitmap(bubblePath);
            var flareBitmap = GetMapsetBitmap(flarePath);
            var lightBitmap = GetMapsetBitmap(lightPath);
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;


            var sea = seaLayer.CreateSprite(seaPath, OsbOrigin.Centre);
            sea.Fade(OsbEasing.InSine, StartTime, StartTime + 1000, 0, 0.5);
            sea.Scale(StartTime, 1.05 * 480.0f / seaBitmap.Height);
            sea.Color(StartTime, SeaColor);
            sea.Fade(EndTime - 1000, EndTime, 0.5, 0);
            BGFlowRand(sea, StartTime, EndTime, 8, new Vector2(320, 240), OsbEasing.InOutSine, OsbEasing.InOutSine);
            
            

            for (int i = 0; i < 40; i++)
            {
                var light = effectLayer.CreateSprite(lightPath, OsbOrigin.Centre);
                light.Additive(StartTime, EndTime);
                light.Color(StartTime, Light2Color);
                var randomStartTime = Random(StartTime, EndTime);
                var randomMoveTime = Random(1000, 4000f);
                var randomX = Random(350 - 200, 350 + 200f);
                var randomY = Random(-0f, 40f);
                int loopCount = (int)Math.Ceiling((EndTime - randomStartTime) / randomMoveTime);
                if (loopCount > 0)
                {
                    light.StartLoopGroup(randomStartTime, loopCount);
                    light.MoveX(OsbEasing.InOutSine, 0, randomMoveTime, randomX, randomX + Random(-100f, 100f));
                    light.MoveY(OsbEasing.InOutSine, 0, randomMoveTime, randomY, randomY + Random(-10f, 10f));
                    light.Fade(OsbEasing.InOutSine, 0, randomMoveTime / 2, 0, 0.1);
                    light.Fade(OsbEasing.InOutSine, randomMoveTime / 2, randomMoveTime, 0.1, 0);
                    light.ScaleVec(OsbEasing.InOutSine, 0, randomMoveTime, Random(0.1, 0.3), Random(0.1, 0.3), Random(0.1, 0.3), Random(0.1, 0.3));
                }
            }

            var bubblePool = new OsbSpritePool(effectLayer, bubblePath, OsbOrigin.Centre,
            (sprite, startTime, endTime) =>
            {
                sprite.Additive(startTime, endTime);
                sprite.Color(startTime, BubbleColor);
            });

            using (bubblePool)
            {
                for (var time = StartTime; time < EndTime - 1000; time += beatDuration / 4)
                {
                    var moveTime = Random(1000, 2000f);
                    var bubbleScale = Random(0.05, 0.1);
                    var bubblePosition = new Vector2(Random(0, 640f), 480f - Random(0, 50f));
                    var moveVector = new Vector2(Random(-40f, 40f), Random(-200f, -300f));

                    var bubble = bubblePool.Get(time, time + moveTime);
                    bubble.Scale(OsbEasing.OutSine, time, time + 500, 0, bubbleScale);
                    bubble.Scale(OsbEasing.InSine, time + 500, time + moveTime, bubbleScale, 0);
                    bubble.MoveX(OsbEasing.InOutSine, time, time + moveTime / 4, bubblePosition.X, bubblePosition.X + moveVector.X);
                    bubble.MoveY(OsbEasing.InSine, time, time + moveTime, bubblePosition.Y, bubblePosition.Y + moveVector.Y);
                    bubble.MoveX(OsbEasing.InOutSine, time + moveTime / 4, time + moveTime, bubblePosition.X + moveVector.X, bubblePosition.X);
                    bubble.Fade(OsbEasing.InSine, time, time + moveTime / 2, 0, 0.5);
                    bubble.Fade(OsbEasing.InSine, time + moveTime / 2, time + moveTime, 0.5, 0);
                }
            }

            var flare = effectLayer.CreateSprite(flarePath, OsbOrigin.Centre, new Vector2(-107, 0));
            flare.Additive(StartTime, EndTime);
            flare.Color(StartTime, FlareColor);
            flare.Scale(StartTime, 0.5);
            flare.Rotate(OsbEasing.InOutSine, StartTime, EndTime, Random(Math.PI / 16, Math.PI / 8), Random(Math.PI / 8, Math.PI / 4));
            flare.Fade(StartTime, StartTime + 1000, 0, 0.5);

            var light2Path = @"sb\light2.png";
            // first long light is at (220, 0) and rotate 20 degree
            for (int i = 0; i < 4; i++)
            {
                var light = effectLayer.CreateSprite(light2Path, OsbOrigin.TopCentre, new Vector2(220, -30));
                light.Additive(StartTime, EndTime);
                light.Color(StartTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(StartTime, (StartTime + EndTime) / 2);
                var moveTime = (EndTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.1, 0.14), Random(0.1, 0.15), Random(0.1, 0.3), Random(0.9, 1.5));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.1);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(30f, 60f)), MathHelper.DegreesToRadians(Random(30f, 60f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.1, 0);
                light.EndGroup();
            }

            for (int i = 0; i < 7; i++)
            {
                var light = effectLayer.CreateSprite(light2Path, OsbOrigin.TopCentre, new Vector2(320.0f + Random(-10f, 10f), -30));
                light.Additive(StartTime, EndTime);
                light.Color(StartTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(StartTime, (StartTime + EndTime) / 2);
                var moveTime = (EndTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.1, 0.14), Random(0.1, 0.15), Random(0.1, 0.3), Random(0.9, 1.5));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.1);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(-30f, 30f)), MathHelper.DegreesToRadians(Random(-30f, 30f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.1, 0);
                light.EndGroup();
            }

            for (int i = 0; i < 3; i++)
            {
                var light = effectLayer.CreateSprite(light2Path, OsbOrigin.TopCentre, new Vector2(600.0f + Random(-10f, 10f), -30));
                light.Additive(StartTime, EndTime);
                light.Color(StartTime, LightColor);
                var loopCount = Random(1, 7);
                var randomStartTime = Random(StartTime, (StartTime + EndTime) / 2);
                var moveTime = (EndTime - randomStartTime) / loopCount;
                light.StartLoopGroup(randomStartTime, loopCount);
                light.ScaleVec(OsbEasing.InOutSine, 0, moveTime / 8, Random(0.1, 0.14), Random(0.1, 0.15), Random(0.1, 0.3), Random(0.9, 1.5));
                light.Fade(OsbEasing.InOutSine, 0, moveTime / 2, 0, 0.1);
                light.Rotate(OsbEasing.InOutSine, 0, moveTime, MathHelper.DegreesToRadians(Random(-30f, 0f)), MathHelper.DegreesToRadians(Random(-30f, 0f)));
                light.Fade(OsbEasing.InOutSine, moveTime / 2, moveTime, 0.1, 0);
                light.EndGroup();
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
            bg.Rotate(easing1, 0, LoopLength * beatDuration/4, -0.01, -0.02);
            bg.Rotate(easing2, LoopLength * beatDuration/4, 3 * LoopLength * beatDuration/4, -0.02, 0.01);
            bg.Rotate(easing1,3 * LoopLength * beatDuration/4, LoopLength * beatDuration, 0.01, -0.01);
            bg.EndGroup();
        }
    }
}
