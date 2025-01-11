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
all:
	make go
	make bun
	make node
	make cs