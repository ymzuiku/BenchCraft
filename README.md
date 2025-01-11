# Performance Test for Go, Bun, Node.js, and C#

This performance test compares the execution time of computationally intensive tasks across four programming environments: Go, Bun.js, Node.js, and C#. The test includes both single-threaded and multi-threaded (or simulated concurrency) scenarios to observe the efficiency and runtime behavior of each language.

## Test Description

This test evaluates the performance of each language by running computationally intensive tasks involving:

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

### Single-threaded Test

150 times of tasks

- Executes tasks sequentially within a single thread for a fixed number of iterations and threads.

### Multi-threaded Test

Task count: 30, Thread count: 50

- For Go and C#, tasks are executed in parallel using native multithreading mechanisms (e.g., Goroutines in Go, `Task.Run` in C#).
- For Bun.js and Node.js, concurrency is simulated using asynchronous programming (event loop). Node.js is not natively multithreaded but uses an event-driven, non-blocking I/O model.

## Commands and Outputs

Windows 10, CPU: AMD Ryzen 9 9950X, RAM: 96GB

| Language | Version                       | Single-threaded Output | Multi-threaded/Event-driven Output |
| -------- | ----------------------------- | ---------------------- | ---------------------------------- |
| Go       | go1.23.4 windows/amd64        | 4138 ms                | 411 ms                             |
| C#       | 9.0.101                       | 4326 ms                | 345 ms                             |
| Bun.js   | 1.1.43                        | 5135 ms                | 5038 ms                            |
| Node.js  | v23.5.0                       | 5982 ms                | 5995 ms                            |
| Rust     | 1.84.0 (9fc6b4312 2025-01-07) | 6487 ms                | 500 ms                             |

## Important Notes on Node.js

- Node.js is **not natively multithreaded**. Instead, it uses an **event loop** and asynchronous I/O to handle concurrent tasks efficiently. While this model allows high-performance non-blocking operations, it is fundamentally different from true multithreading as implemented in Go or C#.
- For computationally intensive tasks, the Node.js event loop may appear slower compared to true multithreading due to the lack of parallel CPU execution.

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

## Results

After running the tests, fill in the execution times in the table above to compare the performance across languages.
