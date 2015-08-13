echo -e "\033[32m ===> Starting SSL testing\033[0m"
mono /home/vagrant/sync/ssl-begintransacton/bin/Debug/ssl-begintransacton.exe > ssl.log &
mono_pid=$!
sleep 2

echo -e "\033[32m ===> Testing OrmLite\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/ormlite
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> OrmLite $avg_time ms\033[0m"

echo -e "\033[32m ===> Testing Nhibernate\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/nhibernate
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> Nhibernate $avg_time ms\033[0m"

echo -e "\033[32m ===> Testing Npgsql\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/npgsql
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> Npgsql $avg_time ms\033[0m"

kill $mono_pid

echo -e "\033[32m ===> Starting non-SSL testing\033[0m"
mono /home/vagrant/sync/ssl-begintransacton/bin/Debug/ssl-begintransacton.exe disabled > no-ssl.log &
mono_pid=$!
sleep 2

echo -e "\033[32m ===> Testing OrmLite\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/ormlite
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> OrmLite $avg_time ms\033[0m"

echo -e "\033[32m ===> Testing Nhibernate\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/nhibernate
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> Nhibernate $avg_time ms\033[0m"

echo -e "\033[32m ===> Testing Npgsql\033[0m"
ab -n 200 -c 100 http://127.0.0.1:9832/npgsql
avg_time=$(curl -s 127.0.0.1:9832/times\?format=json)
echo -e "\033[32m ===> Npgsql $avg_time ms\033[0m"

kill $mono_pid
