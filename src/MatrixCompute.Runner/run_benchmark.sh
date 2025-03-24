#!/bin/bash
set -e

echo "Building project in Release mode..."
dotnet build -c Release

multipliers=(
  "CLASSICAL_MULTIPLIER" 
  "STRIPE_MULTIPLIER" 
  "PARALLEL_STRIPE_MULTIPLIER" 
  "PARALLEL_BULK_MULTIPLIER")
dimensions=(64 128 256 512 1024 1536 2048 2560 3072 3584)
runs=20

dll_path="./bin/Release/net9.0/MatrixCompute.Runner.dll"

for dimension in "${dimensions[@]}"; do
  for multiplier in "${multipliers[@]}"; do
    echo "> MULTIPLIER=${multiplier} and DIMENSION=${dimension}"
    
    sum=0
    for (( i=1; i<=runs; i++ )); do
      result=$(MULTIPLIER=${multiplier} DIMENSION=${dimension} dotnet ${dll_path})
      
      echo "Run ${i}: ${result} ms"
      
      sum=$(awk "BEGIN {print $sum + $result}")
    done
    
    average=$(awk "BEGIN {printf \"%.2f\", $sum / $runs}")
    echo "Average execution time: ${average} ms"
    echo ""
  done
done
