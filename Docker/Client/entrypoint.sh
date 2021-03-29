cd chia-blockchain

. ./activate

chia init

for i in $(echo ${plots_dir} | tr ";" "\n") 
do
  chia plots add -d $i
done

sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

cat ~/.chia/mainnet/config/config.yaml

chia configure --set-farmer-peer pool.playwo.de:8447
chia start harvester

cd /root
sh runmanager.sh