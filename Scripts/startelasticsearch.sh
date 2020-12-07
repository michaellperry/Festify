#!/bin/bash

docker run -d -p 9200:9200 -e "discovery.type=single-node" \
-v esdata:/usr/share/elasticsearch/data \
docker.elastic.co/elasticsearch/elasticsearch:7.9.3