sh runmanager.sh

cd /chia-blockchain

. ./activate

chia init

for path in ${plots_dir}.split(',') do
	chia plots add -d $path
done


sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

cat ~/.chia/mainnet/config/config.yaml

if [[ ${farmer_only} == 'true' ]]; then
  chia start farmer-only
elif [[ ${harvester_only} == 'true' ]]; then
  if [[ -z ${farmer_host} || -z ${farmer_port} ]]; then
    echo "A farmer peer address and port are required."
    exit
  else
    chia configure --set-farmer-peer ${farmer_host}:${farmer_port}
    chia start harvester
  fi
else
  chia start farmer
fi

while true; do sleep 30; done;