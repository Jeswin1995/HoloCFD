#!/bin/bash

# Define the output file
output_file="HE14.stl"

# Check if output file already exists and remove it to avoid appending
if [ -f "$output_file" ]; then
    rm "$output_file"
fi

# Find all STL files in the current directory and concatenate them
for stl in *.stl
do
  # Check if the file is not the output file to avoid self-appending
  if [ "$stl" != "$output_file" ]; then
    cat "$stl" >> "$output_file"
  fi
done

echo "All STL files have been combined into $output_file"
