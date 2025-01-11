import math
import random
import string
import json
import time
import os
from concurrent.futures import ThreadPoolExecutor, as_completed


# 打印内存使用
def print_memory_usage(label):
    import psutil
    process = psutil.Process()
    memory_info = process.memory_info()
    print(f"{label} Memory Usage: RSS = {memory_info.rss / 1024 / 1024:.2f} MB")


# 执行任务逻辑
def execute_task(task_id):
    # 1. 数学计算任务
    math_result = 0.0
    for j in range(1, 101):
        math_result += math.sqrt(j) + math.pow(j, 1.8) + math.sin(j) * math.cos(j)
        math_result *= math.exp(-j / 100.0) + math.tan(j / 50.0)

    # 2. 大规模矩阵运算 (200x200)
    size = 200
    matrix_a = [[math.sin(x + y) * math.cos(x - y) for y in range(size)] for x in range(size)]
    matrix_b = [[math.exp(-(x * y) / 500.0) + math.tan(y + 1) for y in range(size)] for x in range(size)]
    matrix_result = [[0.0 for _ in range(size)] for _ in range(size)]

    for x in range(size):
        for y in range(size):
            matrix_result[x][y] = sum(matrix_a[x][k] * matrix_b[k][y] for k in range(size))

    # 3. 字符串拼接与处理
    large_string = "".join(f"Task-{task_id}-Line-{j};" for j in range(500))
    processed_string = large_string.replace("Line", "ProcessedLine")

    # 4. 集合操作
    data_set = set(range(500))
    _ = [j in data_set for j in range(500)]

    # 5. 排序与查找
    array = [random.randint(0, 1_000_000) for _ in range(200)]
    array.sort()
    target = random.randint(0, 1_000_000)
    _ = next((idx for idx, val in enumerate(array) if val >= target), -1)

    # 6. JSON 处理
    json_object = {
        "Id": task_id,
        "MathResult": math_result,
        "MatrixSample": [matrix_result[0][0], matrix_result[1][1], matrix_result[2][2]],
        "ProcessedString": processed_string[:100],
    }
    json_data = json.dumps(json_object)

    # 7. 文件操作
    file_name = f"task_output_{task_id}.json"
    with open(file_name, "w") as f:
        f.write(json_data)
    with open(file_name, "r") as f:
        file_content = f.read()
    os.remove(file_name)

    parsed_object = json.loads(file_content)
    return parsed_object


# 单线程测试
def single_thread_test(iterations, thread_count):
    print_memory_usage("Before Single-threaded Test")
    start_time = time.time()

    for thread_index in range(thread_count):
        for i in range(iterations):
            execute_task(thread_index * iterations + i)

    end_time = time.time()
    print(f"Single-threaded computation time: {(end_time - start_time) * 1000:.2f} ms")
    print_memory_usage("After Single-threaded Test")


# 多线程测试
def multi_thread_test(iterations, thread_count):
    print_memory_usage("Before Multi-threaded Test")
    start_time = time.time()

    with ThreadPoolExecutor(max_workers=thread_count) as executor:
        futures = [
            executor.submit(execute_task, thread_index * iterations + i)
            for thread_index in range(thread_count)
            for i in range(iterations)
        ]
        for future in as_completed(futures):
            _ = future.result()

    end_time = time.time()
    print(f"Multi-threaded computation time: {(end_time - start_time) * 1000:.2f} ms")
    print_memory_usage("After Multi-threaded Test")


if __name__ == "__main__":
    import psutil  # 确保 psutil 已安装

    ITERATIONS = 20  # 每个线程的任务数
    THREAD_COUNT = 10  # 线程数

    print("Running single-threaded test...")
    single_thread_test(ITERATIONS, THREAD_COUNT)

    print("\nRunning multi-threaded test...")
    multi_thread_test(ITERATIONS, THREAD_COUNT)
