#!/bin/bash

ENV_VARS=$(printenv | grep 'WM_')
JSON_FILE="{}"

while read -r i; do
    VAR_NAME=$(echo "$i" | sed 's/\(WM_.*\)=.*/\1/g')
    VAR_VALUE=$(echo "$i" | sed 's/.*=\(.*\)/\1/g')


    JSON_FILE=$(echo "$JSON_FILE" | jq --arg var_name "$VAR_NAME" --arg var_value "$VAR_VALUE" '. + {($var_name): $var_value}')
done < <(echo "$ENV_VARS")

echo "$JSON_FILE" > /usr/share/nginx/html/appsettings.Production.json
