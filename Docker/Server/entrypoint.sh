iptables -N WHITE 
iptables -A INPUT -p tcp --dport 8447 -j WHITE 
iptables -A INPUT -p tcp --dport 8447 -j DROP

cd chia-blockchain

. ./activate

if [ ${wallet_keys} == "" ]; then
  echo "You need to provide keys for the wallet!"
  echo "generating new keys, add them to the node.env file (wallet_keys=\"24words\")"
  chia keys generate_and_print
  exit 1
fi
if [ ${wallet_address} == ""]; then
	echo "You need to provide a wallet address!"
	echo "Your wallet address is the \"First wallet address\" in the following output. Add it to your node.env file (wallet_address=xch...)"
	echo "${wallet_keys}" | chia keys add
	chia keys show
	exit 1
fi
if [ ${farmer_keys} == ""]; then
	echo "You need to provide keys for plotting!"
	echo "generating new keys, add them to the node.env file (farmer_keys=\"24words\")"
	chia keys generate_and_print
	exit 1
fi

echo "${farmer_keys}" | chia keys add
echo "${wallet_keys}" | chia keys add

chia init
echo "${farmer_keys}" | chia keys add

for i in $(echo ${plot_dirs} | tr ";" "\n") 
do
  chia plots add -d $i
done

sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

chia start farmer

cd /root/.chia/mainnet/config/ssl/ca

zip -r /root/ca.zip .

cd /root/manager
./ChiaMiningManager.Server