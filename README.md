# Performance Test for Go, Bun.js, Node.js, Rust, Python and C#

This performance test evaluates the execution time and resource utilization of computationally intensive tasks across five programming environments: Go, Bun.js, Node.js, Rust, Python and C#. It examines both single-threaded and multi-threaded (or simulated concurrency) scenarios to compare the efficiency, runtime behavior, and memory usage of each language.

## Test Description

The test includes tasks designed to stress the CPU, memory, and concurrency models of each environment. Key benchmarks include:

1. **Complex Mathematical Calculations:**

   - Operations such as square roots, trigonometric functions, logarithms, and exponential calculations are executed repeatedly in nested loops.

2. **Matrix Multiplication:**

   - Simulates a 100x100 matrix multiplication operation, a common benchmark for computational performance.

3. **Advanced Mathematical Expressions:**

   - Combines multiple mathematical operations into a single complex expression to test floating-point precision and processing speed.

4. **JSON Serialization and Deserialization:**

   - Tests JSON handling by creating a structured object, serializing it to a JSON string, and then deserializing it back to an object.

5. **File Operations:**
   - Tests file operations by writing to a file, reading from it, and deleting it.

Each test is executed in both single-threaded and multi-threaded (or event-driven) scenarios to compare the efficiency of different concurrency models.

### Execution Modes

#### Single-threaded Test

Executes tasks sequentially within a single thread for 150 iterations.

#### Multi-threaded Test

Simulates 30 tasks across 50 threads to evaluate the efficiency of parallel execution:

- Go and C#: Use native multithreading (Goroutines in Go, Task.Run in C#).
- Rust: Utilizes the Tokio runtime for concurrency.
- Node.js and Bun.js: Employ worker threads to simulate multithreading.

## Performance Results

### Environment

Operating System: Windows 11
Hardware: AMD Ryzen 9 9950X CPU, 96GB RAM

### Commands and Outputs

| Language          | Version                | Single-threaded/Memory | Multi-threaded/Memory |
| ----------------- | ---------------------- | ---------------------- | --------------------- |
| Rust              | rustc 1.84.0 + tokio   | 7505 ms, 35 MB         | 542 ms, 35 MB         |
| C#                | 9.0.101                | 7396 ms, 34 MB         | 597 ms, 137 MB        |
| Go                | go1.23.4 windows/amd64 | 7301 ms, 11.89 MB      | 649 ms, 56 MB         |
| Node.js           | v23.5.0                | 8885 ms, 73 MB         | 905 ms, 82 MB         |
| Bun.js            | 1.1.43                 | 7850 ms, 189 MB        | 1398 ms, 914 MB       |
| HTML + Javascript | chrome, v8, web-worker | 8909 ms, 9.54 MB       | 816 ms, 9.54MB        |
| Python            | Python 3.12.8          | 95084 ms, 22 MB        | 95267 ms, 54 MB       |

HTML + Javascript Live Demo: https://bench-craft.vercel.app/

### Observations

1. **Minimal Differences in Single-Threaded Performance**

   - C#, Go, and Rust have single-threaded execution times within 200 milliseconds of each other, which is negligible. Even Node.js and Bun.js, while slightly slower, differ by less than 1000 milliseconds.
   - For non-real-time applications, these differences are unlikely to impact user experience significantly.

2. **Converging Multi-Threaded Performance**

   - The multi-threaded performance of C#, Go, and Rust ranges between 500-650 milliseconds, showing only minimal variation.
   - While Node.js and Bun.js are slightly less efficient in multi-threaded tasks, their performance is still adequate for most scenarios, particularly if the tasks are not computation-heavy.

3. **Memory Usage Differences Are Insignificant**

   - Apart from Bun.js, which shows a spike in memory usage in multi-threaded mode, the other languages exhibit reasonable memory consumption. On modern hardware, such differences are unlikely to be a bottleneck.

### Why Are They So Similar?

- **Modern Hardware**: The test environment (Ryzen 9 9950X + 96GB RAM) provides ample computational power, masking the performance differences between these languages.
- **Complexity of Real-World Needs**: Real-world projects depend not only on computational performance but also on ecosystem, development speed, maintainability, and community support.
- **Task Nature**: While these tests cover common scenarios, they may not fully represent the demands of specific domains like real-time systems or embedded development.

### The Outlier: Python

Python’s performance and multi-threading capabilities clearly lag behind the other languages. This is due to its design philosophy prioritizing readability and development efficiency over high performance or concurrency. While libraries like NumPy, Cython, or multiprocessing can optimize specific tasks, Python’s overall performance in such benchmarks remains a limitation.

## How to Run the Tests

To execute the tests for each language, run the following commands:

1. **Go:**

   ```bash
   go build -o main.exe main.go
   ./main.exe
   ```

2. **Bun.js:**

   ```bash
   bun run main.js
   ```

3. **Node.js:**

   ```bash
   node main.js
   ```

4. **C#:**

   ```bash
   dotnet build -c Release Bench/Bench.csproj
   dotnet Bench\bin\Release\net9.0\Bench.dll
   ```

5. **Rust:**

   ```bash
   cd rust_benchmark && cargo build --release
   ./rust_benchmark/target/release/rust_benchmark
   ```

6. **Python:**
   ```bash
   python main.py
   ```

---

This version integrates the updated information from the table and refines the structure for readability and clarity. Let me know if you need further adjustments!
