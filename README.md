# Parallel and Sequential Matrix Multiplication in .NET

This project implements and benchmarks various matrix multiplication algorithms, focusing on parallel computation using the striped approach. It is developed as part of a university course in Parallel Computing Technologies.

## Project Goal

To evaluate the efficiency of matrix multiplication algorithms under different execution models:
- Sequential (classical and striped imitation)
- Parallel (striped with partial and full column transfer)

The main objective is to demonstrate how multithreading can accelerate computations on large matrices using .NET technologies.

## Technologies Used

- **.NET 9.0**
- **C#**
- **Task Parallel Library (TPL)**
- **BlockingCollection** for thread-safe communication
- **Math.NET Numerics** for result verification
- **Docker** for scalable CPU-bound experiments

---

## Implemented Algorithms

### Classical Sequential
- Basic triple-nested loop method.
- Serves as a baseline for performance comparison.

### Sequential Striped (Imitated)
- Simulates striped logic without parallelism.
- Emulates cyclic column shifting.

### Parallel Striped (Partial Transfer)
- Each thread processes a row and exchanges matrix B columns via ring topology.
- Column communication implemented via `BlockingCollection`.

### Parallel Striped (Full Transfer)
- Each thread receives a full copy of matrix B.
- Avoids inter-thread communication, optimized for small-to-medium sizes.

---

## Verification

All implementations are verified against Math.NET's multiplication output.  
Verification includes:

- Element-wise comparison with allowed tolerance.
- Randomized matrix generation.
- Consistency check for sizes up to 2048×2048.

---

## Benchmarking & Testing

### Matrix Sizes
- Tested on square matrices from 64×64 to 3584×3584.

### CPU Scaling
- Experiments include 1–12 CPU cores using Docker.

### Scripts
- Local benchmarking: `run_benchmark.sh`
- Docker-based testing: `run_docker_benchmark.sh`

### Performance Highlights

- Striped imitation outperforms classical in sequential mode.
- Full transfer is faster for small matrices.
- Partial transfer scales better for large matrices (≥2560×2560).
- Maximum speedup:
    - 4.84x vs classical
    - 2.81x vs imitation
- Near-linear scaling observed up to 8 cores.

---

## Experimental Results

| Matrix Size | Classical (ms) | Imitated (ms) | Partial Parallel (ms) | Full Parallel (ms) |
|-------------|----------------|---------------|------------------------|--------------------|
| 1024        | 7877.34        | 6254.05       | 5413.65                | 3446.61            |
| 2048        | 126735.81      | 75985.13      | 29610.64               | 51397.53           |
| 3584        | 764320.81      | 443090.97     | 157936.29              | 269924.47          |

For detailed analysis, refer to the full report (Ukrainian).

---

## How to Run

```bash
# Build in Release
dotnet build -c Release

# Run with specific multiplier and matrix size
MULTIPLIER=PARALLEL_STRIPE_MULTIPLIER DIMENSION=1024 dotnet ./bin/Release/net9.0/MatrixCompute.Runner.dll
```

---

## Docker Usage

```bash
docker build -t matrix-compute .
docker run --rm --cpus=6 -e MULTIPLIER=STRIPE_MULTIPLIER -e DIMENSION=1024 matrix-compute
```

---

## License

Academic use only - created for coursework at university.
