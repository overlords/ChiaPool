cd manager
./ChiaPool.Plotter init

cd ../chia-blockchain

. ./activate

curl https://${pool_host}:${manager_port}/Cert/Keys -H "Authorization: ${token}" | chia keys add
chia init
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

cd ../manager
./ChiaPool.Plotter