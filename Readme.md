# ChiaMiningManager
### What is this?
This application simplifies running multiple harvesters for the cryptocurrency [Chia](https://www.chia.net/).
Instead of doing [the entire process](https://github.com/Chia-Network/chia-blockchain/wiki/Farming-on-many-machines) this will do everything for you with minimal configuration required.

This is used as a private mining pool until pools are officially supported by chia.

## Building
 - Clone the project
 - Go to the subfolder of Docker depending on which one you want to build
 - `docker build . -t [ImageName] --build-arg BRANCH=main --no-cache`

