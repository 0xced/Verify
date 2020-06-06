﻿using System;
using System.Threading.Tasks;
using Verify;
using VerifyXunit;

#region ScrubbersSampleXunit
public class ScrubbersSample
{
    [VerifyFact]
    public Task Lines()
    {
        var settings = new VerifySettings();
        settings.ScrubLinesWithReplace(
            replaceLine: line =>
            {
                if (line == "LineE")
                {
                    return "NoMoreLineE";
                }
                return line;
            });
        settings.ScrubLines(removeLine: line => line.Contains("J"));
        settings.ScrubLinesContaining("b", "D");
        settings.ScrubLinesContaining(StringComparison.Ordinal, "H");
        return Verifier.Verify(
            settings: settings,
            target: @"
LineA
LineB
LineC
LineD
LineE
LineH
LineI
LineJ
");
    }

    [VerifyFact]
    public Task ScrubberAppliedAfterJsonSerialization()
    {
        var target = new ToBeScrubbed
        {
            RowVersion = "0x00000000000007D3"
        };

        var settings = new VerifySettings();
        settings.AddScrubber(
            input => input.Replace("0x00000000000007D3", "TheRowVersion"));
        return Verifier.Verify(target, settings);
    }
}
#endregion