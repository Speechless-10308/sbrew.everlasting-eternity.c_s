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
    public class RevolvingScene : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 LineColor = Color4.White;
        public override void Generate()
        {
            // line path
            string linePath = @"sb\white.png";
            // cg part path
            string shacklesPath0 = @"sb\cg\part\shackles_0.jpg";
            string shacklesPath1 = @"sb\cg\part\shackles_1.jpg";
            string shacklesPath2 = @"sb\cg\part\shackles_2.jpg";
            string shacklesPath3 = @"sb\cg\part\shackles_3.jpg";
            string waterPath0 = @"sb\cg\part\water_0.jpg";
            string waterPath1 = @"sb\cg\part\water_1.jpg";
            string waterPath2 = @"sb\cg\part\water_2.jpg";
            string waterPath3 = @"sb\cg\part\water_3.jpg";
            // decoration path
            string birdPath = @"sb\cg\decorate\bird.png";
            string shacklePath = @"sb\cg\decorate\shackles.png";
            string fishPath = @"sb\cg\decorate\fish.png";
            string visionPath = @"sb\cg\decorate\vision.png";
            // end cg path
            string lockPath = @"sb\cg\locked.jpg";
            string mirrorPath = @"sb\cg\mirror.jpg";
            string realGodPath = @"sb\cg\real_god.jpg";
            string waterPath = @"sb\cg\water.jpg";
            string water2Path = @"sb\cg\water2.jpg";
            string cryingPath = @"sb\cg\crying.jpg";

            var layer = GetLayer("Revolve");
            var switchTime = 271003;
            var switchEndTime = 272374;
            var beatDuration = Beatmap.GetTimingPointAt((int)switchTime).BeatDuration;


            // some settings
            // speed: which control the speed of move. it is the pixels of move per second.
            var speed = 50f;
            // initPosition: the position of cg when it is created.
            var initPosition = new Vector2(500, 240);
            // lineWidth: the width of line.
            var lineWidth = 1.5f;
            // intervals: the intervals between cg and line.
            var intervals = 0f;
            // totalStartTime: the start time of the whole scene.
            var totalStartTime = 251803;
            // lineColor: the color of line.
            var lineColor = LineColor;

            // the first pic show
            // will first  draw a blank cg like the original video, then overlay the part.
            // some control things of this cg.
            // cgScale: the scale of cg.
            var cgScale = 0.2f;
            // blankOffset: the offset of blank cg.
            var blankOffset = new Vector2(90, 70);
            // blankSize: the size of blank cg.
            var blankSize = new Vector2(280, 60);
            // some computation
            var moveTime = (blankSize.Y + initPosition.Y + blankOffset.Y) / speed * 1000;

            // blank cg
            drawLines(blankSize, totalStartTime, totalStartTime + moveTime, intervals, lineColor, linePath, lineWidth * 0.7f, initPosition + blankOffset, initPosition + blankOffset - new Vector2(0, blankSize.Y + initPosition.Y + blankOffset.Y));
            // cg
            // also need computation
            var cgBitmap = GetMapsetBitmap(shacklesPath0);
            moveTime = (cgBitmap.Height * cgScale + initPosition.Y) / speed * 1000;
            drawLineWithPicture(shacklesPath0, linePath, lineWidth, totalStartTime, totalStartTime + moveTime, initPosition, initPosition - new Vector2(0, cgBitmap.Height * cgScale + initPosition.Y), cgScale, intervals, lineColor);

            // some decorations
            // shakles
            // configureThingsHere!
            // scale: the scale of shakles.
            var scale = 0.4f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            var offsetTime = 2600;
            // spriteX: the x position of this sprite.
            var spriteX = 300;
            // some computations
            decorations(shacklePath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the second pic show
            // configureThingsHere!
            // cgScale: the scale of cg.
            cgScale = 0.16f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(140, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(80, 200);
            // initX: the x position of cg.
            var initX = 90;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 5300;

            notFirstCgMove(shacklesPath1, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize);

            // some decorations: bird
            // configureThingsHere!
            // scale: the scale of bird.
            scale = 0.4f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 7800;
            // spriteX: the x position of this sprite.
            spriteX = 250;
            decorations(birdPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the third pic show
            // configureThingsHere!
            // cgScale: the scale of cg.
            cgScale = 0.2f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(90, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(280, 100);
            // initX: the x position of cg.
            initX = 510;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 8200;

            notFirstCgMove(shacklesPath2, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize);

            // some decorations: shackles
            // configureThingsHere!
            // scale: the scale of shackles.
            scale = 0.4f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 12300;
            // spriteX: the x position of this sprite.
            spriteX = 300;
            decorations(shacklePath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the fourth pic show
            // configureThingsHere!
            // cgScale: the scale of cg.
            cgScale = 0.2f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(180, 70);
            // blankSize: the size of blank cg. 
            blankSize = new Vector2(80, 200);
            // initX: the x position of cg.
            initX = 90;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 14000;

            notFirstCgMove(shacklesPath3, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize, 271003 + 300);

            // some decorations: shackles
            // configureThingsHere!
            // scale: the scale of shackles.
            scale = 0.4f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 16500;
            // spriteX: the x position of this sprite.
            spriteX = 400;
            decorations(birdPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale, 271003 + 300);


            cgFlowIn(OsbEasing.OutSine, switchTime, switchTime + beatDuration * 2, new Vector2(320, 240), waterPath, Math.PI / 10, switchEndTime + beatDuration);

            whiteTrans(switchTime);

            cgIn(OsbEasing.OutExpo, switchEndTime, switchEndTime + 500, new Vector2(320, 240), water2Path, 273746 + beatDuration);


            // a new beginning
            // configure new things here!
            // speed: which control the speed of move. it is the pixels of move per second.
            speed = 80f;
            // initPosition: the position of cg when it is created.
            initPosition = new Vector2(500, 140);
            // totalStartTime: the start time of the new beginning scene.
            totalStartTime = 273746;
            // cgScale: the scale of cg.
            cgScale = 0.4f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(90, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(280, 60);
            // some computation
            moveTime = (blankSize.Y + initPosition.Y + blankOffset.Y) / speed * 1000;


            // blank cg
            drawLines(blankSize, totalStartTime, totalStartTime + moveTime, intervals, lineColor, linePath, lineWidth * 0.7f, initPosition + blankOffset, initPosition + blankOffset - new Vector2(0, blankSize.Y + initPosition.Y + blankOffset.Y));
            // cg
            // also need computation
            cgBitmap = GetMapsetBitmap(waterPath0);
            moveTime = (cgBitmap.Height * cgScale + initPosition.Y) / speed * 1000;
            drawLineWithPicture(waterPath0, linePath, lineWidth, totalStartTime, totalStartTime + moveTime, initPosition, initPosition - new Vector2(0, cgBitmap.Height * cgScale + initPosition.Y), cgScale, intervals, lineColor);

            // some decorations
            // fish
            // configureThingsHere!
            // scale: the scale of fish.
            scale = 0.8f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 1600;
            // spriteX: the x position of this sprite.
            spriteX = 300;
            decorations(fishPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the second pic show
            // configureThingsHere!
            // cgScale: the scale of cg.   
            cgScale = 0.4f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(140, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(80, 200);
            // initX: the x position of cg.
            initX = 90;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 3300;

            notFirstCgMove(waterPath1, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize);

            // some decorations: vision
            // configureThingsHere!
            // scale: the scale of vision.
            scale = 0.8f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 4800;
            // spriteX: the x position of this sprite.
            spriteX = 270;
            decorations(visionPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the third pic show
            // configureThingsHere!
            // cgScale: the scale of cg.
            cgScale = 0.4f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(80, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(330, 100);
            // initX: the x position of cg.
            initX = 480;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 5700;

            notFirstCgMove(waterPath2, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize);

            // some decorations: fish
            // configureThingsHere!
            // scale: the scale of fish.
            scale = 0.8f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 6000;
            // spriteX: the x position of this sprite.
            spriteX = 100;
            decorations(fishPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);

            // the fourth pic show
            // configureThingsHere!
            // cgScale: the scale of cg.
            cgScale = 0.6f;
            // blankOffset: the offset of blank cg.
            blankOffset = new Vector2(180, 70);
            // blankSize: the size of blank cg.
            blankSize = new Vector2(80, 200);
            // initX: the x position of cg.
            initX = 90;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 9000;

            notFirstCgMove(waterPath3, totalStartTime, offsetTime, initPosition, initX, speed, cgScale, intervals, lineColor, linePath, lineWidth, blankOffset, blankSize, 284717);

            // some decorations: fish
            // configureThingsHere!
            // scale: the scale of fish.
            scale = 0.8f;
            // offset time: the time when it passed the last cg - the time when the last cg was created.
            offsetTime = 9500;
            // spriteX: the x position of this sprite.
            spriteX = 400;
            decorations(fishPath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale, 284717);

            // cg show show
            var mirror = layer.CreateSprite(mirrorPath, OsbOrigin.Centre);
            var mirrorBitmap = GetMapsetBitmap(mirrorPath);
            
            var CgShowTime = 284717d;
            mirror.Fade(CgShowTime, 1);
            mirror.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / mirrorBitmap.Width * 1.8f, 854.0f / mirrorBitmap.Width * 1.7f);
            mirror.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(30, 280), new Vector2(20, 290));

            CgShowTime += 2*beatDuration;
            mirror.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / mirrorBitmap.Width * 1.8f, 854.0f / mirrorBitmap.Width * 1.7f);
            mirror.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(600, 120), new Vector2(610, 110));

            CgShowTime += 2*beatDuration;
            mirror.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / mirrorBitmap.Width * 1.8f, 854.0f / mirrorBitmap.Width * 1.7f);
            mirror.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(600, 280), new Vector2(590, 290));

            CgShowTime += 2*beatDuration;
            mirror.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / mirrorBitmap.Width * 1.2f, 854.0f / mirrorBitmap.Width * 1.0f);
            mirror.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(310, 250), new Vector2(320, 240));

            CgShowTime += 2*beatDuration;
            mirror.Fade(CgShowTime, 0);

            
            var crying = layer.CreateSprite(cryingPath, OsbOrigin.Centre);
            var cryingBitmap = GetMapsetBitmap(cryingPath);
            crying.Fade(CgShowTime, 1);
            crying.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / cryingBitmap.Width * 1.8f, 854.0f / cryingBitmap.Width * 1.7f);
            crying.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(40, 300), new Vector2(30, 310));

            CgShowTime += 2*beatDuration;
            crying.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / cryingBitmap.Width * 1.8f, 854.0f / cryingBitmap.Width * 1.7f);
            crying.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(40, 80), new Vector2(50, 70));

            CgShowTime += 2*beatDuration;
            crying.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / cryingBitmap.Width * 1.8f, 854.0f / cryingBitmap.Width * 1.7f);
            crying.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(600, 300), new Vector2(590, 310));

            CgShowTime += 2*beatDuration;
            crying.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / cryingBitmap.Width * 1.2f, 854.0f / cryingBitmap.Width * 1.0f);
            crying.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(310, 250), new Vector2(320, 240));

            CgShowTime += 2*beatDuration;
            crying.Fade(CgShowTime, 0);

            for (var cgTime = 284717d; cgTime < CgShowTime; cgTime += 2 * beatDuration)
            {
                whiteTrans(cgTime, beatDuration / 2);
            }

            var locked = layer.CreateSprite(lockPath, OsbOrigin.Centre);
            var lockedBitmap = GetMapsetBitmap(lockPath);
            locked.Fade(CgShowTime, 1);
            locked.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / lockedBitmap.Width * 1.5f, 854.0f / lockedBitmap.Width * 1.4f);
            locked.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 0), new Vector2(320, 10));
            locked.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            var real_god = layer.CreateSprite(realGodPath, OsbOrigin.Centre);
            var real_godBitmap = GetMapsetBitmap(realGodPath);
            real_god.Fade(CgShowTime, 1);
            real_god.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / real_godBitmap.Width * 1.5f, 854.0f / real_godBitmap.Width * 1.4f);
            real_god.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, -290), new Vector2(320, -300));
            real_god.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            locked.Fade(CgShowTime, 1);
            locked.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / lockedBitmap.Width * 1.5f, 854.0f / lockedBitmap.Width * 1.4f);
            locked.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 370), new Vector2(320, 360));
            locked.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            real_god.Fade(CgShowTime, 1);
            real_god.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / real_godBitmap.Width * 1.5f, 854.0f / real_godBitmap.Width * 1.4f);
            real_god.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 330), new Vector2(320, 340));
            real_god.Fade(CgShowTime+beatDuration, 0);  

            CgShowTime += beatDuration;
            locked.Fade(CgShowTime, 1);
            locked.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / lockedBitmap.Width * 1.5f, 854.0f / lockedBitmap.Width * 1.4f);
            locked.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 880), new Vector2(320, 870));
            locked.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            real_god.Fade(CgShowTime, 1);
            real_god.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / real_godBitmap.Width * 1.5f, 854.0f / real_godBitmap.Width * 1.4f);
            real_god.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 610), new Vector2(320, 620));
            real_god.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            locked.Fade(CgShowTime, 1);
            locked.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / lockedBitmap.Width * 1.5f, 854.0f / lockedBitmap.Width * 1.4f);
            locked.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(150, -740), new Vector2(150, -750));
            locked.Fade(CgShowTime+beatDuration, 0);

            CgShowTime += beatDuration;
            real_god.Fade(CgShowTime, 1);
            real_god.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, 854.0f / real_godBitmap.Width * 1.5f, 854.0f / real_godBitmap.Width * 1.4f);
            real_god.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration, new Vector2(320, 860), new Vector2(320, 870));

            CgShowTime += beatDuration;
            real_god.Scale(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration*3, 854.0f / real_godBitmap.Width * 1.5f, 854.0f / real_godBitmap.Width * 1.0f);
            real_god.Move(OsbEasing.OutSine, CgShowTime, CgShowTime+beatDuration*2, new Vector2(320, 200), new Vector2(320, 570));
            real_god.Fade(CgShowTime+beatDuration*4, 0);


            for (var cgTime = 290203d; cgTime < CgShowTime + 1; cgTime += beatDuration)
            {
                whiteTrans(cgTime, beatDuration / 4);
            }

            whiteTrans(CgShowTime+4*beatDuration, beatDuration * 1.8f);
            


        }

        public void drawLineWithPicture(string cgPath, string linePath, double lineWidth, double startTime, double endTime, Vector2 startPosition, Vector2 endPosition, double cgScale, float intervals, Color4 lineColor, double fadeTime = 0)
        {
            var cgBitmap = GetMapsetBitmap(cgPath);
            var cgVec = new Vector2(cgBitmap.Width, cgBitmap.Height);

            var layer = GetLayer("Revolve");

            // draw cg and let it move
            var cg = layer.CreateSprite(cgPath, OsbOrigin.Centre);
            cg.Scale(startTime, cgScale);
            cg.Move(startTime, endTime, startPosition, endPosition);

            if (fadeTime != 0)
            {
                cg.Fade(fadeTime, fadeTime + 100, 1, 0);
            }

            // draw line
            drawLines(cgVec * (float)cgScale, startTime, endTime, intervals, lineColor, linePath, lineWidth, startPosition, endPosition, fadeTime);
        }

        public void drawLines(Vector2 targetSizeVector, double startTime, double endTime, float intervals, Color4 lineColor, string linePath, double lineWidth, Vector2 startPosition, Vector2 endPosition, double fadeTime = 0)
        {
            var layer = GetLayer("Revolve");
            var lineBitmap = GetMapsetBitmap(linePath);

            var left = layer.CreateSprite(linePath, OsbOrigin.Centre);
            left.ScaleVec(startTime, lineWidth / lineBitmap.Width, (targetSizeVector.Y + intervals * 2 + lineWidth) / lineBitmap.Height);
            left.Move(startTime, endTime, startPosition + new Vector2(-targetSizeVector.X / 2 - intervals, 0), endPosition + new Vector2(-targetSizeVector.X / 2 - intervals, 0));
            left.Color(startTime, lineColor);

            var right = layer.CreateSprite(linePath, OsbOrigin.Centre);
            right.ScaleVec(startTime, lineWidth / lineBitmap.Width, (targetSizeVector.Y + intervals * 2 + lineWidth) / lineBitmap.Height);
            right.Move(startTime, endTime, startPosition + new Vector2(targetSizeVector.X / 2 + intervals, 0), endPosition + new Vector2(targetSizeVector.X / 2 + intervals, 0));
            right.Color(startTime, lineColor);

            var up = layer.CreateSprite(linePath, OsbOrigin.Centre);
            up.ScaleVec(startTime, (targetSizeVector.X + intervals * 2 + lineWidth) / lineBitmap.Width, lineWidth / lineBitmap.Height);
            up.Move(startTime, endTime, startPosition + new Vector2(0, -targetSizeVector.Y / 2 - intervals), endPosition + new Vector2(0, -targetSizeVector.Y / 2 - intervals));
            up.Color(startTime, lineColor);

            var down = layer.CreateSprite(linePath, OsbOrigin.Centre);
            down.ScaleVec(startTime, (targetSizeVector.X + intervals * 2 + lineWidth) / lineBitmap.Width, lineWidth / lineBitmap.Height);
            down.Move(startTime, endTime, startPosition + new Vector2(0, targetSizeVector.Y / 2 + intervals), endPosition + new Vector2(0, targetSizeVector.Y / 2 + intervals));
            down.Color(startTime, lineColor);

            if (fadeTime != 0)
            {
                left.Fade(fadeTime, fadeTime + 100, 1, 0);
                right.Fade(fadeTime, fadeTime + 100, 1, 0);
                up.Fade(fadeTime, fadeTime + 100, 1, 0);
                down.Fade(fadeTime, fadeTime + 100, 1, 0);
            }
        }

        public void singleLine(string linePath, double lineWidth, double lineHeight, double startTime, double endTime, Vector2 startPosition, Vector2 endPosition, Color4 lineColor, double fadeTime = 0)
        {
            var layer = GetLayer("Revolve");
            var lineBitmap = GetMapsetBitmap(linePath);

            var line = layer.CreateSprite(linePath, OsbOrigin.Centre);
            line.ScaleVec(startTime, lineWidth / lineBitmap.Width, lineHeight / lineBitmap.Height);
            line.Move(startTime, endTime, startPosition, endPosition);
            line.Color(startTime, lineColor);

            if (fadeTime != 0)
            {
                line.Fade(fadeTime, fadeTime + 100, 1, 0);
            }
        }

        public Tuple<double, Vector2> GetStartConfig(string spritePath, double totalStartTime, float offsetTime, Vector2 firstCgInitPos, float preparedX, float speed, float scale = 1f)
        {
            // We need to see if it can be seen at the totalStartTime. If so, create them at the totalStartTime and compute where this sprite is at totalStartTime; if not, then just create them as totalStartTime + offsetTime and set the initial position Y to 480 + bitmap.Height/2 + linewidth.
            var spriteBitmap = GetMapsetBitmap(spritePath);
            Log($"{spritePath} height: {spriteBitmap.Height}");
            var spriteStartTime = totalStartTime + offsetTime - (480 - firstCgInitPos.Y) / speed * 1000;
            var spriteInitPosition = new Vector2(preparedX, firstCgInitPos.Y + offsetTime / 1000 * speed);
            Log($"{spritePath} scale: {scale}");
            Log($"{spritePath} initPosition: {spriteInitPosition}");
            Log($"{spritePath} threshold: {480 + spriteBitmap.Height * scale / 2}");
            if (spriteInitPosition.Y > 480 + spriteBitmap.Height * scale / 2)
            {
                spriteInitPosition = new Vector2(preparedX, 480 + spriteBitmap.Height * scale / 2);
            }
            else
            {
                spriteStartTime = totalStartTime;
            }
            Log($"{spritePath} initPosition: {spriteInitPosition}");
            Log($"{spritePath} startTime: {spriteStartTime}");
            return new Tuple<double, Vector2>(spriteStartTime, spriteInitPosition);
        }

        public void decorations(string spritePath, double totalStartTime, float offsetTime, Vector2 initPosition, float spriteX, float speed, float scale = 1f, double fadeTime = 0)
        {
            var spriteBitmap = GetMapsetBitmap(spritePath);
            var spriteStartTuple = GetStartConfig(spritePath, totalStartTime, offsetTime, initPosition, spriteX, speed, scale);
            var spriteStartTime = spriteStartTuple.Item1;
            var spriteInitPosition = spriteStartTuple.Item2;
            var moveTime = (spriteInitPosition.Y + spriteBitmap.Height / 2) / speed * 1000;

            var layer = GetLayer("Revolve");
            // create sprite
            var shackleDeco = layer.CreateSprite(spritePath, OsbOrigin.Centre);
            shackleDeco.Scale(spriteStartTime, scale);
            shackleDeco.Move(spriteStartTime, spriteStartTime + moveTime, spriteInitPosition, spriteInitPosition - new Vector2(0, spriteBitmap.Height / 2 + spriteInitPosition.Y));
            if (fadeTime != 0)
            {
                shackleDeco.Fade(fadeTime, fadeTime + 100, 1, 0);
            }
        }

        public void notFirstCgMove(string spritePath, double totalStartTime, float offsetTime, Vector2 initPosition, float initX, float speed, float cgScale, float intervals, Color4 lineColor, string linePath, float lineWidth, Vector2 blankOffset, Vector2 blankSize, double fadeTime = 0)
        {
            // some computation
            var cgBitmap = GetMapsetBitmap(spritePath);
            var spriteStartTuple = GetStartConfig(spritePath, totalStartTime, offsetTime, initPosition, initX, speed, cgScale);
            var spriteStartTime = spriteStartTuple.Item1;
            var spriteInitPosition = spriteStartTuple.Item2;

            // blank cg
            var moveTime = (blankSize.Y + spriteInitPosition.Y + blankOffset.Y) / speed * 1000;
            drawLines(blankSize, spriteStartTime, spriteStartTime + moveTime, intervals, lineColor, linePath, lineWidth * 0.7f, spriteInitPosition + blankOffset, spriteInitPosition + blankOffset - new Vector2(0, blankSize.Y + spriteInitPosition.Y + blankOffset.Y), fadeTime);

            // cgMove
            moveTime = (spriteInitPosition.Y + cgBitmap.Height * cgScale / 2) / speed * 1000;
            drawLineWithPicture(spritePath, linePath, lineWidth, spriteStartTime, spriteStartTime + moveTime, spriteInitPosition, spriteInitPosition - new Vector2(0, cgBitmap.Height * cgScale / 2 + spriteInitPosition.Y), cgScale, intervals, lineColor, fadeTime);
        }

        public void cgFlowIn(OsbEasing easing, double startTime, double endTime, Vector2 position, string cgPath, double rotateAngle, double endFadeTime)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;
            var cgBitmap = GetMapsetBitmap(cgPath);
            var cg = GetLayer("Revolve").CreateSprite(cgPath, OsbOrigin.Centre);
            cg.Scale(easing, startTime, endTime, 854.0f / cgBitmap.Width * 1.3f, 854.0f / cgBitmap.Width);
            cg.Rotate(easing, startTime, endTime, rotateAngle, 0);
            cg.Move(easing, startTime, endTime, position + new Vector2(10, 10), position);

            cg.Fade(endFadeTime - beatDuration, endFadeTime, 1, 0);
        }

        public void cgIn(OsbEasing easing, double startTime, double endTime, Vector2 position, string cgPath, double endFadeTime, float scaleFactor = 1.0f)
        {
            var beatDuration = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;
            var cgBitmap = GetMapsetBitmap(cgPath);
            var cg = GetLayer("Revolve").CreateSprite(cgPath, OsbOrigin.Centre);
            cg.Scale(easing, startTime, endTime, 854.0f / cgBitmap.Width * scaleFactor, 854.0f / cgBitmap.Width);
            cg.Fade(easing, endFadeTime - beatDuration, endFadeTime, 1, 0);
            cg.Fade(easing, startTime, endTime, 0, 1);
        }
        public void whiteTrans(double startTime) => whiteTrans(startTime, 400);

        public void whiteTrans(double startTime, double moveTime)
        {
            var layer = GetLayer("Revolve");
            var white = layer.CreateSprite(@"sb\white.png", OsbOrigin.Centre);
            white.Fade(startTime - moveTime, startTime, 0, 1);
            white.Fade(startTime, startTime + moveTime, 1, 0);
            var whiteBitmap = GetMapsetBitmap(@"sb\white.png");
            white.ScaleVec(startTime - moveTime, 854.0f / whiteBitmap.Width, 480.0f / whiteBitmap.Height);
            white.Additive(startTime - moveTime, startTime + moveTime);
        }
    }
}
