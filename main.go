package main

import (
	"encoding/json"
	"fmt"
	"math"
	"runtime"
	"sync"
	"io/ioutil"
	"os"
	"time"
	"strings"
	"sort"
	"math/rand"
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
	// 1. 数学计算任务
	mathResult := 0.0
	for j := 1; j <= 100; j++ {
		mathResult += math.Sqrt(float64(j)) + math.Pow(float64(j), 1.8) + math.Sin(float64(j))*math.Cos(float64(j))
		mathResult *= math.Exp(-float64(j)/100.0) + math.Tan(float64(j)/50.0)
	}

	// 2. 大规模矩阵运算 (500x500)
	size := 200
	matrixA := make([][]float64, size)
	matrixB := make([][]float64, size)
	matrixResult := make([][]float64, size)
	for x := 0; x < size; x++ {
		matrixA[x] = make([]float64, size)
		matrixB[x] = make([]float64, size)
		matrixResult[x] = make([]float64, size)
		for y := 0; y < size; y++ {
			matrixA[x][y] = math.Sin(float64(x+y)) * math.Cos(float64(x-y))
			matrixB[x][y] = math.Exp(-float64(x*y)/500.0) + math.Tan(float64(y+1))
		}
	}
	for x := 0; x < size; x++ {
		for y := 0; y < size; y++ {
			for k := 0; k < size; k++ {
				matrixResult[x][y] += matrixA[x][k] * matrixB[k][y]
			}
		}
	}

	// 3. 字符串拼接与处理
	var sb strings.Builder
	for j := 0; j < 500; j++ {
		sb.WriteString(fmt.Sprintf("Task-%d-Line-%d;", i, j))
	}
	largeString := sb.String()
	processedString := strings.ReplaceAll(largeString, "Line", "ProcessedLine")

	// 4. 集合操作
	dataSet := make(map[int]struct{})
	for j := 0; j < 500; j++ {
		dataSet[j] = struct{}{}
	}
	// 查询数据
	for j := 0; j < 500; j++ {
		_, _ = dataSet[j]
	}

	// 5. 排序与查找
	array := make([]int, 200)
	for j := 0; j < len(array); j++ {
		array[j] = rand.Intn(1000000)
	}
	sort.Ints(array) // 排序

	// 二分查找
	target := rand.Intn(1000000)
	_ = sort.Search(len(array), func(idx int) bool {
		return array[idx] >= target
	})

	// 6. JSON 处理
	jsonObject := map[string]interface{}{
		"Id":   i,
		"MathResult": mathResult,
		"MatrixSample": []float64{
			matrixResult[0][0], matrixResult[1][1], matrixResult[2][2],
		},
		"ProcessedString": processedString[:100],
	}
	jsonData, _ := json.Marshal(jsonObject)

	// 7. 文件操作
	fileName := fmt.Sprintf("task_output_%d.json", i)
	_ = ioutil.WriteFile(fileName, jsonData, 0644) // 写入文件

	// 读取文件内容
	fileContent, _ := ioutil.ReadFile(fileName)

	// 删除文件
	_ = os.Remove(fileName)

	// 解析文件内容
	var parsedObject map[string]interface{}
	_ = json.Unmarshal(fileContent, &parsedObject)
}

func main() {
	const iterations = 20  // 每个线程的任务数
	const threadCount = 20 // 线程数

	fmt.Println("Running single-threaded test...")
	singleThreadTest(iterations, threadCount)

	fmt.Println("\nRunning multi-threaded test...")
	multiThreadTest(iterations, threadCount)
}
