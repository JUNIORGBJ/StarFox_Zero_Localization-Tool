namespace StarFoxZeroLocalizationTool.Services
{
    internal sealed record GtxAnalysisResult(
        bool Success,
        string Format,
        int? TileMode,
        string Swizzle,
        int? FullSwizzleValue,
        int? InitialSwizzleValue,
        string ComponentSelector,
        bool IsSrgb,
        bool HasLosslessRoundtripRisk,
        string AdvisoryMessage,
        string ToolOutput);

    internal sealed record GtxCommandResult(
        bool Success,
        int ExitCode,
        string Output,
        string ExecutablePath,
        string Arguments);
}
