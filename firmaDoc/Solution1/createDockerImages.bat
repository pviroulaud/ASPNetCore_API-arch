docker build -t pdviroulaud/userfileapi -f userfileapidocker .
docker build -t pdviroulaud/certificateapi -f certificateapidocker .
docker build -t pdviroulaud/userapi -f userapidocker .
docker build -t pdviroulaud/logapi -f logapidocker .
docker build -t pdviroulaud/documentapi -f documentapidocker .

docker push pdviroulaud/userfileapi
docker push pdviroulaud/certificateapi
docker push pdviroulaud/userapi
docker push pdviroulaud/logapi
docker push pdviroulaud/documentapi