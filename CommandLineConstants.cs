namespace Birdhouse.CommandLine
{
    public static class CommandLineConstants
    {
        // Например:
        // "... groupName1 argumentName1 Appropriator value1 InGroupSeparator argumentName2 Appropriator value2 GroupSeparator groupName2 argumentName3 Appropriator value3"
        // "... -SamustaiArgs: isDebugBuild = false, version = 1.0.0; -AnotherArgs: overhead = Infinity"

        public const char GroupAppropriator = ':';
        public const char GroupSeparator = ';';
        public const char InGroupSeparator = ',';
        public const char InGroupAppropriator = '=';
    }
}