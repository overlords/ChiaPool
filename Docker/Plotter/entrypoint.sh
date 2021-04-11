cd manager
./ChiaPool.Plotter init

cd ../chia-blockchain

. ./activate

chia init
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml
curl https://${pool_host}:${manager_port}/Cert/Keys -H "Authorization: Plotter ${token}" | chia keys add

cd ../manager
./ChiaPool.Plotter