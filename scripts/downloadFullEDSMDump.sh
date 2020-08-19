#/bin/bash
rm systemsWithCoordinates.json.gz
wget https://www.edsm.net/dump/systemsWithCoordinates.json.gz -O systemsWithCoordinates.json.gz
python importsys_pgsql.py
exit 0
