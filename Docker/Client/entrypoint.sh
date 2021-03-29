cd chia-blockchain

. ./activate

chia init

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

chia configure --set-farmer-peer ${farmer_host}:${farmer_port}
chia start harvester

cd /root
sh runmanager.sh