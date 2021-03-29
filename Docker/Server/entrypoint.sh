cd chia-blockchain

. ./activate

chia init

if [ ${keys} == "generate" ]; then
  echo "to use your own keys pass them as a variable keys=\"24words\""
  chia keys generate
else
  echo "${keys}" | chia keys add
fi

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

cat ~/.chia/mainnet/config/config.yaml

chia start farmer

cd /root
sh runmanager.sh