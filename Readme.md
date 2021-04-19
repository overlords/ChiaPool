# ChiaMiningManager
## What is this?
This application simplifies running multiple harvesters for the cryptocurrency [Chia](https://www.chia.net/).
Instead of having to do [the entire process](https://github.com/Chia-Network/chia-blockchain/wiki/Farming-on-many-machines) 
on every harvester machine this will get everything running with minimal configuration required.

This is used as a private mining pool until pools are officially supported by chia.

#### This is not a replacement for proper pools and should only be used as stated below:

| Can be used for                     | Cannot be used for                      |
|-------------------------------------|-----------------------------------------|
| Mining with others you **do** trust | Mining with others you **do not** trust |
| Self Pooling                        | Plots from different 24 word keys       |
           
**This is because of 2 main reasons:**
- The server cannot know how many plots the miners really have 
  - Pool must trust miners
- The mining rewards are stored on a wallet which is not accessible by miners
  - Miners must trust Pool
                 
## Installation
 - https://github.com/Playwo/ChiaPool/wiki/Installation

## Using the CLI Interface
Configure environment variables
  - **chiapoolcli_poolhost** - Address of the pools webinterface, e.g. https://my-chia-pool.com/
  - **chiapoolcli_minerhost** - Address to the miners webinterface, usually http://localhost:8888

The miner is optional, but some features will not be available if the CLI has no access to the local miner to authenticate requests.



## Wallet vs Farmer Keys
This pool uses 2 keys, one for farming and one for storing rewards. 
You can read about how it works [here](https://github.com/Chia-Network/chia-blockchain/wiki/Chia-Keys-Management).
This is enabled by default, you only need to add your keys to the node.env file.

|                        | Farmer key | Wallet Key |
|------------------------|------------|------------|
| Stores mined XCH       | No         | Yes        |
| Accessible by miners   | Yes        | No         |
| Accessible by plotters | Yes        | No         |
| Used for plotting      | Yes        | No         |
