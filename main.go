package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"math"
	"os"
	"runtime"
	"sync"
	"time"
)

// 打印内存使用
func printMemoryUsage(label string) {
	var m runtime.MemStats
	runtime.ReadMemStats(&m)

	fmt.Printf("%s Memory Usage: Alloc = %.2f MB, TotalAlloc = %.2f MB, Sys = %.2f MB, NumGC = %d\n",
		label,
		float64(m.Alloc)/1024/1024,
		float64(m.TotalAlloc)/1024/1024,
		float64(m.Sys)/1024/1024,
		m.NumGC,
	)
}

// 单线程执行
func singleThreadTest(iterations, threadCount int) {
	printMemoryUsage("Before Single-threaded Test")
	startTime := time.Now()

	for threadIndex := 0; threadIndex < threadCount; threadIndex++ {
		for i := 0; i < iterations; i++ {
			executeTask(i)
		}
	}

	endTime := time.Now()
	fmt.Printf("Single-threaded computation time: %.2f ms\n", endTime.Sub(startTime).Seconds()*1000)
	printMemoryUsage("After Single-threaded Test")
}

// 多线程执行
func multiThreadTest(iterations, threadCount int) {
	printMemoryUsage("Before Multi-threaded Test")
	startTime := time.Now()

	var wg sync.WaitGroup
	for threadIndex := 0; threadIndex < threadCount; threadIndex++ {
		wg.Add(1)
		go func(threadIndex int) {
			defer wg.Done()
			for i := 0; i < iterations; i++ {
				executeTask(i)
			}
		}(threadIndex)
	}

	wg.Wait()
	endTime := time.Now()
	fmt.Printf("Multi-threaded computation time: %.2f ms\n", endTime.Sub(startTime).Seconds()*1000)
	printMemoryUsage("After Multi-threaded Test")
}

// 执行任务逻辑
func executeTask(i int) {
	// 1. 复杂循环中的非线性运算
	result := 0.0
	for j := 1; j <= 100; j++ {
		result += math.Sqrt(float64(j)) + math.Sin(float64(j))*math.Cos(float64(j)) + math.Log(float64(j)+1)/math.Tan(float64(j)+0.1)
		result *= math.Exp(math.Mod(-float64(j), 5)) + math.Atan(float64(j)/10.0)
	}

	// 2. 模拟矩阵计算 (100x100 矩阵乘法)
	size := 100
	matrixA := make([][]float64, size)
	matrixB := make([][]float64, size)
	matrixResult := make([][]float64, size)
	for x := 0; x < size; x++ {
		matrixA[x] = make([]float64, size)
		matrixB[x] = make([]float64, size)
		matrixResult[x] = make([]float64, size)
		for y := 0; y < size; y++ {
			matrixA[x][y] = math.Sin(float64(x + y))
			matrixB[x][y] = math.Cos(float64(x - y))
		}
	}

	for x := 0; x < size; x++ {
		for y := 0; y < size; y++ {
			for k := 0; k < size; k++ {
				matrixResult[x][y] += matrixA[x][k] * matrixB[k][y]
			}
		}
	}

	// 3. 高级数学函数组合 (复杂表达式)
	specialResult := 0.0
	for j := 1; j <= 50; j++ {
		specialResult += math.Sqrt(float64(j)) * math.Sin(float64(j)) * math.Log10(float64(j)+1) + math.Exp(-float64(j))*math.Cos(math.Tan(float64(j)))
	}

	// 4. JSON 处理
	jsonObject := map[string]interface{}{
		"Id":   i,
		"Name": fmt.Sprintf("Test-%d", i),
		"Data": map[string]interface{}{
			"MatrixSum": specialResult,
			"Result":    result,
			"Details": []map[string]interface{}{
				{"Index": 1, "Value": matrixResult[0][0]},
				{"Index": 2, "Value": matrixResult[1][1]},
			},
		},
		"AdditionalData": []map[string]interface{}{},
	}

	for k := 0; k < 30; k++ {
		jsonObject["AdditionalData"] = append(jsonObject["AdditionalData"].([]map[string]interface{}), map[string]interface{}{
			"Index": k,
			"Value": k * 2,
		})
	}

	jsonString, _ := json.Marshal(jsonObject)
	var deserializedObject map[string]interface{}
	_ = json.Unmarshal(jsonString, &deserializedObject)

	// 5. 文件操作测试
	fileName := fmt.Sprintf("temp_file_%d.json", i)
	_ = ioutil.WriteFile(fileName, jsonString, 0644) // 写入文件
	fileContent, _ := ioutil.ReadFile(fileName)     // 读取文件
	_ = os.Remove(fileName)                         // 删除文件

	// 解析文件内容，模拟处理
	var fileData map[string]interface{}
	_ = json.Unmarshal(fileContent, &fileData)
}

func main() {
	const iterations = 30  // 每个线程的任务数
	const threadCount = 50 // 线程数

	fmt.Println("Running single-threaded test...")
	singleThreadTest(iterations, threadCount)

	fmt.Println("\nRunning multi-threaded test...")
	multiThreadTest(iterations, threadCount)
}
