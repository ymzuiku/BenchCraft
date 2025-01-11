using System.Diagnostics;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        const int iterations = 30; // 每个线程的任务数
        const int threadCount = 50; // 线程数

        Console.WriteLine("Running single-threaded test...");
        PrintMemoryAndGC("Before Single-threaded Test");
        SingleThreadTest(iterations, threadCount);
        PrintMemoryAndGC("After Single-threaded Test");

        Console.WriteLine("\nRunning multi-threaded test...");
        PrintMemoryAndGC("Before Multi-threaded Test");
        MultiThreadTest(iterations, threadCount);
        PrintMemoryAndGC("After Multi-threaded Test");
    }

    // 打印内存使用和垃圾回收次数
    static void PrintMemoryAndGC(string label)
    {
        Process currentProcess = Process.GetCurrentProcess();
        long memorySize = currentProcess.WorkingSet64; // 获取进程的物理内存使用

        Console.WriteLine($"{label} Memory Usage: {memorySize / 1024.0 / 1024.0:F2} MB");
        Console.WriteLine($"{label} GC Counts: Gen0 = {GC.CollectionCount(0)}, Gen1 = {GC.CollectionCount(1)}, Gen2 = {GC.CollectionCount(2)}");
    }

    // 单线程执行
    static void SingleThreadTest(int iterations, int threadCount)
    {
        var stopwatch = Stopwatch.StartNew();

        for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
        {
            for (int i = 0; i < iterations; i++)
            {
                ExecuteTask(i);
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Single-threaded computation time: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
    }

    // 多线程执行
    static void MultiThreadTest(int iterations, int threadCount)
    {
        var stopwatch = Stopwatch.StartNew();

        var tasks = new Task[threadCount];
        for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
        {
            tasks[threadIndex] = Task.Run(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    ExecuteTask(i);
                }
            });
        }

        Task.WaitAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Multi-threaded computation time: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
    }

    // 执行任务逻辑
    static void ExecuteTask(int i)
    {
        // 1. 复杂循环中的非线性运算
        double result = 0.0;
        for (int j = 1; j <= 100; j++)
        {
            result += Math.Sqrt(j) + Math.Sin(j) * Math.Cos(j) + Math.Log(j + 1) / Math.Tan(j + 0.1);
            result *= Math.Exp(-j % 5) + Math.Atan(j / 10.0);
        }

        // 2. 模拟矩阵计算
        int size = 100;
        double[,] matrixA = new double[size, size];
        double[,] matrixB = new double[size, size];
        double[,] matrixResult = new double[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                matrixA[x, y] = Math.Sin(x + y);
                matrixB[x, y] = Math.Cos(x - y);
            }
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int k = 0; k < size; k++)
                {
                    matrixResult[x, y] += matrixA[x, k] * matrixB[k, y];
                }
            }
        }

        // 3. 高级数学函数组合
        double specialResult = 0.0;
        for (int j = 1; j <= 50; j++)
        {
            specialResult += Math.Sqrt(j) * Math.Sin(j) * Math.Log10(j + 1) + Math.Exp(-j) * Math.Cos(Math.Tan(j));
        }

        // 4. JSON 处理
        var jsonObject = new
        {
            Id = i,
            Name = $"Test-{i}",
            Data = new
            {
                MatrixSum = specialResult,
                Result = result,
                Details = new[]
                {
                new { Index = 1, Value = matrixResult[0, 0] },
                new { Index = 2, Value = matrixResult[1, 1] }
            }
            },
            AdditionalData = new object[30]
        };

        for (int k = 0; k < 30; k++)
        {
            jsonObject.AdditionalData[k] = new { Index = k, Value = k * 2 };
        }

        string jsonString = JsonSerializer.Serialize(jsonObject);

        // 5. 文件操作
        string fileName = $"temp_file_{i}_{Task.CurrentId}.json";

        // 写入文件
        File.WriteAllText(fileName, jsonString);

        // 读取文件
        string fileContent = File.ReadAllText(fileName);

        // 删除文件
        File.Delete(fileName);

        // 解析文件内容
        var fileData = JsonSerializer.Deserialize<object>(fileContent);
    }
}
