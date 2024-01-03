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
    public class DeleteBackground : StoryboardObjectGenerator
    {
        [Configurable]
        public string BG="";
        public override void Generate()
        {
		    if(BG=="")
                BG=Beatmap.BackgroundPath ?? string.Empty;
            var bgr=GetLayer("").CreateSprite(BG,OsbOrigin.Centre);
            bgr.Fade(0,0);
            
        }
    }
}
