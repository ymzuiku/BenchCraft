const {
  Worker,
  isMainThread,
  parentPort,
  workerData,
} = require("worker_threads");
const fs = require("fs");

// 执行任务逻辑
function executeTask(i) {
  let result = 0.0;
  for (let j = 1; j <= 100; j++) {
    result +=
      Math.sqrt(j) +
      Math.sin(j) * Math.cos(j) +
      Math.log(j + 1) / Math.tan(j + 0.1);
    result *= Math.exp(-j % 5) + Math.atan(j / 10.0);
  }

  const size = 100;
  const matrixA = Array.from({ length: size }, (_, x) =>
    Array.from({ length: size }, (_, y) => Math.sin(x + y))
  );
  const matrixB = Array.from({ length: size }, (_, x) =>
    Array.from({ length: size }, (_, y) => Math.cos(x - y))
  );
  const matrixResult = Array.from({ length: size }, () => Array(size).fill(0));

  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      for (let k = 0; k < size; k++) {
        matrixResult[x][y] += matrixA[x][k] * matrixB[k][y];
      }
    }
  }

  let specialResult = 0.0;
  for (let j = 1; j <= 50; j++) {
    specialResult +=
      Math.sqrt(j) * Math.sin(j) * Math.log10(j + 1) +
      Math.exp(-j) * Math.cos(Math.tan(j));
  }

  const jsonObject = {
    Id: i,
    Name: `Test-${i}`,
    Data: {
      MatrixSum: specialResult,
      Result: result,
      Details: [
        { Index: 1, Value: matrixResult[0][0] },
        { Index: 2, Value: matrixResult[1][1] },
      ],
    },
    AdditionalData: [],
  };

  for (let k = 0; k < 30; k++) {
    jsonObject.AdditionalData.push({
      Index: k,
      Value: k * 2,
    });
  }

  const jsonString = JSON.stringify(jsonObject);
  const fileName = `temp_file_${i}.json`;

  fs.writeFileSync(fileName, jsonString, "utf-8");
  const fileContent = fs.readFileSync(fileName, "utf-8");
  fs.unlinkSync(fileName);

  JSON.parse(fileContent);
}

// 打印内存消耗
function printMemoryUsage(prefix) {
  const used = process.memoryUsage();
  console.log(
    `${prefix} Memory Usage:`,
    `RSS: ${(used.rss / 1024 / 1024).toFixed(2)} MB,`,
    `Heap Total: ${(used.heapTotal / 1024 / 1024).toFixed(2)} MB,`,
    `Heap Used: ${(used.heapUsed / 1024 / 1024).toFixed(2)} MB`
  );
}

// Worker 线程任务
function workerTask({ iterations, startIndex }) {
  for (let i = 0; i < iterations; i++) {
    executeTask(startIndex + i);
  }
  parentPort.postMessage("done");
}

// 多线程执行
async function multiThreadTest(iterations, threadCount) {
  const start = performance.now();
  const workers = [];

  for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
    const workerData = { iterations, startIndex: threadIndex * iterations };

    workers.push(
      new Promise((resolve, reject) => {
        const worker = new Worker(__filename, { workerData });

        worker.on("message", (msg) => {
          if (msg === "done") resolve();
        });
        worker.on("error", reject);
        worker.on("exit", (code) => {
          if (code !== 0) reject(new Error(`Worker exited with code ${code}`));
        });
      })
    );
  }

  await Promise.all(workers);

  const end = performance.now();
  printMemoryUsage("After Multi-threaded Test");
  console.log(`Multi-threaded computation time: ${end - start}ms`);
}

// 主线程逻辑
if (isMainThread) {
  (async () => {
    const iterations = 30; // 每个线程的任务数
    const threadCount = 50; // 线程数

    console.log("Running single-threaded test...");
    printMemoryUsage("Before Single-threaded Test");
    const startSingle = performance.now();
    for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
      for (let i = 0; i < iterations; i++) {
        executeTask(i);
      }
    }
    const endSingle = performance.now();
    printMemoryUsage("After Single-threaded Test");
    console.log(
      `Single-threaded computation time: ${endSingle - startSingle}ms`
    );

    console.log("\nRunning multi-threaded test...");
    printMemoryUsage("Before Multi-threaded Test");
    await multiThreadTest(iterations, threadCount);
  })();
} else {
  workerTask(workerData);
}
