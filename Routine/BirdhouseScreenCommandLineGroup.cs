using System;
using Birdhouse.Extended.CommandLine.Attributes;
using UnityEngine;

namespace Birdhouse.Extended.CommandLine.Routine
{
    [RuntimeCommandLineGroup("-BirdhouseScreenBindings:")]
    public static class BirdhouseScreenCommandLineGroup
    {
        [RuntimeCommandLineArgument("resolution")]
        private static void SetResolution(string argument)
        {
            var split = argument.Split('x');
            
            var canParseX = int.TryParse(split[0], out var x);
            var canParseY = int.TryParse(split[1], out var y);
            if (!canParseX || !canParseY)
            {
                throw new ArgumentException($"Can't parse \"{argument}\" as screen resolution! Try write like that: \"1920x1080\"");
            }
            
            var resolution = new Vector2Int(x, y);
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
        }

        [RuntimeCommandLineArgument("screenMode")]
        private static void SetScreenMode(string argument)
        {
            var canParse = Enum.TryParse<FullScreenMode>(argument, true, out var result);
            if (!canParse)
            {
                throw new ArgumentException($"Can't parse \"{argument}\" as {nameof(FullScreenMode)} enum!");
            }
            
            Screen.SetResolution(Screen.width, Screen.height, result);
        }

        [RuntimeCommandLineArgument("msaaSamples")]
        private static void SetMsaaSamples(string argument)
        {
            var canParse = int.TryParse(argument, out var result);
            if (!canParse)
            {
                throw new ArgumentException($"Can't parse \"{argument}\" as integer! (trying to set MSAA samples count)");
            }

            Screen.SetMSAASamples(result);
        }

        [RuntimeCommandLineArgument("targetFramerate")]
        private static void SetTargetFramerate(string argument)
        {
            var canParse = int.TryParse(argument, out var targetFrameRate);
            if (!canParse)
            {
                throw new ArgumentException($"Can't parse \"{argument}\" as integer! (trying to set target framerate)");
            }
            
            Application.targetFrameRate = targetFrameRate;
        }

        [RuntimeCommandLineArgument("brightness")]
        private static void SetBrightness(string argument)
        {
            var canParse = float.TryParse(argument, out var brightness);
            if (!canParse)
            {
                throw new ArgumentException($"Can't parse \"{argument}\" as float! (trying to set brightness)");
            }

            Screen.brightness = brightness;
        }
    }
}
