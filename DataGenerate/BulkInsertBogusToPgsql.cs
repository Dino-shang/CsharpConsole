#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：BulkInsertBogusToPgsql
// 创 建 者：Dino
// 创建时间：2025年08月24日 星期日 19:55
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//
//
//----------------------------------------------------------------*/

#endregion

using System.Diagnostics;
using Bogus;
using Npgsql;

namespace ConsoleTest.DataGenerate;

public class BulkInsertBogusToPgsql
{
    public static void Run()
    {
        // 数据库连接配置
        string connStr = "Host=localhost;Database=postgres;Username=postgres;Password=srw1119.";
        string tableName = "huge_data";
        int totalRows = 10_000_000;
        int batchSize = 5000;

        // 字段模板
        var fields = new List<(string Name, string PgType, Func<Faker, object> Generator)>
        {
            ("FullName", "TEXT", f => f.Name.FullName()),
            ("Email", "TEXT", f => f.Internet.Email()),
            ("Phone", "TEXT", f => f.Phone.PhoneNumber()),
            ("City", "TEXT", f => f.Address.City()),
            ("StreetAddress", "TEXT", f => f.Address.StreetAddress()),
            ("CreatedAt", "TIMESTAMP", f => f.Date.Recent(365).ToString("yyyy-MM-dd HH:mm:ss")),
            ("IsActive", "BOOLEAN", f => f.Random.Bool()),
            ("Score", "INT", f => f.Random.Int(0, 100)),
            ("Balance", "NUMERIC", f => f.Finance.Amount()),
            ("UUID", "UUID", f => Guid.NewGuid()),
        };

        using var conn = new NpgsqlConnection(connStr);
        conn.Open();

        // 创建表
        CreateTable(conn, tableName, fields);

        var faker = new Faker("zh_CN");
        int inserted = 0;
        int batchCount = (int)Math.Ceiling((double)totalRows / batchSize);

        var columns = string.Join(", ", fields.Select(f => $"\"{f.Name}\""));

        Console.WriteLine("开始批量插入数据…");
        var sw = Stopwatch.StartNew();

        for (int batch = 0; batch < batchCount; batch++)
        {
            int currentBatchSize = Math.Min(batchSize, totalRows - inserted);
            var rowsData = new List<string>(currentBatchSize);

            for (int i = 0; i < currentBatchSize; i++)
            {
                var row = fields.Select(f => EscapeCsv(f.Generator(faker)));
                rowsData.Add(string.Join(",", row));
            }

            using var writer = conn.BeginTextImport($"COPY \"{tableName}\" ({columns}) FROM STDIN (FORMAT CSV)");
            foreach (var line in rowsData)
            {
                writer.WriteLine(line);
            }

            inserted += currentBatchSize;
            ShowProgress(inserted, totalRows, sw.Elapsed);
        }

        sw.Stop();
        Console.WriteLine($"\n全部数据插入完成！总耗时：{sw.Elapsed.TotalSeconds:F1} 秒。");
    }

    static void CreateTable(NpgsqlConnection conn, string tableName,
        List<(string Name, string PgType, Func<Faker, object> Generator)> fields)
    {
        string sql = $"DROP TABLE IF EXISTS \"{tableName}\";";
        sql += $"\nCREATE TABLE \"{tableName}\" (\n";
        sql += string.Join(",\n", fields.Select(f => $"\"{f.Name}\" {f.PgType}"));
        sql += "\n);";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"已创建表 {tableName}。");
    }

    static string EscapeCsv(object val)
    {
        if (val == null) return "";
        var s = val.ToString().Replace("\"", "\"\"");
        if (s.Contains(",") || s.Contains("\n") || s.Contains("\""))
            return $"\"{s}\"";
        return s;
    }

    static void ShowProgress(int current, int total, TimeSpan elapsed)
    {
        double percent = (double)current / total * 100;
        var speed = current / Math.Max(1, elapsed.TotalSeconds);
        var eta = speed > 0 ? (total - current) / speed : 0;
        Console.Write(
            $"\r已插入：{current:N0}/{total:N0} ({percent:F2}%) | 用时：{elapsed.TotalSeconds:F1}s | 速度：{speed:F1}条/s | 剩余约：{eta:F0}s");
    }
}