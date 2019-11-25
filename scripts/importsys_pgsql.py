import psycopg2
import psycopg2.extras
import json

INIT_STRING = 'INSERT INTO "GalacticSystems" ("Name", "Coordinates_X", "Coordinates_Y", "Coordinates_Z") VALUES %s'
UPDATE_STRING = 'INSERT INTO "GalacticSystems" ("Name", "Coordinates_X", "Coordinates_Y", "Coordinates_Z") VALUES %s' +\
                ' ON CONFLICT("Name") DO UPDATE SET "Coordinates_X"="excluded"."Coordinates_X", ' +\
                '"Coordinates_Y"="excluded"."Coordinates_Y", "Coordinates_Z"="excluded"."Coordinates_Z";'

def push_to_database(execute_string):
    json_file = "systemsWithCoordinates.json"
    with open(json_file) as f:
        connection = get_connection()
        cursor = connection.cursor()
        system_data = []
        lnumber = 0
        for line in f:
            lnumber = lnumber + 1
            if lnumber % 1000 == 0:
                psycopg2.extras.execute_values(cursor, execute_string, system_data, template=None, page_size=100)
                connection.commit()
                connection.close()
                connection = get_connection()
                cursor = connection.cursor()
                system_data = []
            if line[0] == '[' or line[0] == ']': # ditch the start and end
                continue
            line = line[0:-2] # ditch the trailing comma and line break
            # print("line:" + line + ":")
            d = json.loads(line)
            try:
                c = d["coords"]
                system_data.append((d["name"], c["x"], c["y"], c["z"]))
            except KeyError:
                print('badline: ' + str(d))
                continue
    cursor.executemany(execute_string, system_data)
    connection.commit()
    print('file ' + json_file + ' imported.')
    connection.close()

def get_connection():
    return psycopg2.connect(
        host="localhost",
        user="",
        password="",
        database="elite"
    )

if __name__ == "__main__":
    push_to_database(UPDATE_STRING)

