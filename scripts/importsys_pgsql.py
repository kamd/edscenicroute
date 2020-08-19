import psycopg2
import psycopg2.extras
import json
import gzip

INIT_STRING = 'INSERT INTO "GalacticSystems" ("Name", "Coordinates_X", "Coordinates_Y", "Coordinates_Z") VALUES %s'
UPDATE_STRING = 'INSERT INTO "GalacticSystems" ("Name", "Coordinates_X", "Coordinates_Y", "Coordinates_Z") VALUES %s' +\
                ' ON CONFLICT("Name") DO UPDATE SET "Coordinates_X"="excluded"."Coordinates_X", ' +\
                '"Coordinates_Y"="excluded"."Coordinates_Y", "Coordinates_Z"="excluded"."Coordinates_Z";'

def push_to_database(execute_string):
    json_file = "systemsWithCoordinates.json.gz"
    with gzip.open(json_file, mode='rt') as f:
        connection = get_connection()
        cursor = connection.cursor()
        system_data = []
        lnumber = 0
        for line in f:
            lnumber = lnumber + 1
            if lnumber % 100000 == 0:
                print('...' + str(lnumber))
            if lnumber % 1000 == 0:
                commit_systems(system_data, cursor, execute_string, connection)
                system_data = []
            if line[0] == '[' or line[0] == ']': # ditch the start and end
                continue
            if line[-2] == ',':
                line = line[0:-2] # ditch the trailing comma and line break
            else:
                line = line[0:-1] # ditch just line break
            # print("line:" + line + ":")
            d = json.loads(line)
            try:
                c = d["coords"]
                system_data.append((d["name"], c["x"], c["y"], c["z"]))
            except KeyError:
                print('badline: ' + str(d))
                continue
    commit_systems(system_data, cursor, execute_string, connection)
    print('file ' + json_file + ' imported.')
    connection.close()

def commit_systems(system_data, cursor, execute_string, connection):
    try:
        # print(system_data)
        psycopg2.extras.execute_values(cursor, execute_string, system_data, template=None, page_size=100)
        connection.commit()
    except psycopg2.errors.CardinalityViolation:
        print('CardinalityViolation, reducing...')
        connection.rollback()
        halfSystems = len(system_data)/2
        if halfSystems == 0:
            print('skipping: ' + str(system_data))
            return
        commit_systems(system_data[0:halfSystems], cursor, execute_string,
                connection)
        commit_systems(system_data[halfSystems:len(system_data)], cursor,
                execute_string, connection)


def get_connection():
    return psycopg2.connect(
        host="localhost",
        user="",
        password="",
        database="elite"
    )

if __name__ == "__main__":
    push_to_database(UPDATE_STRING)

