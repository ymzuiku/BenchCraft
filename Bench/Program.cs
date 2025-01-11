using System.Diagnostics;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        const int iterations = 20; // 每个线程的任务数
        const int threadCount = 20; // 线程数

        Console.WriteLine("Running single-threaded test...");
        SingleThreadTest(iterations, threadCount);

        Console.WriteLine("\nRunning multi-threaded test...");
        MultiThreadTest(iterations, threadCount).Wait();
    }

    // 打印内存使用
    static void PrintMemoryUsage(string label)
    {
        var process = Process.GetCurrentProcess();
        Console.WriteLine($"{label} Memory Usage: WorkingSet = {process.WorkingSet64 / 1024.0 / 1024.0:F2} MB, PrivateMemory = {process.PrivateMemorySize64 / 1024.0 / 1024.0:F2} MB");
    }

    // 单线程执行
    static void SingleThreadTest(int iterations, int threadCount)
    {
        PrintMemoryUsage("Before Single-threaded Test");
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
        PrintMemoryUsage("After Single-threaded Test");
    }

    // 多线程执行
    static async Task MultiThreadTest(int iterations, int threadCount)
    {
        PrintMemoryUsage("Before Multi-threaded Test");
        var stopwatch = Stopwatch.StartNew();

        var tasks = new List<Task>();
        for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
        {
            int localThreadIndex = threadIndex;
            tasks.Add(Task.Run(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    ExecuteTask(localThreadIndex * iterations + i);
                }
            }));
        }

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        Console.WriteLine($"Multi-threaded computation time: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
        PrintMemoryUsage("After Multi-threaded Test");
    }

    // 执行任务逻辑
    static void ExecuteTask(int id)
    {
        // 1. 数学计算任务
        double mathResult = 0.0;
        for (int j = 1; j <= 100; j++)
        {
            mathResult += Math.Sqrt(j) + Math.Pow(j, 1.8) + Math.Sin(j) * Math.Cos(j);
            mathResult *= Math.Exp(-j / 100.0) + Math.Tan(j / 50.0);
        }

        // 2. 大规模矩阵运算 (200x200)
        int size = 200;
        double[,] matrixA = new double[size, size];
        double[,] matrixB = new double[size, size];
        double[,] matrixResult = new double[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                matrixA[x, y] = Math.Sin(x + y) * Math.Cos(x - y);
                matrixB[x, y] = Math.Exp(-(x * y) / 500.0) + Math.Tan(y + 1);
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

        // 3. 字符串拼接与处理
        StringBuilder sb = new StringBuilder();
        for (int j = 0; j < 500; j++)
        {
            sb.Append($"Task-{id}-Line-{j};");
        }
        string largeString = sb.ToString();
        string processedString = largeString.Replace("Line", "ProcessedLine");

        // 4. 集合操作
        HashSet<int> dataSet = new HashSet<int>();
        for (int j = 0; j < 500; j++)
        {
            dataSet.Add(j);
        }
        for (int j = 0; j < 500; j++)
        {
            _ = dataSet.Contains(j);
        }

        // 5. 排序与查找
        Random random = new Random();
        List<int> array = Enumerable.Range(0, 200).Select(_ => random.Next(0, 1_000_000)).ToList();
        array.Sort();
        int target = random.Next(0, 1_000_000);
        int index = array.BinarySearch(target);

        // 6. JSON 处理
        var jsonObject = new
        {
            Id = id,
            MathResult = mathResult,
            MatrixSample = new[] { matrixResult[0, 0], matrixResult[1, 1], matrixResult[2, 2] },
            ProcessedString = processedString.Substring(0, Math.Min(processedString.Length, 100))
        };
        string jsonData = JsonSerializer.Serialize(jsonObject);

        // 7. 文件操作
        string fileName = $"task_output_{id}.json";
        File.WriteAllText(fileName, jsonData);

        string fileContent = File.ReadAllText(fileName);
        File.Delete(fileName);

        var parsedObject = JsonSerializer.Deserialize<object>(fileContent);
    }
}
