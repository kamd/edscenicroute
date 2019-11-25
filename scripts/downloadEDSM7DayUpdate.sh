#/bin/bash
rm systemsWithCoordinates.json
wget https://www.edsm.net/dump/systemsWithCoordinates7days.json -O systemsWithCoordinates.json
python importsys_pgsql.py
exit 0

