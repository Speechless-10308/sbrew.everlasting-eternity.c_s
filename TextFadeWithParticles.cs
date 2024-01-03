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
    public class TextFadeWithParticles : StoryboardObjectGenerator
    {
        [Configurable]
        public string Text = "Forever";
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
        [Configurable]
        public Vector2 Position = new Vector2(320, 240);
        [Configurable]
        public double StartTime = 0;
        [Configurable]
        public double EndTime = 0;
        [Configurable]
        public double MoveTime = 1000;
        [Configurable]
        public double ScrollTime = 100;
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
            
            generateText(font, Text, StartTime, EndTime, MoveTime, Position, ScrollTime);
        }

        public void generateText(FontGenerator font, string text, double startTime, double endTime, double moveTime, Vector2 position, double scrollTime)
        {
            var textWidth = 0d;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                textWidth += texture.BaseWidth * FontScale;
            }
            var textPosition = position - new Vector2((float)textWidth / 2, 0);

            var i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var sprite = GetLayer("Text").CreateSprite(texture.Path, Origin, textPosition + texture.OffsetFor(Origin) * (float)FontScale);
                    sprite.Scale(startTime, FontScale);
                    sprite.Fade(startTime + i * scrollTime, startTime + i * scrollTime + moveTime, 0, 1);
                    sprite.Fade(OsbEasing.OutExpo ,endTime - moveTime, endTime, 1, 0);
                    sprite.Color(startTime, FontColor);
                    generateFadingParticle(texture, textPosition + texture.OffsetFor(Origin) * (float)FontScale, endTime - moveTime, endTime);
                }
                
                textPosition.X += texture.BaseWidth * (float)FontScale;
                i++;
                
            }
        }

        public void generateFadingParticle(FontTexture texture, Vector2 position, double startTime, double endTime)
        {
            var fontPath = texture.Path;
            var particlePath = @"sb\dot.png";

            var fontBitmap = GetMapsetBitmap(fontPath);
            var particleBitmap = GetMapsetBitmap(particlePath);

            for(double x = 0; x < fontBitmap.Width; x += 3)
            {
                for(double y = 0; y < fontBitmap.Height; y += 3)
                {
                    var pixel = fontBitmap.GetPixel((int)x, (int)y);
                    if(pixel.A > 0)
                    {
                        var particle = GetLayer("Particle").CreateSprite(particlePath, OsbOrigin.Centre);
                        particle.Scale(startTime, 6f/particleBitmap.Width);
                        particle.Fade(OsbEasing.OutSine ,startTime, endTime, 1, 0);
                        particle.Color(startTime, pixel);
                        particle.Move(OsbEasing.OutSine ,startTime, endTime, position - new Vector2(fontBitmap.Width/2, fontBitmap.Height/2) * (float)FontScale + new Vector2((float)x, (float)y) * (float)FontScale, position - new Vector2(fontBitmap.Width/2, fontBitmap.Height/2) * (float)FontScale + new Vector2((float)x, (float)y) * (float)FontScale + new Vector2(Random(-10f, 10f), Random(-10f, 10f)));
                    }
                }
            }
        }

    }
}
