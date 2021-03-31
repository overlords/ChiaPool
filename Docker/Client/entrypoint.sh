cd manager
./ChiaMiningManager.Client init

cd ../chia-blockchain

. ./activate

chia init
chia init -c ca
sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml
chia configure --set-farmer-peer ${pool_host}:${farmer_port}

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

chia start harvester

cd ../manager
./ChiaMiningManager.Client