#! /bin/bash

# ==========================================================================
# Run this script to download all the input files from AOC. Since AOC
# requests that input files are not included in git repos, they're not.
# This expects the cookiesession file to exist (cookiesession.example
# shows what it should look like). You can get the cookie by signing into
# AOC and opening the devtools of the browser and finding the cookie 
# header.
# ==========================================================================

# Load the cookie session data out of the file.
COOKIE_SESSION=$(<cookiesession)

# Get the directories that follow the year naming convention (\d{4}).
YEARS=$(find -regex '\.\/20[0-9][0-9]' | grep -oe "[0-9][0-9][0-9][0-9]")

# Loop over every year found.
for year in $YEARS 
do 
	printf 'Handling year %s:\n' $year

	# Create the input directory if it does not yet exist.
	if [ ! -d "$year/Input" ]
	then
		mkdir "$year/Input"
	fi

	# Go through each day.
	for i in {1..25}
	do
		# Calculate the input file path from the current dir.
		INP_FILE="$year/Input/$(printf '%02d' $i).txt"
		printf 'Day %s\t%s\n' $i $INP_FILE

		# Only download the input file if it does not yet exist.
		if [ ! -f "$INP_FILE" ]
		then
			AOC_URL="https://adventofcode.com/$year/day/$i/input"
			printf 'Downloading "%s" from "%s"\n' $INP_FILE $AOC_URL
			
			# The downloading.
			curl -H "Cookie: session=$COOKIE_SESSION" "$AOC_URL" > $INP_FILE
		else
			printf 'Skipping...\n'
		fi
	done
done

printf 'Completed Script'
