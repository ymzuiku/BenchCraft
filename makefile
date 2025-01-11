go:
	go build -o main.exe main.go
	./main.exe
bun:
	bun run main.js
node:
	node main.js
cs:
	dotnet build -c Release Bench/Bench.csproj
	dotnet Bench\bin\Release\net9.0\Bench.dll
rust:
	cd rust_benchmark && cargo build --release
	./rust_benchmark/target/release/rust_benchmark
all:
	make go
	make bun
	make node
	make cs
	make rust