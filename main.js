const fs = require("fs");
const crypto = require("crypto");

// 执行任务逻辑
function executeTask(i) {
  // 1. 复杂循环中的非线性运算
  let result = 0.0;
  for (let j = 1; j <= 100; j++) {
    result +=
      Math.sqrt(j) +
      Math.sin(j) * Math.cos(j) +
      Math.log(j + 1) / Math.tan(j + 0.1);
    result *= Math.exp(-j % 5) + Math.atan(j / 10.0);
  }

  // 2. 模拟矩阵计算 (100x100 矩阵乘法)
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

  // 3. 高级数学函数组合 (复杂表达式)
  let specialResult = 0.0;
  for (let j = 1; j <= 50; j++) {
    specialResult +=
      Math.sqrt(j) * Math.sin(j) * Math.log10(j + 1) +
      Math.exp(-j) * Math.cos(Math.tan(j));
  }

  // 4. JSON 处理
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

  // 反序列化回对象
  JSON.parse(jsonString);

  // 5. 文件操作测试
  const fileName = `temp_file_${i}.json`;

  // 写入文件
  fs.writeFileSync(fileName, jsonString, "utf-8");

  // 读取文件
  const fileContent = fs.readFileSync(fileName, "utf-8");

  // 删除文件
  fs.unlinkSync(fileName);

  // 解析文件内容，模拟处理
  const fileData = JSON.parse(fileContent);
}

// 单线程执行
async function singleThreadTest(iterations, threadCount) {
  const start = performance.now();

  for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
    for (let i = 0; i < iterations; i++) {
      executeTask(i);
    }
  }

  const end = performance.now();
  console.log(`Single-threaded computation time: ${end - start}ms`);
}

// 多线程执行
async function eventDrivenTest(iterations, threadCount) {
  const start = performance.now();

  const promises = [];
  for (let threadIndex = 0; threadIndex < threadCount; threadIndex++) {
    promises.push(
      new Promise((resolve) => {
        for (let i = 0; i < iterations; i++) {
          executeTask(i);
        }
        resolve();
      })
    );
  }

  await Promise.all(promises);
  const end = performance.now();
  console.log(`Event-driven computation time: ${end - start}ms`);
}

(async () => {
  const iterations = 30; // 每个线程的任务数
  const threadCount = 50; // 线程数

  console.log("Running single-threaded test...");
  await singleThreadTest(iterations, threadCount);

  console.log("\nRunning event-driven test...");
  await eventDrivenTest(iterations, threadCount);
})();
