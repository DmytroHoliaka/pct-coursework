#!/bin/bash
set -e

echo "Building Docker image 'matrix-compute'..."
docker build -t matrix-compute ../../

multipliers=(
  "STRIPE_MULTIPLIER" 
  "PARALLEL_STRIPE_MULTIPLIER")  
cpu_limits=(1 2 4 6 8 10 12)
runs=20
dimension=2560

for cpus in "${cpu_limits[@]}"; do
  for multiplier in "${multipliers[@]}"; do
    echo "> MULTIPLIER=${multiplier} and CPU limit=${cpus} CPU(s)"
    
    sum=0
    for (( i=1; i<=runs; i++ )); do
      result=$(docker container run --rm --cpus=${cpus} \
        -e DIMENSION=${dimension} \
        -e MULTIPLIER=${multiplier} \
        matrix-compute)

      echo "Run ${i}: ${result} ms"
      sum=$(awk "BEGIN {print $sum + $result}")
    done
    
    average=$(awk "BEGIN {printf \"%.2f\", $sum / $runs}")
    echo "Average execution time: ${average} ms"
    echo ""
  done
done