# Performance Test for Go, Bun.js, Node.js, Rust, and C#

This performance test evaluates the execution time and resource utilization of computationally intensive tasks across five programming environments: Go, Bun.js, Node.js, Rust, and C#. It examines both single-threaded and multi-threaded (or simulated concurrency) scenarios to compare the efficiency, runtime behavior, and memory usage of each language.

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

| Language | Version                | Single-threaded/Memory | Multi-threaded/Memory | GC Count |
| -------- | ---------------------- | ---------------------- | --------------------- | -------- |
| C#       | 9.0.101                | 4326 ms, 50 MB         | 345 ms, 60 MB         | 46       |
| Go       | go1.23.4 windows/amd64 | 4138 ms, 11.89 MB      | 429 ms, 45 MB         | 150      |
| Rust     | rustc 1.84.0 + tokio   | 5848 ms, 33.86 MB      | 444 ms, 33.88 MB      | 0        |
| Node.js  | v23.5.0                | 6030 ms, 73 MB         | 866 ms, 104 MB        | ?        |
| Bun.js   | 1.1.43                 | 5135 ms, 37 MB         | 1644 ms, 37 MB        | ?        |

### Observations

- C#: Exhibited exceptional multi-threaded performance with the lowest execution time in multi-threaded tasks.
- Go: Delivered consistent performance with low memory usage and efficient garbage collection.
- Rust: Despite its reputation for high performance, Rust's results in release mode were not as competitive as expected in this benchmark. This could be due to the nature of the test tasks, the overhead of the Tokio runtime, or differences in optimization for these specific workloads.
- Node.js: As an event-driven, single-threaded runtime, Node.js lags in computational tasks due to limited parallel execution.
- Bun.js: Demonstrated improved single-threaded performance over Node.js, but multi-threading performance is less optimized compared to Go or C#.

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

6. **All at Once (using Make):**
   ```bash
   make all
   ```

---

This version integrates the updated information from the table and refines the structure for readability and clarity. Let me know if you need further adjustments!
