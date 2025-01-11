using System.Diagnostics;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        const int iterations = 100; // 每个线程的任务数
        const int threadCount = 8;  // 线程数

        Console.WriteLine("Running single-threaded test...");
        SingleThreadTest(iterations, threadCount);

        Console.WriteLine("\nRunning multi-threaded test...");
        MultiThreadTest(iterations, threadCount);
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

        // 2. 模拟矩阵计算 (100x100 矩阵乘法)
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

        // 3. 高级数学函数组合 (复杂表达式)
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

        // 序列化为字符串
        string jsonString = JsonSerializer.Serialize(jsonObject);

        // 反序列化回对象
        var deserializedObject = JsonSerializer.Deserialize<object>(jsonString);
    }
}
