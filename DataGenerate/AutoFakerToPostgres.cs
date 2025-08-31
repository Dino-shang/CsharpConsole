#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：AutoFakerToPostgres
// 创 建 者：Dino
// 创建时间：2025年08月19日 星期二 20:55
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
// 
//
//----------------------------------------------------------------*/

#endregion

using Bogus;
using Npgsql;

namespace ConsoleTest.DataGenerate;

public class AutoFakerToPostgres
{
    // 字段模板定义
    static readonly List<FieldTemplate> Templates = new List<FieldTemplate>
    {
        new FieldTemplate("FullName", "TEXT", f => f.Name.FullName()),
        new FieldTemplate("FirstName", "TEXT", f => f.Name.FirstName()),
        new FieldTemplate("LastName", "TEXT", f => f.Name.LastName()),
        new FieldTemplate("Email", "TEXT", f => f.Internet.Email()),
        new FieldTemplate("Phone", "TEXT", f => f.Phone.PhoneNumber()),
        new FieldTemplate("Country", "TEXT", f => f.Address.Country()),
        new FieldTemplate("City", "TEXT", f => f.Address.City()),
        new FieldTemplate("StreetAddress", "TEXT", f => f.Address.StreetAddress()),
        new FieldTemplate("ZipCode", "TEXT", f => f.Address.ZipCode()),
        new FieldTemplate("Company", "TEXT", f => f.Company.CompanyName()),
        new FieldTemplate("JobTitle", "TEXT", f => f.Name.JobTitle()),
        new FieldTemplate("DateOfBirth", "DATE",
            f => f.Date.Past(30, DateTime.Today.AddYears(-18)).ToString("yyyy-MM-dd")),
        new FieldTemplate("CreatedAt", "TIMESTAMP", f => f.Date.Recent(30).ToString("yyyy-MM-dd HH:mm:ss")),
        new FieldTemplate("IsActive", "BOOLEAN", f => f.Random.Bool()),
        new FieldTemplate("Score", "INT", f => f.Random.Int(0, 100)),
        new FieldTemplate("Balance", "NUMERIC", f => f.Finance.Amount()),
        new FieldTemplate("UUID", "UUID", f => Guid.NewGuid()),
        new FieldTemplate("Website", "TEXT", f => f.Internet.Url()),
        new FieldTemplate("Color", "TEXT", f => f.Commerce.Color()),
        new FieldTemplate("Product", "TEXT", f => f.Commerce.ProductName()),
        new FieldTemplate("Department", "TEXT", f => f.Commerce.Department()),
        new FieldTemplate("Avatar", "TEXT", f => f.Internet.Avatar()),
        new FieldTemplate("IP", "TEXT", f => f.Internet.Ip()),
        new FieldTemplate("MAC", "TEXT", f => f.Internet.Mac()),
        new FieldTemplate("Latitude", "NUMERIC", f => f.Address.Latitude()),
        new FieldTemplate("Longitude", "NUMERIC", f => f.Address.Longitude()),
        new FieldTemplate("Sentence", "TEXT", f => f.Lorem.Sentence()),
        new FieldTemplate("Paragraph", "TEXT", f => f.Lorem.Paragraph()),
        new FieldTemplate("Comment", "TEXT", f => f.Lorem.Text()),
        new FieldTemplate("FileName", "TEXT", f => f.System.FileName()),
        new FieldTemplate("MimeType", "TEXT", f => f.System.MimeType()),
        new FieldTemplate("UserAgent", "TEXT", f => f.Internet.UserAgent()),
        new FieldTemplate("Currency", "TEXT", f => f.Finance.Currency().Code),
        new FieldTemplate("CreditCard", "TEXT", f => f.Finance.CreditCardNumber()),
        new FieldTemplate("ISBN", "TEXT", f => f.Commerce.Ean13()),
        new FieldTemplate("Car", "TEXT", f => f.Vehicle.Model()),
        new FieldTemplate("CarType", "TEXT", f => f.Vehicle.Type()),
        new FieldTemplate("CarFuel", "TEXT", f => f.Vehicle.Fuel()),
        new FieldTemplate("CarVIN", "TEXT", f => f.Vehicle.Vin()),
        new FieldTemplate("MusicGenre", "TEXT", f => f.Music.Genre()),
        new FieldTemplate("Password", "TEXT", f => f.Internet.Password()),
        new FieldTemplate("Emoji", "TEXT", f => f.Random.Replace("😀😁😂🤣😃😄😅😆😉😊")),
        // 还可继续添加更多字段，总共100个
    };

    public static void Run()
    {
        Console.WriteLine("欢迎使用万能数据生成脚本！");
        Console.WriteLine($"模板字段总数：{Templates.Count}");

        // 用户选择字段
        Console.WriteLine("请输入需要使用的字段数量（如20），或者输入字段名（用逗号分隔），如FullName,Email,Phone。留空则随机20个字段：");
        var fieldInput = Console.ReadLine()?.Trim();
        List<FieldTemplate> selectedFields;

        if (string.IsNullOrEmpty(fieldInput))
        {
            selectedFields = Templates.OrderBy(_ => Guid.NewGuid()).Take(20).ToList();
        }
        else if (int.TryParse(fieldInput, out int n) && n > 0)
        {
            selectedFields = Templates.OrderBy(_ => Guid.NewGuid()).Take(n).ToList();
        }
        else
        {
            var names = fieldInput.Split(',').Select(s => s.Trim()).ToList();
            selectedFields = Templates.Where(t => names.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList();
            if (selectedFields.Count == 0)
            {
                Console.WriteLine("未找到有效字段，自动随机选择20个字段。");
                selectedFields = Templates.OrderBy(_ => Guid.NewGuid()).Take(20).ToList();
            }
        }

        Console.WriteLine("选择字段如下：");
        Console.WriteLine(string.Join(", ", selectedFields.Select(f => f.Name)));

        // PG连接参数
        Console.WriteLine("请输入Postgres连接串（如Host=localhost;Username=postgres;Password=xxx;Database=testdb）：");
        var connStr = Console.ReadLine()?.Trim();

        // 表名
        Console.WriteLine("请输入要创建的表名（如auto_data）：");
        var tableName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(tableName)) tableName = "auto_data";

        // 数据量
        Console.WriteLine("请输入要生成的数据总量（如100000）：");
        int totalRows = int.TryParse(Console.ReadLine(), out var rows) ? rows : 10000;

        // 批量大小自动选择（简单策略：1000~5000）
        int batchSize = totalRows >= 10000 ? 2000 : 500;
        Console.WriteLine($"自动选择每批插入{batchSize}条数据。");

        // 创建表
        using var conn = new NpgsqlConnection(connStr);
        conn.Open();
        CreateTable(conn, tableName, selectedFields);

        var faker = new Faker("zh_CN");
        int inserted = 0;
        int batchCount = (int)Math.Ceiling((double)totalRows / batchSize);

        for (int batch = 0; batch < batchCount; batch++)
        {
            int currentBatchSize = Math.Min(batchSize, totalRows - inserted);
            var rowsData = new List<Dictionary<string, object>>(currentBatchSize);

            for (int i = 0; i < currentBatchSize; i++)
            {
                var row = new Dictionary<string, object>();
                foreach (var field in selectedFields)
                {
                    row[field.Name] = field.Generator(faker);
                }

                rowsData.Add(row);
            }

            BulkInsert(conn, tableName, selectedFields, rowsData);
            inserted += currentBatchSize;
            ShowProgress(inserted, totalRows);
        }

        Console.WriteLine("\n全部数据插入完成！");
    }

    // 字段模板
    public class FieldTemplate
    {
        public string Name { get; set; }
        public string PgType { get; set; }
        public Func<Faker, object> Generator { get; set; }

        public FieldTemplate(string name, string pgType, Func<Faker, object> generator)
        {
            Name = name;
            PgType = pgType;
            Generator = generator;
        }
    }

    // 创建表
    static void CreateTable(NpgsqlConnection conn, string tableName, List<FieldTemplate> fields)
    {
        string sql = $"DROP TABLE IF EXISTS \"{tableName}\";";
        sql += $"\nCREATE TABLE \"{tableName}\" (\n";
        sql += string.Join(",\n", fields.Select(f => $"\"{f.Name}\" {f.PgType}"));
        sql += "\n);";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine($"已创建表 {tableName}。");
    }

    // 批量插入
    static void BulkInsert(NpgsqlConnection conn, string tableName, List<FieldTemplate> fields,
        List<Dictionary<string, object>> rows)
    {
        // 使用 COPY 批量插入
        var columns = string.Join(", ", fields.Select(f => $"\"{f.Name}\""));
        using var writer = conn.BeginTextImport($"COPY \"{tableName}\" ({columns}) FROM STDIN (FORMAT CSV)");
        foreach (var row in rows)
        {
            var line = string.Join(",", fields.Select(f => EscapeCsv(row[f.Name])));
            writer.WriteLine(line);
        }
    }

    // CSV转义
    static string EscapeCsv(object val)
    {
        if (val == null) return "";
        var s = val.ToString().Replace("\"", "\"\"");
        if (s.Contains(",") || s.Contains("\n") || s.Contains("\""))
            return $"\"{s}\"";
        return s;
    }

    // 进度显示
    static void ShowProgress(int current, int total)
    {
        double percent = (double)current / total * 100;
        Console.Write($"\r已插入：{current}/{total} ({percent:F2}%)");
    }
}