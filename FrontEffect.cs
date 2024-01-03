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
    public class FrontEffect : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    var whitePath = @"sb/white.png";
            var whiteBitmap = GetMapsetBitmap(whitePath);

            var vigPath = @"sb/vig.png";
            var vigBitmap = GetMapsetBitmap(vigPath);

            var vig = GetLayer("Vignette").CreateSprite(vigPath, OsbOrigin.Centre);
            vig.Scale(0, 480.0f / vigBitmap.Height);
            vig.Fade(0, 1);
            vig.Fade(417388, 417388 + 1000, 1, 0);

            var beatDuration = Beatmap.GetTimingPointAt(0).BeatDuration;

            var white = GetLayer("White").CreateSprite(whitePath, OsbOrigin.Centre);
            white.ScaleVec(0, 854.0f / whiteBitmap.Width, 480.0f / whiteBitmap.Height);
            white.Additive(0);
            white.Fade(0, 0);
            white.Fade(OsbEasing.OutSine, 60319, 60319+beatDuration * 2, 1,0);
            white.Fade(OsbEasing.OutSine, 87748, 87748+beatDuration * 2, 1,0);
            white.Fade(OsbEasing.InSine, 180328, 185128, 0,1);
            white.Fade(OsbEasing.OutSine, 185128, 185128+beatDuration, 1,0);
            white.Fade(OsbEasing.OutSine, 191611, 191611+beatDuration * 2, 1,0);
            white.Fade(OsbEasing.InSine, 251460, 251803, 0, 1);
            white.Fade(OsbEasing.OutSine, 251803, 251803+beatDuration, 1, 0);
            white.Fade(OsbEasing.InSine, 273746, 273746+beatDuration, 1 ,0);
            white.Fade(OsbEasing.OutSine, 295689, 295689+beatDuration*2, 1, 0);
            white.Fade(OsbEasing.InSine, 338203, 339574, 0, 1);
            white.Fade(OsbEasing.OutSine, 339574, 339574+beatDuration +beatDuration * 2, 1, 0);
            white.Fade(OsbEasing.InSine, 350889, 350889 + beatDuration * 2, 1, 0);
            white.Fade(OsbEasing.InSine, 367003, 369746, 0, 0.3);
            white.Fade(369746, 369746+beatDuration,  0.3, 0);
            white.Fade(OsbEasing.InSine, 377974, 383117, 0, 1);
            white.Fade(OsbEasing.OutSine, 383117, 383117+beatDuration*2, 1, 0);

            var black = GetLayer("Black").CreateSprite(whitePath, OsbOrigin.Centre);
            black.ScaleVec(0, 854.0f / whiteBitmap.Width, 480.0f / whiteBitmap.Height);
            black.Fade(0, 0);
            black.Color(0, Color4.Black);
            black.Fade(OsbEasing.InSine, 87062, 87062+beatDuration*2, 0,1);
            black.Fade(87062+beatDuration*2,0);
            black.Fade(OsbEasing.OutSine, 190277, 191611, 0, 1);
            black.Fade(191611,0);
            black.Fade(OsbEasing.OutSine, 349174, 350889, 0, 1);

            
        }
    }
}
