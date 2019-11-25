#!/bin/bash

dbname="test"
username="test"
psql $dbname $username << EOF
CREATE DATABASE elite;
\c elite;
CREATE EXTENSION pg_trgm;
EOF

