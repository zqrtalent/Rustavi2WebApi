#!/bin/bash

DOCKER_REGISTRY_URL="registry.heroku.com"

USER_NAME=$1 #Heroku username
HEROKU_APP=$2 #Heroku app name
HEROKU_APP_PROCESS_TYPE="web"
HEROKU_API_KEY=$3 #Heroku api key
BUILD_DIR='../Rustavi2WebApi'
BASIC_AUTH_VAL=$(echo ':${HEROKU_API_KEY}' | base64)

if [ "$4" != "" ]; then
  BUILD_DIR=$4
fi

#Login in docker registry.
docker login --username=$USER_NAME --password=$HEROKU_API_KEY $DOCKER_REGISTRY_URL

#Build docker image.
docker build -t ${HEROKU_APP}_image ${BUILD_DIR}/Rustavi2WebApi/

#Tag and push into container registry.
docker tag ${HEROKU_APP}_image ${DOCKER_REGISTRY_URL}/${HEROKU_APP}/${HEROKU_APP_PROCESS_TYPE} 
docker push ${DOCKER_REGISTRY_URL}/${HEROKU_APP}/${HEROKU_APP_PROCESS_TYPE}

#Get docker image id.
DOCKER_IMAGE_ID=$(docker inspect ${HEROKU_APP}_image --format={{.Id}})

#Release heroku app
curl -n -X PATCH https://api.heroku.com/apps/$HEROKU_APP/formation \
  -d '{
  "updates": [
    {
      "type": "'"$HEROKU_APP_PROCESS_TYPE"'",
      "docker_image": "'"$DOCKER_IMAGE_ID"'"
    }
  ]
}' \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer ${HEROKU_API_KEY}" \
  -H "Accept: application/vnd.heroku+json; version=3.docker-releases" \