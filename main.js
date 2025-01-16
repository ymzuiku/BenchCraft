const {
  Worker,
  isMainThread,
  parentPort,
  workerData,
} = require("worker_threads");
const fs = require("fs");
const os = require("os");

// 打印内存使用
function printMemoryUsage(label) {
  const memoryUsage = process.memoryUsage();
  console.log(
    `${label} Memory Usage: RSS = ${(memoryUsage.rss / 1024 / 1024).toFixed(
      2
    )} MB, HeapUsed = ${(memoryUsage.heapUsed / 1024 / 1024).toFixed(2)} MB`
  );
}

// 执行任务逻辑
function executeTask(id) {
  // 1. 数学计算任务
  let mathResult = 0.0;
  for (let j = 1; j <= 100; j++) {
    mathResult += Math.sqrt(j) + Math.pow(j, 1.8) + Math.sin(j) * Math.cos(j);
    mathResult *= Math.exp(-j / 100.0) + Math.tan(j / 50.0);
  }

  // 2. 大规模矩阵运算 (200x200)
  const size = 200;
  const matrixA = Array.from({ length: size }, () => Array(size).fill(0));
  const matrixB = Array.from({ length: size }, () => Array(size).fill(0));
  const matrixResult = Array.from({ length: size }, () => Array(size).fill(0));
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      matrixA[x][y] = Math.sin(x + y) * Math.cos(x - y);
      matrixB[x][y] = Math.exp(-(x * y) / 500.0) + Math.tan(y + 1);
    }
  }
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      for (let k = 0; k < size; k++) {
        matrixResult[x][y] += matrixA[x][k] * matrixB[k][y];
      }
    }
  }

  // 3. 字符串拼接与处理
  let largeString = "";
  for (let j = 0; j < 500; j++) {
    largeString += `Task-${id}-Line-${j};`;
  }
  const processedString = largeString.replace(/Line/g, "ProcessedLine");

  // 4. 集合操作
  const dataSet = new Set();
  for (let j = 0; j < 500; j++) {
    dataSet.add(j);
  }
  for (let j = 0; j < 500; j++) {
    dataSet.has(j);
  }

  // 5. 排序与查找
  const array = Array.from({ length: 200 }, () =>
    Math.floor(Math.random() * 1000000)
  );
  array.sort((a, b) => a - b);
  const target = Math.floor(Math.random() * 1000000);
  array.findIndex((value) => value >= target);

  // 6. JSON 处理
  const jsonObject = {
    Id: id,
    MathResult: mathResult,
    MatrixSample: [matrixResult[0][0], matrixResult[1][1], matrixResult[2][2]],
    ProcessedString: processedString.slice(0, 100),
  };
  const jsonData = JSON.stringify(jsonObject);

  // 7. 文件操作
  const fileName = `task_ouatput_${id}.json`;
  fs.writeFileSync(fileName, jsonData);

  const fileContent = fs.readFileSync(fileName, "utf8");
  fs.unlinkSync(fileName);
  JSON.parse(fileContent);
}

// 单线程执行
function singleThreadTest(iterations, threadCount) {
  printMemoryUsage("Before Single-threaded Test");
  const startTime = Date.now();

  for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
    for (let i = 0; i < iterations; i++) {
      executeTask(i);
    }
  }

  const endTime = Date.now();
  console.log(`Single-threaded computation time: ${endTime - startTime} ms`);
  printMemoryUsage("After Single-threaded Test");
}

// 多线程执行
function multiThreadTest(iterations, threadCount) {
  printMemoryUsage("Before Multi-threaded Test");
  const startTime = Date.now();

  const workers = [];
  let completedWorkers = 0;

  return new Promise((resolve) => {
    for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
      const worker = new Worker(__filename, {
        workerData: { iterations, threadIndex },
      });

      workers.push(worker);

      worker.on("exit", () => {
        completedWorkers++;
        if (completedWorkers === threadCount) {
          const endTime = Date.now();
          console.log(
            `Multi-threaded computation time: ${endTime - startTime} ms`
          );
          printMemoryUsage("After Multi-threaded Test");
          resolve();
        }
      });
    }
  });
}

// Worker线程逻辑
if (!isMainThread) {
  const { iterations, threadIndex } = workerData;
  for (let i = 0; i < iterations; i++) {
    executeTask(threadIndex * iterations + i);
  }
  parentPort.postMessage({ status: "done" });
} else {
  // 主线程逻辑
  const iterations = 20; // 每个线程的任务数
  const threadCount = 20; // 线程数

  console.log("Running single-threaded test...");
  singleThreadTest(iterations, threadCount);

  console.log("\nRunning multi-threaded test...");
  multiThreadTest(iterations, threadCount).then(() => {
    console.log("All tasks completed.");
  });
}
