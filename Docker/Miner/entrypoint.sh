set -e

cd /root/chia-blockchain

. ./activate

chia init
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

echo "${farmer_keys}" | chia keys add

chia start farmer-only

cd /root/manager
./ChiaPool.Miner init

echo "${plotting_keys}" | chia keys add

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

chia start harvester
./ChiaPool.Miner