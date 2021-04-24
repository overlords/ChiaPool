cd /root/chia-blockchain

. ./activate

chia init
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

cd /root/manager

chia start farmer-only
./ChiaPool.Miner init

curl ${pool_host}/Keys/Plotting -H "Authorization: Miner ${token}" | chia keys add

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

chia start harvester
./ChiaPool.Miner