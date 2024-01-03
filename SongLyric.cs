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
    public class SongLyric : StoryboardObjectGenerator
    {
        [Group("Font")]
        [Description("The name of a system font, or the path to a font relative to your project's folder.\nIt is preferable to add fonts to the project folder and use their file name rather than installing fonts.")]
        [Configurable] public string FontName = "Verdana";
        [Description("A path inside your mapset's folder where lyrics images will be generated.")]
        [Configurable] public string FontPath = "sb/fonts";
        [Description("The Size of the font.\nIncreasing the font size creates larger images.")]
        [Configurable] public int FontSize = 26;
        [Description("The Scale of the font.\nIncreasing the font scale does not creates larger images, but the result may be blurrier.")]
        [Configurable] public float FontScale = 0.5f;
        [Configurable] public Color4 FontColor = Color4.White;
        [Configurable] public System.Drawing.FontStyle FontStyle =System.Drawing.FontStyle.Regular;

        [Group("Outline")]
        [Configurable] public int OutlineThickness = 3;
        [Configurable] public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Group("Shadow")]
        [Configurable] public int ShadowThickness = 0;
        [Configurable] public Color4 ShadowColor = new Color4(0, 0, 0, 100);

        [Group("Glow")]
        [Configurable] public int GlowRadius = 0;
        [Configurable] public Color4 GlowColor = new Color4(255, 255, 255, 100);
        [Configurable] public bool GlowAdditive = true;

        [Group("Misc")]
        [Configurable] public bool PerCharacter = true;
        [Configurable] public bool TrimTransparency = true;
        [Configurable] public bool EffectsOnly = false;
        [Description("How much extra space is allocated around the text when generating it.\nShould be increased when characters look cut off.")]
        [Configurable] public Vector2 Padding = Vector2.Zero;
        [Configurable] public OsbOrigin Origin = OsbOrigin.Centre;
        public override void Generate()
        {
		    var font = LoadFont($"{FontPath}/{FontName}", new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
            },new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            },new FontGlow()
            {
                Radius = GlowRadius,
                Color = GlowColor,
            });

            var glowFont = font;
            if (GlowRadius > 0 && GlowAdditive)
            {
                glowFont = LoadFont(System.IO.Path.Combine($"{FontPath}/{FontName}", "glow"), new FontDescription()
                {
                    FontPath = FontName,
                    FontSize = FontSize,
                    Color = FontColor,
                    Padding = Padding,
                    FontStyle = FontStyle,
                    TrimTransparency = TrimTransparency,
                    EffectsOnly = true,
                },
                new FontGlow()
                {
                    Radius = GlowRadius,
                    Color = GlowColor,
                });
            }
            
            generateTextGlow(font, glowFont, "I'm just creating space for what is still to come", new Vector2(320, 210), 24490, 27405, 0, FontScale);

            generateTextGlow(font, glowFont, "I can't explain.", new Vector2(120, 40), 120319, 128890, 1000, FontScale);

            generateTextGlow(font, glowFont, "I'd go insane.", new Vector2(550, 40), 125805, 128890, 1000, FontScale);

            generateTextGlow(font, glowFont, "Don't say goodbye.", new Vector2(100, 40), 131633, 137119, 1000, FontScale);
            generateTextGlow(font, glowFont, "Don't make me cry.", new Vector2(570, 40), 134376, 137119, 1000, FontScale);
        }

        public void generateTextGlow(FontGenerator font, FontGenerator glowFont, string text, Vector2 position, double startTime, double endTime, double moveTime, float fontScale)
        {
            var glowLayer = GetLayer("glow");
            generatePerCharacter(glowFont, glowLayer, true, position, text, startTime, endTime, moveTime / (text.Length), fontScale);
            var layer = GetLayer("");
            generatePerCharacter(font, layer, true, position, text, startTime, endTime, moveTime / (text.Length), fontScale, 0.8);
        }

        public void generatePerCharacter(FontGenerator font, StoryboardLayer layer, bool additive, Vector2 textPosition, string text, double startTime, double endTime, double intervalTime, float fontScale, double opacity = 1)
        {
            var lineWidth = 0f;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * fontScale;
            }

            var letterX = textPosition.X - lineWidth * 0.5f;

            int i = 0;
            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, textPosition.Y)
                        + texture.OffsetFor(Origin) * fontScale;
                    var realStartTime = startTime + i * intervalTime;
                    var realEndTime = endTime + i * intervalTime;

                    var sprite = layer.CreateSprite(texture.Path, Origin, position);
                    sprite.Scale(realStartTime, fontScale);
                    sprite.Fade(realStartTime - 200, realStartTime, 0, opacity);
                    sprite.Fade(realEndTime - 200, realEndTime, opacity, 0);
                    if (additive) sprite.Additive(realStartTime - 200, realEndTime);
                    if (FontColor != Color4.White) sprite.Color(realStartTime, FontColor);
                }
                letterX += texture.BaseWidth * fontScale;
                i++;
            }
        }
    }
}
