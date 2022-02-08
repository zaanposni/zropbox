FROM node:14.15 AS compile-frontend
WORKDIR /app

COPY . .

RUN node -v
RUN npm -v

RUN npm install
# RUN npm rebuild node-sass --force
RUN mkdir -p src/theme
RUN npx smui-theme compile public/build/smui.css -i src/theme
RUN npm run build

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=compile-frontend /app/public/ /var/www/data/
RUN ls -la /var/www/data/

# Set timezone
ENV TZ=Europe/Berlin
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

CMD ["nginx", "-g", "daemon off;"]

EXPOSE 80
