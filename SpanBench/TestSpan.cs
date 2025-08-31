#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：TestSpan
// 创 建 者：Dino
// 创建时间：2025年08月31日 星期日 18:46
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/

#endregion

using BenchmarkDotNet.Attributes;

namespace ConsoleTest.SpanBench;

/// <summary>
/// @example: var summary = BenchmarkRunner.Run<SpanPerformanceTests>();
/// | Method               | Mean      | Error     | StdDev    | Rank | Gen0   | Allocated |
// |--------------------- |----------:|----------:|----------:|-----:|-------:|----------:|
//     | UseSubstring         | 3.2810 ns | 0.0791 ns | 0.0701 ns |    2 | 0.0057 |      48 B |
//     | UseAsSpan            | 0.0264 ns | 0.0012 ns | 0.0010 ns |    1 |      - |         - |
//     | UseAsSpanAndToString | 7.7747 ns | 0.1957 ns | 0.4836 ns |    3 | 0.0057 |      48 B |
/// 结论，UseAsSpan最快
/// </summary>
// 定义性能测试类
[MemoryDiagnoser] // 启用内存诊断，查看内存分配情况
[RankColumn]      // 根据性能排名
public class SpanPerformanceTests
{
    private const string OriginalString = "This is a long string that we will use for our performance tests.";
    private const int StartIndex = 15;
    private const int Length = 10;

    // 基准测试方法 1: 使用 Substring
    [Benchmark]
    public string UseSubstring()
    {
        return OriginalString.Substring(StartIndex, Length);
    }

    // 基准测试方法 2: 使用 AsSpan (不分配新字符串)
    [Benchmark]
    public ReadOnlySpan<char> UseAsSpan()
    {
        return OriginalString.AsSpan().Slice(StartIndex, Length);
    }

    // 基准测试方法 3: 使用 AsSpan 后再转换为新字符串
    [Benchmark]
    public string UseAsSpanAndToString()
    {
        return OriginalString.AsSpan().Slice(StartIndex, Length).ToString();
    }
}