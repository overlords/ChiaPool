# ChiaPool
## What is this?
This application is a trust based mining pool for the cryptocurrency [Chia](https://www.chia.net/).
It works by running a harvester and farmer (no node) on each miner machine and sending all rewards to the pool address.
Winning block rewards will be redistributed amongst miners.
Setting up miners is simple and can be done with minimal configuration required.

This can be used as a private mining pool until pools are officially supported by chia.

#### This is not a replacement for proper pools and should only be used as stated below:

| Can be used for                     | Cannot be used for                      |
|-------------------------------------|-----------------------------------------|
| Mining with others you **do** trust | Mining with others you **do not** trust |
| Self Pooling                        |                                         |
           
**This is because of 2 main reasons:**
- The server cannot know securely measure how many plots miners really have & miners could change the reward address back to their own wallet
  - Pool must trust miners
- The mining rewards are stored on a wallet which is not directly accessible by miners
  - Miners must trust the Pool
                 
## Installation
 - https://github.com/Playwo/ChiaPool/wiki/Installation

## Using the CLI Interface
Configure environment variables
  - **chiapoolcli_poolhost** - Address of the pools webinterface, e.g. https://my-chia-pool.com/
  - **chiapoolcli_minerhost** - Address to the miners webinterface, usually http://localhost:8888

Running a miner for the CLI is optional, but some features will not be available if the CLI has no access to the local miner to authenticate requests.



## Wallet vs Plotting Keys
This pool uses 2 keys, one for farming and one for storing rewards. 
You can read about how it works [here](https://github.com/Chia-Network/chia-blockchain/wiki/Chia-Keys-Management).
This is enabled by default, you only need to add your keys to the node.env file.

|                        | Plotting Keys |  Wallet Keys |
|------------------------|---------------|--------------|
| Stores mined XCH       | No            | Yes          |
| Accessible by miners   | Yes           | No           |
| Accessible by plotters | Only pks      | No           |
| Used for plotting      | Yes           | No           |
