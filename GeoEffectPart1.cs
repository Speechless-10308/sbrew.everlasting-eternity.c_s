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
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class GeoEffectPart1 : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var pxPath = @"sb\p.png";
            var circleOutlinePath = @"sb\c4.png";
            var circlePath = @"sb\c3.png";
            var smallCirclePath = @"sb\c.png";

            var beatDuration = Beatmap.GetTimingPointAt(157128).BeatDuration;

            var circleBitmap = GetMapsetBitmap(circleOutlinePath);
            var circleLength = circleBitmap.Height;

            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;
            var angle = MathHelper.DegreesToRadians(10f);

            Prepare(pxPath, beatDuration, pxLength, angle);

            // gonna implement it one by one...
            Drop1(pxPath, beatDuration);
            Drop2(pxPath, beatDuration);

            var circleScale = 100f;

            Drop3(pxPath, circleOutlinePath, beatDuration, circleLength, circleScale);

            // next is a game scene
            GameScene(pxPath, circlePath, smallCirclePath, beatDuration, pxBitmap);

            var startTime = 185927;
            var transTime = 188887;
            var endTime = 190278;
            var particleCount = 100;
            var opacity = 0.5f;
            for (int i = 0; i<particleCount;i++)
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

        }

        private void GameScene(string pxPath, string circlePath, string smallCirclePath, double beatDuration, Bitmap pxBitmap)
        {
            var speed = 200f;   // speed of the whole scene movement, in px per second

            // just create some obstacles
            var totalStartTime = 166727;
            var totalEndTime = 185128;

            // some square and circle obstacles
            createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime, new Vector2(0, 0), speed, new Vector2(200, 100), true, false);
            createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime, new Vector2(30, 250), speed, new Vector2(200, 100), true, false);
            createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime, new Vector2(320, 90), speed, new Vector2(100, 200), true, false);
            createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime + 3 / 4f * beatDuration, new Vector2(420, 230), speed, new Vector2(100, 20), true, true);
            createObstacles(circlePath, totalStartTime, totalEndTime, totalStartTime + 5 / 4f * beatDuration, new Vector2(680, 330), speed, new Vector2(140, 140), true, true);
            // some point
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, totalStartTime + 3 / 4f * beatDuration, totalStartTime + 2 * beatDuration + i * beatDuration / 4f, new Vector2((2 * (float)beatDuration + i * (float)beatDuration / 4f) * speed / 1000f + 320, 400), speed, new Vector2(10, 10), true, true);
            }
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, totalStartTime + 5 / 4f * beatDuration, totalStartTime + 4 * beatDuration + i * beatDuration / 4f, new Vector2((4 * (float)beatDuration + i * (float)beatDuration / 4f) * speed / 1000f + 320, 80), speed, new Vector2(10, 10), true, true);
            }
            //  other Obstacles
            for (int i = 0; i < 2; i++)
            {
                createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime + 3.5 * beatDuration, new Vector2(850 + i * 100, 30 + i * 40), speed, new Vector2(40, 80 + i * 90), true, true);
            }
            for (int i = 0; i < 3; i++)
                createObstacles(pxPath, totalStartTime, totalEndTime, totalStartTime + 6 * beatDuration + i * 3 / 4f * beatDuration, new Vector2(990 + i * 100, 480 - i * 40), speed, new Vector2(60, 80 + i * 90), true, true);
            // point
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, 169727 + i * beatDuration / 8, 170727 + i * beatDuration / 4f, new Vector2((170727 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 100), speed, new Vector2(10, 10), true, true);
            }
            // obstacles
            for (int i = 0; i < 3; i++)
            {
                createObstacles(circlePath, totalStartTime, totalEndTime, 170727 + i * 3 * beatDuration / 4f, new Vector2(Random(1400, 1600), Random(0, 200)), speed, new Vector2(200, 200), true, true);
            }
            // point
            for (int i = 0; i < 4; i++)
            {
                if (i >= 1)
                    createPoint(smallCirclePath, totalStartTime, totalEndTime, 171128 + i * beatDuration / 8, 171527 + i * beatDuration / 4f, new Vector2((171527 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 380), speed, new Vector2(10, 10), true, true);
                else
                    createPoint(smallCirclePath, totalStartTime, totalEndTime, 171128 + i * beatDuration / 8, 171527 + i * beatDuration / 4f, new Vector2((171527 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 100), speed, new Vector2(10, 10), true, true);
            }

            createObstacles(pxPath, totalStartTime, totalEndTime, 172328, new Vector2(1550, 370), speed, new Vector2(90, 160), true, true);

            createObstacles(pxPath, totalStartTime, totalEndTime, 173128, new Vector2(1750, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 173128 + beatDuration * 3 / 4f, new Vector2(1800, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 173128 + beatDuration * 6 / 4f, new Vector2(1850, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 173128 + beatDuration * 9 / 4f, new Vector2(1900, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 173128 + beatDuration * 12 / 4f, new Vector2(1950, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 173128 + beatDuration * 15 / 4f, new Vector2(2000, 100), speed, new Vector2(20, 160), true, true);

            createObstacles(pxPath, totalStartTime, totalEndTime, 174727, new Vector2(1950, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 174727 + beatDuration / 4f, new Vector2(2000, 380), speed, new Vector2(20, 160), true, true);

            createObstacles(pxPath, totalStartTime, totalEndTime, 175027, new Vector2(2050, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027, new Vector2(2050, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration / 4, new Vector2(2100, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration / 4, new Vector2(2100, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration * 2f / 4f, new Vector2(2150, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration * 2f / 4f, new Vector2(2150, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration * 3f / 4f, new Vector2(2200, 100), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175027 + beatDuration * 3f / 4f, new Vector2(2200, 380), speed, new Vector2(20, 160), true, true);

            createObstacles(pxPath, totalStartTime, totalEndTime, 175527, new Vector2(2250, 380), speed, new Vector2(20, 160), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175527 + beatDuration * 1f / 4f, new Vector2(2280, 380), speed, new Vector2(20, 200), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175527 + beatDuration * 2f / 4f, new Vector2(2310, 380), speed, new Vector2(20, 240), true, true);
            createObstacles(pxPath, totalStartTime, totalEndTime, 175527 + beatDuration * 3f / 4f, new Vector2(2340, 380), speed, new Vector2(20, 280), true, true);

            createObstacles(circlePath, totalStartTime, totalEndTime, 176328 + 0 * beatDuration * 2, new Vector2(2500, 100), speed, new Vector2(100, 100), true, true);
            createObstacles(circlePath, totalStartTime, totalEndTime, 176328 + 1 * beatDuration * 2, new Vector2(2650, 380), speed, new Vector2(100, 100), true, true);

            createObstacles(pxPath, totalStartTime, totalEndTime, 177927, new Vector2(2800, 100), speed, new Vector2(20, 160), true, true);
            for (int i = 0; i < 6; i++)
                createObstacles(pxPath, totalStartTime, totalEndTime, 178128 + i * beatDuration / 4f, new Vector2(2950, 0 + i * 40), speed, new Vector2(100, 30), true, true);

            // new things
            createObstacles(pxPath, totalStartTime, totalEndTime, 179527, new Vector2(2950, 90), speed, new Vector2(100, 200), true, false);
            createObstacles(pxPath, totalStartTime, totalEndTime, 179527, new Vector2(3050, 230), speed, new Vector2(100, 20), true, true);
            createObstacles(circlePath, totalStartTime, totalEndTime, 179527, new Vector2(3250, 330), speed, new Vector2(140, 140), true, true);
            // some point
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, 179527, 180328 + i * beatDuration / 4f, new Vector2((180328 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 400), speed, new Vector2(10, 10), true, true);
            }
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, 180727 + i * beatDuration / 16f, 181128 + i * beatDuration / 4f, new Vector2((181128 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 80), speed, new Vector2(10, 10), true, true);
            }
            //  other Obstacles
            for (int i = 0; i < 2; i++)
            {
                createObstacles(pxPath, totalStartTime, totalEndTime, 180927, new Vector2(3400 + i * 100, 30 + i * 40), speed, new Vector2(40, 80 + i * 90), true, true);
            }
            for (int i = 0; i < 3; i++)
                createObstacles(pxPath, totalStartTime, totalEndTime, 181927 + i * 3 / 4f * beatDuration, new Vector2(3530 + i * 100, 480 - i * 40), speed, new Vector2(60, 80 + i * 90), true, true);
            // point
            for (int i = 0; i < 4; i++)
            {
                createPoint(smallCirclePath, totalStartTime, totalEndTime, 182727 + i * beatDuration / 4, 183527 + i * beatDuration / 4f, new Vector2((183527 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 100), speed, new Vector2(10, 10), true, true);
            }
            // obstacles
            for (int i = 0; i < 3; i++)
            {
                createObstacles(circlePath, totalStartTime, totalEndTime, 183527 + i * 3 * beatDuration / 4f, new Vector2(Random(3900, 4200), Random(0, 200)), speed, new Vector2(200, 200), true, true);
            }
            // point
            for (int i = 0; i < 4; i++)
            {
                if (i >= 1)
                    createPoint(smallCirclePath, totalStartTime, totalEndTime, 183927 + i * beatDuration / 8, 184328 + i * beatDuration / 4f, new Vector2((184328 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 380), speed, new Vector2(10, 10), true, true);
                else
                    createPoint(smallCirclePath, totalStartTime, totalEndTime, 183927 + i * beatDuration / 8, 184328 + i * beatDuration / 4f, new Vector2((184328 + i * (float)beatDuration / 4f - totalStartTime) * speed / 1000f + 320, 100), speed, new Vector2(10, 10), true, true);
            }

            createObstacles(pxPath, totalStartTime, totalEndTime, 184328, new Vector2(4050, 370), speed, new Vector2(90, 160), true, true);


            // our player
            var playerScale = new Vector2(20, 20);
            var player = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
            player.ScaleVec(totalStartTime, playerScale / pxBitmap.Width);
            player.MoveX(OsbEasing.OutSine, totalStartTime, totalStartTime + 3 / 4f * beatDuration, 0, 320);
            player.MoveY(OsbEasing.InSine, totalStartTime, totalStartTime + 3 / 4f * beatDuration, 100, 400);
            player.Rotate(totalStartTime, totalEndTime, 0, Math.PI * 10);

            player.MoveY(OsbEasing.InOutBack, 167927, 168227, 400, 80);
            player.MoveY(OsbEasing.InOutBack, 168727, 168727 + beatDuration, 80, 400);
            player.MoveY(OsbEasing.OutBack, 169927 + beatDuration / 4, 169927 + beatDuration, 400, 100);
            player.MoveY(OsbEasing.OutBack, 171527, 171477 + beatDuration / 3, 100, 380);
            player.MoveY(OsbEasing.OutBack, 172328, 172328 + beatDuration, 380, 240);
            player.MoveY(OsbEasing.OutBack, 173128, 173128 + beatDuration, 240, 400);
            player.MoveY(OsbEasing.OutBack, 173927, 173927 + beatDuration, 400, 120);
            player.MoveY(OsbEasing.OutBack, 174727, 174727 + beatDuration, 120, 240);
            player.MoveY(OsbEasing.OutBack, 176328, 176328 + beatDuration, 240, 100);
            player.MoveY(OsbEasing.OutBack, 177128, 177128 + beatDuration, 100, 380);
            player.MoveY(OsbEasing.OutBack, 177927, 177927 + beatDuration, 380, 100);
            player.MoveY(OsbEasing.OutBack, 178727, 178727 + beatDuration, 100, 400);
            player.MoveY(OsbEasing.InOutBack, 180727, 180727 + beatDuration, 400, 80);
            player.MoveY(OsbEasing.OutBack, 181727, 181727 + beatDuration, 80, 400);
            player.MoveY(OsbEasing.OutBack, 182727, 182727 + beatDuration, 400, 100);
            player.MoveY(OsbEasing.OutBack, 184328, 184328 + beatDuration / 2, 100, 380);
            player.MoveY(OsbEasing.OutBack, 184927, 184927 + beatDuration, 380, 80);

            player.Fade(totalStartTime, 1);
            player.Fade(totalEndTime - beatDuration / 4, 0);

            var pxPool = new OsbSpritePool(GetLayer("Effect"), pxPath, OsbOrigin.Centre,
            (sprite, startTime, endTime) =>
            {
                sprite.ScaleVec(startTime, playerScale / pxBitmap.Width / 2);
            });

            using (pxPool)
            {
                for (double time = totalStartTime; time < totalEndTime; time += beatDuration / 4)
                {
                    var px = pxPool.Get(time, time + beatDuration);
                    var moveDirection = (Vector2)player.PositionAt(time) - (Vector2)player.PositionAt(time + 5) + new Vector2(-9, 0);
                    px.Move(OsbEasing.OutSine, time, time + beatDuration, player.PositionAt(time), (Vector2)player.PositionAt(time) + 10 * moveDirection);
                    px.Rotate(OsbEasing.OutExpo, time, time + beatDuration, player.RotationAt(time), player.RotationAt(time) + Math.PI / 2);
                    px.Fade(OsbEasing.OutSine, time, time + beatDuration, 0.2, 0);
                }
            }
        }

        private void createObstacles(string spritePath, double totalStartTime, double totalEndTime, double spawnTime, Vector2 startPos, float speed, Vector2 scale, bool isScaleIn, bool isScaleOut)
        {
            var sprite = GetLayer("Effect").CreateSprite(spritePath, OsbOrigin.Centre);
            var spriteBitmap = GetMapsetBitmap(spritePath);
            var beatDuration = Beatmap.GetTimingPointAt((int)totalStartTime).BeatDuration;
            if (isScaleIn) sprite.ScaleVec(OsbEasing.OutBack, spawnTime, spawnTime + beatDuration / 2, 2 * scale / spriteBitmap.Width, scale / spriteBitmap.Width);
            else sprite.ScaleVec(spawnTime, scale / spriteBitmap.Width);

            var moveTime = (startPos.X + 107 + scale.X / 2) / speed * 1000;
            sprite.Move(totalStartTime, totalStartTime + moveTime, startPos, new Vector2(-107 - scale.X / 2, startPos.Y));

            if (spawnTime != totalStartTime) sprite.Fade(OsbEasing.OutExpo, spawnTime, spawnTime + beatDuration / 2, 0, 1);
            if (moveTime > totalEndTime - totalStartTime)
            {
                if (isScaleOut)
                {
                    sprite.ScaleVec(OsbEasing.InBack, totalEndTime - beatDuration / 2, totalEndTime, scale / spriteBitmap.Width, 0 * scale / spriteBitmap.Width);
                }
                else
                {
                    sprite.ScaleVec(totalEndTime - beatDuration / 2, 0, 0);
                }
            }
        }

        private void createPoint(string spritePath, double totalStartTime, double totalEndTime, double spawnTime, double fadeTime, Vector2 startPos, float speed, Vector2 scale, bool isScaleIn, bool isScaleOut)
        {
            var sprite = GetLayer("Effect").CreateSprite(spritePath, OsbOrigin.Centre);
            var spriteBitmap = GetMapsetBitmap(spritePath);
            var beatDuration = Beatmap.GetTimingPointAt((int)totalStartTime).BeatDuration;
            if (isScaleIn) sprite.ScaleVec(OsbEasing.OutBack, spawnTime, spawnTime + beatDuration / 2, 2 * scale / spriteBitmap.Width, scale / spriteBitmap.Width);
            else sprite.ScaleVec(spawnTime, scale / spriteBitmap.Width);
            Log(sprite.PositionAt(totalStartTime));
            var moveTime = (startPos.X + 107 + scale.X / 2) / speed * 1000;
            sprite.Move(totalStartTime, totalStartTime + moveTime, startPos, new Vector2(-107 - scale.X / 2, startPos.Y));
            if (spawnTime != totalStartTime) sprite.Fade(OsbEasing.OutExpo, spawnTime, spawnTime + beatDuration / 2, 0, 1);
            sprite.ScaleVec(OsbEasing.OutBack, fadeTime, fadeTime + beatDuration / 2, scale / spriteBitmap.Width, 2 * scale / spriteBitmap.Width);
            sprite.Fade(OsbEasing.OutExpo, fadeTime, fadeTime + beatDuration / 2, 1, 0);
        }
        private void Prepare(string pxPath, double beatDuration, int pxLength, float angle)
        {
            var intervals = 20f;
            var blue = GetLayer("Effect").CreateSprite(@"sb\blue.jpg", OsbOrigin.Centre);
            blue.Fade(155527, 0.4f);
            blue.Fade(191611, 0);

            for (int i = 0; i < 16; i++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
                px.ScaleVec(OsbEasing.OutExpo, 153927 + i / 8f * beatDuration, 153927 + i / 8f * beatDuration + beatDuration, 854.0f / Math.Cos(angle) / pxLength, 0, 854.0f / Math.Cos(angle) / pxLength, 30 / pxLength);
                px.Move(OsbEasing.OutExpo, 154727, 154727 + beatDuration, new Vector2(320, 240 - intervals * 7 - 5 + i * intervals), new Vector2(320 - intervals * 7 - 5 + i * intervals, 240));
                px.Rotate(OsbEasing.OutExpo, 154727, 154727 + beatDuration, angle, Math.PI / 2);
                px.Fade(OsbEasing.OutExpo, 155527, 155527 + beatDuration / 2, 1, 0);
            }

            for (int j = 0; j < 6; j++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, new Vector2(320 - intervals * 8 - 5 - intervals * j, 240));
                px.ScaleVec(OsbEasing.OutExpo, 154927 + j / 8f * beatDuration, 154927 + j / 8f * beatDuration + beatDuration, 0, 480.0f / pxLength, intervals / pxLength, 480.0f / pxLength);
                px.Fade(OsbEasing.OutExpo, 155527, 155527 + beatDuration / 2, 1, 0);

                px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, new Vector2(320 + intervals * 8 + 5 + intervals * j, 240));
                px.ScaleVec(OsbEasing.OutExpo, 154927 + j / 8f * beatDuration, 154927 + j / 8f * beatDuration + beatDuration, 0, 480.0f / pxLength, intervals / pxLength, 480.0f / pxLength);
                px.Fade(OsbEasing.OutExpo, 155527, 155527 + beatDuration / 2, 1, 0);
            }

            for (int j = 0; j < 3; j++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, new Vector2(320 - intervals * 13 - 5 - 4 * intervals * j, 240));
                px.ScaleVec(OsbEasing.OutExpo, 155227 + j / 4f * beatDuration, 155527, 0, 480.0f / pxLength, 4 * intervals / pxLength, 480.0f / pxLength);
                px.Fade(OsbEasing.OutExpo, 155527, 155527 + beatDuration / 2, 1, 0);

                px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, new Vector2(320 + intervals * 13 + 5 + 4 * intervals * j, 240));
                px.ScaleVec(OsbEasing.OutExpo, 155227 + j / 4f * beatDuration, 155527, 0, 480.0f / pxLength, 4 * intervals / pxLength, 480.0f / pxLength);
                px.Fade(OsbEasing.OutExpo, 155527, 155527 + beatDuration / 2, 1, 0);
            }
            intervals = 854.0f / 5;
            for (int j = 0; j < 5; j++)
            {
                var px = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, new Vector2(-107 + intervals / 2 + j * intervals, 240));
                px.ScaleVec(OsbEasing.OutExpo, 155527 + 2 * j * beatDuration / 5f, 155527 + 2 * j * beatDuration / 5f + beatDuration, intervals / pxLength, 480.0f / pxLength, 0, 480.0f / pxLength);
            }
            squareInOut(pxPath, 200, 156328, 156328 + beatDuration * 1.5f, OsbEasing.OutExpo, new Vector2(320, 240));
            squareInOutInverse(pxPath, 200, 156727, 156727 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));
        }

        private void Drop3(string pxPath, string circleOutlinePath, double beatDuration, int circleLength, float circleScale)
        {
            squareInOut(pxPath, 200, 163527, 163527 + beatDuration * 1.5, OsbEasing.OutExpo, new Vector2(320, 240));

            var intervals = 30f;
            for (int i = 0; i < 6; i++)
            {
                var circle = GetLayer("Effect").CreateSprite(circleOutlinePath, OsbOrigin.Centre);
                circle.Scale(OsbEasing.OutExpo, 163828 + i / 4f * beatDuration, 164328, 2 * circleScale / circleLength, (1 + i / 8f) * circleScale / circleLength);
                circle.Scale(OsbEasing.OutExpo, 164328, 164328 + beatDuration * 3 / 4, (1 + i / 8f) * circleScale / circleLength, circleScale / circleLength);
                circle.Move(OsbEasing.OutExpo, 164328, 164328 + beatDuration * 3 / 4, new Vector2(320, 240), new Vector2(320 - 30 * 2 - 15, 240) + new Vector2(i * intervals, 0));

                if (i % 2 == 0)
                {
                    circle.Scale(OsbEasing.OutExpo, 164328 + beatDuration * 3 / 4 + i / 2 * beatDuration / 2, 164328 + beatDuration * 3 / 4 + i / 2 * beatDuration / 2 + beatDuration, circleScale / circleLength, circleScale / circleLength * 3);
                    circle.Fade(OsbEasing.OutExpo, 164328 + beatDuration * 3 / 4 + i / 2 * beatDuration / 2, 164328 + beatDuration * 3 / 4 + i / 2 * beatDuration / 2 + beatDuration, 1, 0);
                }
                else
                {
                    circle.Move(OsbEasing.OutExpo, 165128, 165128 + beatDuration, new Vector2(320 - 30 * 2 - 15, 240) + new Vector2(i * intervals, 0), new Vector2(320, 240) + intervals * new Vector2((float)Math.Sin(Math.Floor(i / 2f) * Math.PI * 2f / 3f), (float)Math.Cos(Math.Floor(i / 2f) * Math.PI * 2f / 3f)));

                    circle.Move(OsbEasing.OutExpo, 165628, 165628 + beatDuration / 2, new Vector2(320, 240) + intervals * new Vector2((float)Math.Sin(Math.Floor(i / 2f) * Math.PI * 2f / 3f), (float)Math.Cos(Math.Floor(i / 2f) * Math.PI * 2f / 3f)), new Vector2(320, 240));
                    circle.Scale(OsbEasing.OutExpo, 165628, 165628 + beatDuration / 2, circleScale / circleLength, circleScale / circleLength * (1 + i / 8f));

                    circle.Scale(OsbEasing.OutExpo, 165927 - beatDuration / 8f + (i * 3) * beatDuration / 8f, beatDuration + 165927 + (i * 3) * beatDuration / 8f, circleScale / circleLength * (1 + i / 8f), circleScale / circleLength);
                    circle.Fade(OsbEasing.OutExpo, 165927 - beatDuration / 8f + (i * 3) * beatDuration / 8f, beatDuration + 165927 + (i * 3) * beatDuration / 8f, 1, 0);
                }

            }
        }

        private void Drop2(string pxPath, double beatDuration)
        {
            squareInOut(pxPath, 200, 160328, 160328 + beatDuration * 1.5, OsbEasing.OutExpo, new Vector2(320, 240));
            squareInOutInverse(pxPath, 200, 160727, 160727 + beatDuration * 1.5, OsbEasing.OutExpo, new Vector2(320, 240));

            for (int i = 0; i < 2; i++)
            {
                squareInOutInverse(pxPath, 200, 160927 + i / 4f * beatDuration, 160927 + i / 4f * beatDuration + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));
            }
            squareInOut(pxPath, 500, 161128, 161128 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));

            var angle = MathHelper.DegreesToRadians(-45f);
            var deltaAngle = MathHelper.DegreesToRadians(18f);
            var length = 100d;
            for (int i = 0; i < 6; i++)
            {
                var spriteList = squareOutlineScale(pxPath, 3, 161128 + i / 4f * beatDuration, 161128 + i / 4f * beatDuration + beatDuration, OsbEasing.OutExpo, length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), angle, new Vector2(320, 240));
                length *= (Math.Sin(deltaAngle) + Math.Cos(deltaAngle));
                angle += deltaAngle;
                spriteList.All(sprite => { sprite.Fade(161628, 161694, 1, 0); return true; });
            }

            squareInOutInverse(pxPath, 400, 161628, 161628 + beatDuration * 1.5, OsbEasing.OutExpo, new Vector2(320, 240));

            angle = MathHelper.DegreesToRadians(-45f);
            deltaAngle = MathHelper.DegreesToRadians(18f);
            length = 100d;
            for (int i = 0; i < 6; i++)
            {
                var spriteList = squareOutlineScale(pxPath, 3, 161828 + i / 4f * beatDuration, 161828 + i / 4f * beatDuration + beatDuration, OsbEasing.OutExpo, length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), angle, new Vector2(320, 240));
                length *= (Math.Sin(deltaAngle) + Math.Cos(deltaAngle));
                angle += deltaAngle;
                spriteList.All(sprite => { sprite.Fade(162328, 162394, 1, 0); return true; });
            }

            squareInOutInverse(pxPath, 400, 162328, 162328 + beatDuration * 1.5, OsbEasing.OutExpo, new Vector2(320, 240));

            squareInOut(pxPath, 500, 162727, 162727 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));

            angle = MathHelper.DegreesToRadians(-25f);
            deltaAngle = MathHelper.DegreesToRadians(10f);
            length = 100d;
            for (int i = 0; i < 8; i++)
            {
                var spriteList = squareOutlineScale(pxPath, 3, 162727 + i / 8f * beatDuration, 162727 + i / 8f * beatDuration + beatDuration, OsbEasing.OutExpo, length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), angle, new Vector2(320, 240));
                length *= (Math.Sin(deltaAngle) + Math.Cos(deltaAngle));
                angle += deltaAngle;
                spriteList.All(sprite => { sprite.Fade(163128, 163194, 1, 0); return true; });
            }

            squareInOutInverse(pxPath, 400, 163128, 163128 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));
            squareInOutInverse(pxPath, 400, 163328, 163328 + beatDuration / 2, OsbEasing.OutExpo, new Vector2(320, 240));
        }

        private void Drop1(string pxPath, double beatDuration)
        {
            squareInOut(pxPath, 200, 157128, 157128 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));

            for (int i = 0; i < 8; i++)
            {
                var scale = new Vector2(Random(60, 100f), Random(200, 300f));
                var position = new Vector2(Random(0, 640f), Random(120, 360f));
                var squareList = squareOutlineScale(pxPath, 4, 157527 + i * beatDuration / 8, 157527 + (i + 1) * beatDuration / 8, OsbEasing.OutExpo, 1.4 * scale.X, scale.X, 1.4 * scale.Y, scale.Y, position + Random(0.1f, 0.2f) * (position - new Vector2(320, 240)), position);
                squareList.All(sprite => { sprite.Fade(157927, 157927 + beatDuration / 2, 1, 0); return true; });
            }

            squareInOutInverse(pxPath, 500, 157927, 157927 + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));

            for (int i = 0; i < 3; i++)
            {
                squareInOutInverse(pxPath, 500, 158227 + i / 2f * beatDuration, 158227 + i / 2f * beatDuration + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));
            }

            for (int i = 0; i < 6; i++)
            {
                squareInOut(pxPath, 500, 158861 + i / 3f * beatDuration, 158861 + i / 3f * beatDuration + beatDuration, OsbEasing.OutExpo, new Vector2(320, 240));
            }

            sunShow(159527, 159527 + beatDuration / 1.2, beatDuration, 16, new Vector2(20, 70), 90, new Vector2(320, 240), 200f);

            var angle = MathHelper.DegreesToRadians(-25f);
            var deltaAngle = MathHelper.DegreesToRadians(10f);
            var length = 100d;
            for (int i = 0; i < 8; i++)
            {
                var spriteList = squareOutlineScale(pxPath, 3, 159927 + i / 8f * beatDuration, 159927 + i / 8f * beatDuration + beatDuration, OsbEasing.OutExpo, length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), length * (Math.Sin(deltaAngle) + Math.Cos(deltaAngle)), angle, new Vector2(320, 240));
                length *= (Math.Sin(deltaAngle) + Math.Cos(deltaAngle));
                angle += deltaAngle;
                spriteList.All(sprite => { sprite.Fade(160328, 160394, 1, 0); return true; });
            }
        }

        public void squareInOut(string pxPath, double diagonalLength, double startTime, double endTime, OsbEasing easing, Vector2 centrePosition)
        {
            // this effect actually comes from tochi-. I looked at his osb and try to implement this effect.
            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;
            Log($"pxLength: {pxLength}");

            // this effect consist 4 square, there angles are Math.PI/4, -Math.PI/4, 3 * Math.PI/4 and 5 * Math.PI/4, they sepepately go to right, down, left and up by diagonalLength. their origins are both OsbOrigin.BottomCentre, and they start their scaleVec to be (50 / pxLength, 25 / pxLength), end by (0, Math.Sqrt(2) * diagonalLength / pxLength).

            var square1 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square2 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square3 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square4 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);

            square1.ScaleVec(easing, startTime, endTime, 50.0f / pxLength, 25.0f / pxLength, 0, Math.Sqrt(2) * diagonalLength / pxLength);
            square1.Move(easing, startTime, endTime, centrePosition, centrePosition + new Vector2((float)diagonalLength, 0));
            square1.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square1.Rotate(startTime, -Math.PI / 4);
            Log($"square1: {square1.PositionAt(endTime)} {square1.OpacityAt(startTime)} {square1.ScaleAt(startTime)} {square1.RotationAt(startTime)} {square1.GetTexturePathAt(startTime)}  {square1.CommandCount}");

            square2.ScaleVec(easing, startTime, endTime, 50.0f / pxLength, 25.0f / pxLength, 0, Math.Sqrt(2) * diagonalLength / pxLength);
            square2.Move(easing, startTime, endTime, centrePosition, centrePosition + new Vector2(0, (float)diagonalLength));
            square2.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square2.Rotate(startTime, Math.PI / 4);

            square3.ScaleVec(easing, startTime, endTime, 50.0f / pxLength, 25.0f / pxLength, 0, Math.Sqrt(2) * diagonalLength / pxLength);
            square3.Move(easing, startTime, endTime, centrePosition, centrePosition + new Vector2(-(float)diagonalLength, 0));
            square3.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square3.Rotate(startTime, 3 * Math.PI / 4);

            square4.ScaleVec(easing, startTime, endTime, 50.0f / pxLength, 25.0f / pxLength, 0, Math.Sqrt(2) * diagonalLength / pxLength);
            square4.Move(easing, startTime, endTime, centrePosition, centrePosition + new Vector2(0, -(float)diagonalLength));
            square4.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square4.Rotate(startTime, 5 * Math.PI / 4);
        }

        public void squareInOutInverse(string pxPath, double diagonalLength, double startTime, double endTime, OsbEasing easing, Vector2 centrePosition)
        {
            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;

            var square1 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square2 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square3 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);
            var square4 = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.BottomCentre);

            square1.ScaleVec(easing, startTime, endTime, 0, Math.Sqrt(2) * diagonalLength / pxLength, 50.0f / pxLength, 25.0f / pxLength);
            square1.Move(easing, startTime, endTime, centrePosition + new Vector2((float)diagonalLength, 0), centrePosition);
            // square1.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square1.Rotate(startTime, -Math.PI / 4);

            square2.ScaleVec(easing, startTime, endTime, 0, Math.Sqrt(2) * diagonalLength / pxLength, 50.0f / pxLength, 25.0f / pxLength);
            square2.Move(easing, startTime, endTime, centrePosition + new Vector2(0, (float)diagonalLength), centrePosition);
            // square2.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square2.Rotate(startTime, Math.PI / 4);

            square3.ScaleVec(easing, startTime, endTime, 0, Math.Sqrt(2) * diagonalLength / pxLength, 50.0f / pxLength, 25.0f / pxLength);
            square3.Move(easing, startTime, endTime, centrePosition + new Vector2(-(float)diagonalLength, 0), centrePosition);
            // square3.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square3.Rotate(startTime, 3 * Math.PI / 4);

            square4.ScaleVec(easing, startTime, endTime, 0, Math.Sqrt(2) * diagonalLength / pxLength, 50.0f / pxLength, 25.0f / pxLength);
            square4.Move(easing, startTime, endTime, centrePosition + new Vector2(0, -(float)diagonalLength), centrePosition);
            // square4.Fade(OsbEasing.None, startTime + 3f / 5f * (endTime - startTime), startTime + 4f / 5f * (endTime - startTime), 1, 0);
            square4.Rotate(startTime, 5 * Math.PI / 4);
        }

        public List<OsbSprite> squareOutlineScale(string pxPath, double width, double startTime, double endTime, OsbEasing easing, double startLenX, double endLenX, double startLenY, double endLenY, Vector2 startPos, Vector2 endPos)
        {
            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;

            // upper square
            var squareUp = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
            if (startLenX != endLenX)
                squareUp.ScaleVec(easing, startTime, endTime, startLenX / pxLength, width / pxLength, endLenX / pxLength, width / pxLength);
            if (startPos != endPos)
                squareUp.Move(easing, startTime, endTime, startPos + new Vector2(0, -(float)startLenY / 2 + (float)width / 2), endPos + new Vector2(0, -(float)endLenY / 2 + (float)width / 2));

            // right square
            var squareRight = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
            if (startLenY != endLenY)
                squareRight.ScaleVec(easing, startTime, endTime, width / pxLength, startLenY / pxLength, width / pxLength, endLenY / pxLength);
            if (startPos != endPos)
                squareRight.Move(easing, startTime, endTime, startPos + new Vector2((float)startLenX / 2 - (float)width / 2, 0), endPos + new Vector2((float)endLenX / 2 - (float)width / 2, 0));

            // bottom square
            var squareBottom = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
            if (startLenX != endLenX)
                squareBottom.ScaleVec(easing, startTime, endTime, startLenX / pxLength, width / pxLength, endLenX / pxLength, width / pxLength);
            if (startPos != endPos)
                squareBottom.Move(easing, startTime, endTime, startPos + new Vector2(0, (float)startLenY / 2 - (float)width / 2), endPos + new Vector2(0, (float)endLenY / 2 - (float)width / 2));

            // left square
            var squareLeft = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre);
            if (startLenY != endLenY)
                squareLeft.ScaleVec(easing, startTime, endTime, width / pxLength, startLenY / pxLength, width / pxLength, endLenY / pxLength);
            if (startPos != endPos)
                squareLeft.Move(easing, startTime, endTime, startPos + new Vector2(-(float)startLenX / 2 + (float)width / 2, 0), endPos + new Vector2(-(float)endLenX / 2 + (float)width / 2, 0));

            return new List<OsbSprite> { squareUp, squareRight, squareBottom, squareLeft };
        }

        public List<OsbSprite> squareOutlineScale(string pxPath, double width, double startTime, double endTime, OsbEasing easing, double startLenX, double endLenX, double startLenY, double endLenY, double angle, Vector2 centerPosition)
        {
            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;

            // upper square
            var squareUp = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, centerPosition + new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle)) * (float)(startLenY / 2 - width / 2));
            if (startLenX != endLenX)
                squareUp.ScaleVec(easing, startTime, endTime, startLenX / pxLength, width / pxLength, endLenX / pxLength, width / pxLength);
            else
                squareUp.ScaleVec(startTime, startLenX / pxLength, width / pxLength);
            squareUp.Rotate(startTime, angle);

            // right square
            var squareRight = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, centerPosition + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (float)(startLenX / 2 - width / 2));
            if (startLenY != endLenY)
                squareRight.ScaleVec(easing, startTime, endTime, width / pxLength, startLenY / pxLength, width / pxLength, endLenY / pxLength);
            else
                squareRight.ScaleVec(startTime, width / pxLength, startLenY / pxLength);
            squareRight.Rotate(startTime, angle);

            // bottom square
            var squareBottom = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, centerPosition + new Vector2(-(float)Math.Sin(angle), (float)Math.Cos(angle)) * (float)(startLenY / 2 - width / 2));
            if (startLenX != endLenX)
                squareBottom.ScaleVec(easing, startTime, endTime, startLenX / pxLength, width / pxLength, endLenX / pxLength, width / pxLength);
            else
                squareBottom.ScaleVec(startTime, startLenX / pxLength, width / pxLength);
            squareBottom.Rotate(startTime, angle);

            // left square
            var squareLeft = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.Centre, centerPosition + new Vector2(-(float)Math.Cos(angle), -(float)Math.Sin(angle)) * (float)(startLenX / 2 - width / 2));
            if (startLenY != endLenY)
                squareLeft.ScaleVec(easing, startTime, endTime, width / pxLength, startLenY / pxLength, width / pxLength, endLenY / pxLength);
            else
                squareLeft.ScaleVec(startTime, width / pxLength, startLenY / pxLength);
            squareLeft.Rotate(startTime, angle);

            return new List<OsbSprite> { squareUp, squareRight, squareBottom, squareLeft };
        }

        public void sunShow(double startTime, double endTime, double beatDuration, double lightCount, Vector2 lightScale, double sunScale, Vector2 centrePosition, float radius)
        {
            string pxPath = @"sb\p.png";
            string circlePath = @"sb\c.png";

            var pxBitmap = GetMapsetBitmap(pxPath);
            var pxLength = pxBitmap.Height;

            var circleBitmap = GetMapsetBitmap(circlePath);
            var circleLength = circleBitmap.Height;

            var sun = GetLayer("Effect").CreateSprite(circlePath, OsbOrigin.Centre, centrePosition);
            sun.Scale(OsbEasing.OutBack, startTime, endTime, 0, sunScale / circleLength);
            sun.Scale(OsbEasing.InBack, endTime, endTime + beatDuration, sunScale / circleLength, 0);
            var angle = 0d;
            for (int i = 0; i < lightCount; i++)
            {
                var lightPosition = centrePosition + new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle)) * radius;
                var lightPosition2 = centrePosition + new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle)) * (radius + 20);
                var light = GetLayer("Effect").CreateSprite(pxPath, OsbOrigin.TopCentre);
                light.ScaleVec(OsbEasing.OutExpo, startTime, endTime, lightScale.X / pxLength, lightScale.Y / pxLength, lightScale.X / pxLength, 0);
                light.Move(OsbEasing.OutExpo, startTime, endTime, lightPosition, lightPosition2);
                light.Rotate(startTime, angle);
                angle += Math.PI * 2d / (int)lightCount;
            }
        }
    }
}
