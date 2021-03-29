FROM ubuntu:20.04

EXPOSE 8888

ARG BRANCH

WORKDIR /root

RUN DEBIAN_FRONTEND=noninteractive apt-get update && apt-get upgrade -y
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y curl jq python3 ansible tar bash ca-certificates git openssl unzip wget python3-pip sudo acl build-essential python3-dev python3.8-venv python3.8-distutils apt nfs-common python-is-python3

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
&& dpkg -i packages-microsoft-prod.deb

RUN DEBIAN_FRONTEND=noninteractive apt-get update \
&& apt-get install -y apt-transport-https && \
&& apt-get update && \
&& apt-get install -y dotnet-sdk-5.0

RUN echo "Cloning ChiaMiningManager"
RUN git clone --branch main https://github.com/Playwo/ChiaMiningManager.git \
&& cd ChiaMiningManager/ChiaMiningManager.Client \
&& dotnet publish --self-contained -o /root/manager \
&& chmod +x /root/manager/ChiaMiningManager.Client 

RUN DEBIAN_FRONTEND apt purge -y dotnet-sdk-5.0 apt-transport-https #Only required for the build

RUN echo "Cloning chia-blockchain"
RUN git clone --branch ${BRANCH} https://github.com/Chia-Network/chia-blockchain.git \
&& cd chia-blockchain \
&& git submodule update --init mozilla-ca \
&& chmod +x install.sh \
&& /usr/bin/sh ./install.sh

WORKDIR /chia-blockchain
RUN mkdir /plots
ADD ./entrypoint.sh entrypoint.sh
ADD ./runmanager.sh runmanager.sh

ENTRYPOINT ["bash", "./entrypoint.sh"]