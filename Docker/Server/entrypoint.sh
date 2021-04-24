set -e

cd /root/chia-blockchain
. ./activate
chia init

if [[ -z "${wallet_keys}" ]]; then
	echo "You need to provide keys for the wallet!"
	echo "generating new keys, add them to the node.env file (wallet_keys=\"24words\")"
	chia keys generate_and_print
	echo "Aborting..."
  exit 1
fi
if [[ -z "${db_connection}" ]]; then
	echo "You need to provide a postgres connection string!"
	echo "Aborting..."
	exit 1
fi

echo "${wallet_keys}" | chia keys add

sed -i 's/localhost/127.0.0.1/g' ~/.chia/mainnet/config/config.yaml

chia start wallet

cd /root/manager
./ChiaPool.Server init

{ echo "1"; echo "S"; } | chia wallet show

./ChiaPool.Server