FROM node:18.15.0

WORKDIR /app

COPY package.json .

RUN npm i

COPY . .

CMD [ "npm", "run", "dev" ]