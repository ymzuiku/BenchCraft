use std::fs;
use std::f64;
use std::thread;
use std::time::Instant;

// 执行任务逻辑
fn execute_task(i: usize) {
    // 1. 复杂循环中的非线性运算
    let mut result = 0.0;
    for j in 1..=100 {
        result += (j as f64).sqrt()
            + (j as f64).sin() * (j as f64).cos()
            + ((j as f64) + 1.0).ln() / ((j as f64) + 0.1).tan();
        result *= (-((j % 5) as f64)).exp() + ((j as f64) / 10.0).atan();
    }

    // 2. 模拟矩阵计算 (100x100 矩阵乘法)
    let size = 100;
    let mut matrix_a = vec![vec![0.0; size]; size];
    let mut matrix_b = vec![vec![0.0; size]; size];
    let mut matrix_result = vec![vec![0.0; size]; size];

    for x in 0..size {
        for y in 0..size {
            matrix_a[x][y] = (x + y) as f64;
            matrix_b[x][y] = (x - y) as f64;
        }
    }

    for x in 0..size {
        for y in 0..size {
            for k in 0..size {
                matrix_result[x][y] += matrix_a[x][k] * matrix_b[k][y];
            }
        }
    }

    // 3. 高级数学函数组合 (复杂表达式)
    let mut special_result = 0.0;
    for j in 1..=50 {
        special_result += (j as f64).sqrt() * (j as f64).sin() * ((j as f64) + 1.0).log10()
            + (-j as f64).exp() * ((j as f64).tan()).cos();
    }

    // 4. JSON 处理
    let json_object = serde_json::json!({
        "Id": i,
        "Name": format!("Test-{}", i),
        "Data": {
            "MatrixSum": special_result,
            "Result": result,
            "Details": [
                { "Index": 1, "Value": matrix_result[0][0] },
                { "Index": 2, "Value": matrix_result[1][1] }
            ]
        },
        "AdditionalData": (0..30).map(|k| {
            serde_json::json!({
                "Index": k,
                "Value": k * 2
            })
        }).collect::<Vec<_>>()
    });

    // 序列化为字符串
    let json_string = serde_json::to_string(&json_object).unwrap();

    // 文件操作
    let file_name = format!("temp_file_{}.json", i);
    fs::write(&file_name, &json_string).unwrap();
    let file_content = fs::read_to_string(&file_name).unwrap();
    fs::remove_file(&file_name).unwrap();

    // 反序列化回对象
    let _: serde_json::Value = serde_json::from_str(&file_content).unwrap();
}

// 单线程执行
fn single_thread_test(iterations: usize, thread_count: usize) {
    let start = Instant::now();

    for thread_index in 0..thread_count {
        for i in 0..iterations {
            execute_task(thread_index * iterations + i);
        }
    }

    println!(
        "Single-threaded computation time: {:.2} ms",
        start.elapsed().as_secs_f64() * 1000.0
    );
}

// 多线程执行
fn multi_thread_test(iterations: usize, thread_count: usize) {
    let start = Instant::now();

    let handles: Vec<_> = (0..thread_count)
        .map(|thread_index| {
            let iterations = iterations;
            thread::spawn(move || {
                for i in 0..iterations {
                    execute_task(thread_index * iterations + i);
                }
            })
        })
        .collect();

    for handle in handles {
        handle.join().unwrap();
    }

    println!(
        "Multi-threaded computation time: {:.2} ms",
        start.elapsed().as_secs_f64() * 1000.0
    );
}

fn main() {
    let iterations = 30; // 每个线程的任务数
    let thread_count = 50; // 线程数

    println!("Running single-threaded test...");
    single_thread_test(iterations, thread_count);

    println!("\nRunning multi-threaded test...");
    multi_thread_test(iterations, thread_count);
}
