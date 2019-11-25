#/bin/bash
rm systemsWithCoordinates.json
wget https://www.edsm.net/dump/systemsWithCoordinates.json -O systemsWithCoordinates.json
python importsys_pgsql.py
exit 0
