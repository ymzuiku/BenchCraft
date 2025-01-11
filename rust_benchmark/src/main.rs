use rand::Rng;
use std::fs::{self, File};
use std::io::{Read, Write};
use std::sync::Arc;
use std::time::Instant;
use tokio::sync::Semaphore;
use tokio::task;

use sysinfo::{System, SystemExt};

// 打印内存使用
fn print_memory_usage(label: &str) {
    let mut sys = System::new_all();
    sys.refresh_memory();

    let total_memory = sys.total_memory() as f64 / 1024.0 / 1024.0 / 1024.0;
    let used_memory = sys.used_memory() as f64 / 1024.0 / 1024.0 / 1024.0;

    println!(
        "{} Memory Usage: Total = {:.2} MB, Used = {:.2} MB",
        label, total_memory, used_memory
    );
}

// 执行任务逻辑
fn execute_task(id: usize) {
    // 1. 数学计算任务
    let mut math_result = 0.0;
    for j in 1..=100 {
        math_result +=
            (j as f64).sqrt() + (j as f64).powf(1.8) + (j as f64).sin() * (j as f64).cos();
        math_result *= (-j as f64 / 100.0).exp() + (j as f64 / 50.0).tan();
    }

    // 2. 大规模矩阵运算 (200x200)
    let size = 200;
    let mut matrix_a = vec![vec![0.0; size]; size];
    let mut matrix_b = vec![vec![0.0; size]; size];
    let mut matrix_result = vec![vec![0.0; size]; size];
    for x in 0..size {
        for y in 0..size {
            matrix_a[x][y] = (x as f64 + y as f64).sin() * (x as f64 - y as f64).cos();
            matrix_b[x][y] = (-(x as f64 * y as f64) / 500.0).exp() + (y as f64 + 1.0).tan();
        }
    }
    for x in 0..size {
        for y in 0..size {
            for k in 0..size {
                matrix_result[x][y] += matrix_a[x][k] * matrix_b[k][y];
            }
        }
    }

    // 3. 字符串拼接与处理
    let mut large_string = String::new();
    for j in 0..500 {
        large_string.push_str(&format!("Task-{id}-Line-{j};"));
    }
    let processed_string = large_string.replace("Line", "ProcessedLine");

    // 4. 集合操作
    let mut data_set = std::collections::HashSet::new();
    for j in 0..500 {
        data_set.insert(j);
    }
    for j in 0..500 {
        data_set.contains(&j);
    }

    // 5. 排序与查找
    let mut array: Vec<i32> = (0..200)
        .map(|_| rand::thread_rng().gen_range(0..1_000_000))
        .collect();
    array.sort_unstable();
    let target = rand::thread_rng().gen_range(0..1_000_000);
    let _ = array.binary_search(&target);

    // 6. JSON 处理
    let json_object = serde_json::json!({
        "Id": id,
        "MathResult": math_result,
        "MatrixSample": [
            matrix_result[0][0],
            matrix_result[1][1],
            matrix_result[2][2]
        ],
        "ProcessedString": &processed_string[0..100]
    });
    let json_data = serde_json::to_string(&json_object).unwrap();

    // 7. 文件操作
    let file_name = format!("task_output_{id}.json");
    let mut file = File::create(&file_name).unwrap();
    file.write_all(json_data.as_bytes()).unwrap();

    let mut file_content = String::new();
    File::open(&file_name)
        .unwrap()
        .read_to_string(&mut file_content)
        .unwrap();
    fs::remove_file(&file_name).unwrap();

    let _: serde_json::Value = serde_json::from_str(&file_content).unwrap();
}

// 单线程测试
fn single_thread_test(iterations: usize, thread_count: usize) {
    print_memory_usage("Before Single-threaded Test");
    let start = Instant::now();

    for _ in 0..thread_count {
        for i in 0..iterations {
            execute_task(i);
        }
    }

    let duration = start.elapsed();
    println!(
        "Single-threaded computation time: {:.2} ms",
        duration.as_secs_f64() * 1000.0
    );
    print_memory_usage("After Single-threaded Test");
}

// 多线程测试
async fn multi_thread_test(iterations: usize, thread_count: usize) {
    print_memory_usage("Before Multi-threaded Test");
    let start = Instant::now();
    let semaphore = Arc::new(Semaphore::new(thread_count));

    let mut tasks = Vec::new();
    for i in 0..thread_count {
        let permit = semaphore.clone().acquire_owned().await.unwrap();
        let task = task::spawn(async move {
            for j in 0..iterations {
                execute_task(i * iterations + j);
            }
            drop(permit); // 释放信号量
        });
        tasks.push(task);
    }

    for task in tasks {
        task.await.unwrap();
    }

    let duration = start.elapsed();
    println!(
        "Multi-threaded computation time: {:.2} ms",
        duration.as_secs_f64() * 1000.0
    );
    print_memory_usage("After Multi-threaded Test");
}

#[tokio::main]
async fn main() {
    const ITERATIONS: usize = 20; // 每个线程的任务数
    const THREAD_COUNT: usize = 20; // 线程数

    println!("Running single-threaded test...");
    single_thread_test(ITERATIONS, THREAD_COUNT);

    println!("\nRunning multi-threaded test...");
    multi_thread_test(ITERATIONS, THREAD_COUNT).await;
}
