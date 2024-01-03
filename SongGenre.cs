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
    public class SongGenre : StoryboardObjectGenerator
    {
        [Configurable]
        public string FontName = "Monotype Corsiva";
        [Configurable]
        public string FontPath = "sb/fonts";
        [Configurable]
        public int FontSize = 52;
        [Configurable]
        public Vector2 Padding = Vector2.Zero;
        [Configurable]
        public double FontScale = 0.4f;
        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;
        [Configurable]
        public Color4 FontColor = Color4.White;
        public override void Generate()
        {
		     var font = LoadFont($"{FontPath}/{FontName}", new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle.Regular,
                TrimTransparency = true,
                EffectsOnly = false,
                Debug = false,
            });
            
            var startTime = 49347d;
            var endTime = 383460d;
            generateText(font, "Song Genre", startTime, endTime, 1000, new Vector2(-45, 420), 0.15, false, OsbOrigin.CentreRight);

            generateText(font, "Song BPM", startTime, endTime, 1000, new Vector2(-45, 390), 0.15, false, OsbOrigin.CentreRight);

            var bpmS = Beatmap.TimingPoints;
            var bpmTimingList = new List<double> {};
            var bpmValueList = new List<double> {};
            foreach (var bpm in bpmS)
            {
                if (bpm.IsInherited) continue;
                bpmTimingList.Add(bpm.Offset);
                bpmValueList.Add(bpm.Bpm);
            }
            
            foreach (var timing in bpmTimingList)
            {
                var index = bpmTimingList.IndexOf(timing);
                if (index == bpmTimingList.Count - 1) break;

                var nextTiming = bpmTimingList[index + 1];
                var nextBpm = bpmValueList[index + 1];
                
                if (nextTiming < startTime || timing > endTime) continue;
                if (timing <= startTime)
                    generateText(font, bpmValueList[index].ToString(), startTime, nextTiming, 300, new Vector2(-40, 378), 0.3, true, OsbOrigin.CentreLeft);
                else if (nextTiming >= endTime)
                    generateText(font, bpmValueList[index].ToString(), timing, endTime, 300, new Vector2(-40, 378), 0.3, true, OsbOrigin.CentreLeft);
                else
                    generateText(font, bpmValueList[index].ToString(), timing, nextTiming, 300, new Vector2(-40, 378), 0.3, true, OsbOrigin.CentreLeft);
            }

            var genreTiming = new List<double>{49347, 60319, 87748, 120662, 153927, 191611, 230917, 295689, 339574, 350546};
            var genreDetail = new List<string>{"ambience", "complextro", "glitch hop", "drumstep", "melodic dubstep", "neurofunk", "ambience", "hardcore", "complextro", "extratone"};

            foreach (var time in genreTiming)
            {
                var index = genreTiming.IndexOf(time);
                if (index == genreTiming.Count - 1)    
                {
                    generateText(font, genreDetail[index], time, endTime, 1000, new Vector2(-40, 420 - 12), 0.3, true, OsbOrigin.CentreLeft);
                    break;
                }    
                generateText(font, genreDetail[index], time, genreTiming[index + 1], 1000, new Vector2(-40, 420 - 12), 0.3, true, OsbOrigin.CentreLeft);
            }
            // generateText(font, "Hardrock", startTime, endTime, 1000, new Vector2(-40, 420-12), 0.3, true, OsbOrigin.CentreLeft);
        }

        public void generateText(FontGenerator font, string text, double startTime, double endTime, double moveTime, Vector2 position, double fontScale, bool underline, OsbOrigin origin)
        {
            var textWidth = 0d;
            var maxHeight = 0d;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                textWidth += texture.BaseWidth * fontScale;
                maxHeight = Math.Max(maxHeight, texture.BaseHeight * fontScale);
            }
            var textPosition = position - new Vector2((float)textWidth / 2, 0);
            if ((int)origin % 3 == 0)
            {
                textPosition = position;
            }
            else if ((int)origin % 3 == 2)
            {
                textPosition = position - new Vector2((float)textWidth, 0);

            }

            if (underline)
            {
                var underlineSprite = GetLayer("Text").CreateSprite("sb/p.png", OsbOrigin.CentreLeft, textPosition + new Vector2(0, (float)maxHeight / 2 + 15));
                var pBitmap = GetMapsetBitmap("sb/p.png");
                underlineSprite.ScaleVec(startTime, startTime + moveTime, 0, 1,  textWidth / pBitmap.Width, 1);
                underlineSprite.Fade(startTime, startTime + moveTime, 0, 1);
                underlineSprite.ScaleVec(endTime-moveTime, endTime, textWidth / pBitmap.Width, 1, 0, 1);
            }

            var i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var sprite = GetLayer("Text").CreateSprite(texture.Path, origin, textPosition + texture.OffsetFor(origin) * (float)fontScale);
                    sprite.Scale(startTime, fontScale);
                    sprite.Fade(startTime, startTime + moveTime, 0, 1);
                    sprite.Fade(OsbEasing.OutExpo , endTime - moveTime, endTime, 1, 0);
                    sprite.Color(startTime, FontColor);
                    Log(sprite.PositionAt(startTime));
                }
                
                textPosition.X += texture.BaseWidth * (float)fontScale;
                i++;
            }
        }
    }
}
