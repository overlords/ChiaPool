iptables -N WHITE 
iptables -A INPUT -p tcp --dport 8447 -j WHITE 
iptables -A INPUT -p tcp --dport 8447 -j DROP

cd chia-blockchain

./activate

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

chia start farmer

cd /root

zip -r ca.zip .chia/mainnet/config/ssl/ca

cd ../manager
./ChiaMiningManager.Server