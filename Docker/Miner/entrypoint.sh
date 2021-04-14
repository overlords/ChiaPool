cd manager
./ChiaPool.Miner init

cd ../chia-blockchain

. ./activate

chia init
chia init -c ca
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml
chia configure --set-farmer-peer ${farmer_host}

curl ${pool_host}/Cert/Keys -H "Authorization: Miner ${token}" | chia keys add

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

chia start harvester

cd ../manager
./ChiaPool.Miner