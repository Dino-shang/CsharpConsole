using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Bogus;
using ConsoleTest.DataGenerate;
using ConsoleTest.SpanBench;

namespace ConsoleTest;

class Program
{
    static void Main(string[] args)
    {
        // AssemblyPlugin.LoadNewtonSoftJsonPlugin();
        // DllImportUtil.FindTargetWindow();
        // DllImportUtil.FindContainWIndow();
        // AutoFakerToPostgres.Run();
        
        // BulkInsertBogusToPgsql.Run();
        
        // // 基础随机数据
        // Console.WriteLine($"随机字母数字: {faker.Random.AlphaNumeric(10)}");
        // Console.WriteLine($"随机布尔值: {faker.Random.Bool()}");
        // Console.WriteLine($"随机字节: {faker.Random.Byte()}");
        // Console.WriteLine($"随机字节数组: {string.Join(",", faker.Random.Bytes(5))}");
        // Console.WriteLine($"随机字符: {faker.Random.Char()}");
        // Console.WriteLine($"随机字符串: {faker.Random.String(8)}");
        // Console.WriteLine($"随机数字: {faker.Random.Number(1, 100)}");
        // Console.WriteLine($"随机小数: {faker.Random.Decimal(0, 100):F2}");
        // Console.WriteLine($"随机日期: {faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now)}");
        // Console.WriteLine();
        //
        // // 个人信息
        // Console.WriteLine("--- 个人信息 ---");
        // Console.WriteLine($"姓名: {faker.Name.FullName()}");
        // Console.WriteLine($"名: {faker.Name.FirstName()}");
        // Console.WriteLine($"姓: {faker.Name.LastName()}");
        // Console.WriteLine($"用户名: {faker.Internet.UserName()}");
        // Console.WriteLine($"邮箱: {faker.Internet.Email()}");
        // Console.WriteLine($"手机号: {faker.Phone.PhoneNumber()}");
        // Console.WriteLine($"地址: {faker.Address.FullAddress()}");
        // Console.WriteLine($"城市: {faker.Address.City()}");
        // Console.WriteLine($"国家: {faker.Address.Country()}");
        // Console.WriteLine();
        //
        // // 商务信息
        // Console.WriteLine("--- 商务信息 ---");
        // Console.WriteLine($"公司名: {faker.Company.CompanyName()}");
        // Console.WriteLine($"职位: {faker.Name.JobTitle()}");
        // Console.WriteLine($"部门: {faker.Commerce.Department()}");
        // Console.WriteLine($"产品名: {faker.Commerce.ProductName()}");
        // Console.WriteLine($"产品描述: {faker.Commerce.ProductDescription()}");
        // Console.WriteLine($"价格: {faker.Commerce.Price(10, 1000, 2)}");
        // Console.WriteLine($"SKU: {faker.Commerce.Ean13()}");
        // Console.WriteLine();
        //
        // // 网络相关
        // Console.WriteLine("--- 网络相关 ---");
        // Console.WriteLine($"URL: {faker.Internet.Url()}");
        // Console.WriteLine($"域名: {faker.Internet.DomainName()}");
        // Console.WriteLine($"IP地址: {faker.Internet.Ip()}");
        // Console.WriteLine($"MAC地址: {faker.Internet.Mac()}");
        // Console.WriteLine($"用户代理: {faker.Internet.UserAgent()}");
        // Console.WriteLine();
        //
        // // 金融相关
        // Console.WriteLine("--- 金融相关 ---");
        // Console.WriteLine($"信用卡号: {faker.Finance.CreditCardNumber()}");
        // Console.WriteLine($"BIC Code: {faker.Finance.Bic()}");
        // Console.WriteLine($"银行账号: {faker.Finance.Account()}");
        // Console.WriteLine($"货币代码: {faker.Finance.Currency().Code}");
        // Console.WriteLine($"交易金额: {faker.Finance.Amount(100, 10000, 2)}");
        // Console.WriteLine();
        //
        // // 系统信息
        // Console.WriteLine("--- 系统信息 ---");
        // Console.WriteLine($"语义化 版本号: {faker.System.Semver()}");
        // Console.WriteLine($"苹果Push token: {faker.System.ApplePushToken()}");
        // Console.WriteLine($"版本: {faker.System.Version()}");
        // Console.WriteLine($"文件名: {faker.System.FileName()}");
        // Console.WriteLine($"文件扩展名: {faker.System.FileExt()}");
        // Console.WriteLine();
        //
        // // 其他实用功能
        // Console.WriteLine("--- 其他功能 ---");
        // Console.WriteLine($"随机颜色: {faker.Internet.Color()}");
        // Console.WriteLine($"随机图片URL: {faker.Image.PicsumUrl(200, 200)}");
        // Console.WriteLine($"随机UUID: {faker.Random.Guid()}");
        // Console.WriteLine($"随机哈希: {faker.Random.Hash()}");
        // Console.WriteLine($"随机单词: {faker.Random.Words(3)}");
        // Console.WriteLine($"随机句子: {faker.Random.Words(10)}");
        // Console.WriteLine();
        //
        // // 数组和集合
        // Console.WriteLine("--- 数组和集合 ---");
        // var randomNumbers = faker.Random.Digits(5);
        // Console.WriteLine($"随机数字数组: {string.Join(",", randomNumbers)}");
        //
        // var randomStrings = faker.Random.WordsArray(3, 5);
        // Console.WriteLine($"随机字符串数组: {string.Join(",", randomStrings)}");
        //
        // // 原始代码保留
        // Console.WriteLine("--- 原始代码示例 ---");
        // Console.WriteLine(faker.Commerce.Locale);
        // Console.WriteLine(faker.Commerce.Random.AlphaNumeric(10));
        // Console.WriteLine(faker.Commerce.Random.Bool());
        // Console.WriteLine(faker.Commerce.Random.Byte());
        // Console.WriteLine(faker.Commerce.Random.Bytes(10));
        // Console.WriteLine(faker.Commerce.Random.Char());
        // Console.WriteLine(faker.Commerce.Random.Chars());
        // Console.WriteLine(faker.Random.AlphaNumeric(64)); // 可以用于生成密钥   
        // Console.WriteLine(Faker.DefaultStrictMode);
        
        

    }
}