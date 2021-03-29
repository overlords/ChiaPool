cd /chia-blockchain

. ./activate

chia init

if [[ ${keys} == "generate" ]]; then
  echo "to use your own keys pass them as a variable keys=\"24words\""
  chia keys generate
else
  echo "${keys}" | chia keys add
fi

if [[ ! "$(ls -A /plots)" ]]; then
  echo "Plots directory appears to be empty and you have not specified another, try mounting a plot directory"
fi

chia plots add -d ${plots_dir}

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