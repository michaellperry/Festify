#!/bin/bash

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pass@word1' -p 1433:1433 \
-v sqlvolume:/var/opt/mssql \
-d mcr.microsoft.com/mssql/server:2019-latest
