## [1.0.4](https://github.com/GlobalX/PockyBot.NET/compare/v1.0.3...v1.0.4) (2020-01-23)


### Bug Fixes

* make sure correct things are pulled from the db in each place ([09e3d62](https://github.com/GlobalX/PockyBot.NET/commit/09e3d628f74c749ad26587db215be434d5026920))

## [1.0.3](https://github.com/GlobalX/PockyBot.NET/compare/v1.0.2...v1.0.3) (2020-01-22)


### Bug Fixes

* specify foreign key relationship of userlocation ([9c2f95c](https://github.com/GlobalX/PockyBot.NET/commit/9c2f95cac8304b831e6f743f4c050a65da02000a))

## [1.0.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.0.1...v1.0.2) (2020-01-22)


### Bug Fixes

* ignore bots ([c2c3ca6](https://github.com/GlobalX/PockyBot.NET/commit/c2c3ca60ab6531a81534e2500e90b42bbcaf8a77))

## [1.0.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.0.0...v1.0.1) (2020-01-20)


### Bug Fixes

* move persistence into main project so it doesn't get referenced separately ([f69b980](https://github.com/GlobalX/PockyBot.NET/commit/f69b98019c1d6a19e79640d4ff05b8439db82b01))

# 1.0.0 (2020-01-20)


### Bug Fixes

* add make sure collapse ids are unique and fix category results ([49f8c9d](https://github.com/GlobalX/PockyBot.NET/commit/49f8c9ddffd96875683a3332729760983031ce45))
* bold usernames in status response ([c4ded92](https://github.com/GlobalX/PockyBot.NET/commit/c4ded9241d134087f2f0a69f6bca2836e870efa7))
* configure awaited calls so that execution can continue on any thread ([45a24a4](https://github.com/GlobalX/PockyBot.NET/commit/45a24a4eb320f1c526313f9da1d1b5bd58e2a824))
* fix peg comment validator ([0a2989c](https://github.com/GlobalX/PockyBot.NET/commit/0a2989c86bd2bbf19773d1063db7edfaa9041a62))
* fix peg request validation logic ([8768aa5](https://github.com/GlobalX/PockyBot.NET/commit/8768aa53193ef62ec32c6d5d659ba56db84f56ea))
* fix send penalty peg ([9650a9c](https://github.com/GlobalX/PockyBot.NET/commit/9650a9c11c0df33db37c27e76d9d41bbea92609b))
* minor style issues ([32eef23](https://github.com/GlobalX/PockyBot.NET/commit/32eef230cce8acef1d08d8727f235195b86515f0))
* properly account for unmetered users ([e956385](https://github.com/GlobalX/PockyBot.NET/commit/e956385456f559113a73a9bb42c85af5d02894ba))
* some more minor style issues ([e49f5f2](https://github.com/GlobalX/PockyBot.NET/commit/e49f5f26bfa736defbd14a631819387f87dbc4cf))
* status message should be PM'd to sender ([a56af7c](https://github.com/GlobalX/PockyBot.NET/commit/a56af7ccb53c440b7ed76b0977fd8c831abb214b))
* **finish:** fix build errors ([7c5661b](https://github.com/GlobalX/PockyBot.NET/commit/7c5661bdda8e21aca5c47bdf84f0493dacd1874a))
* tweak database models ([c1e4e82](https://github.com/GlobalX/PockyBot.NET/commit/c1e4e82d8af1f627b2319204cf31de48d61638d7))
* update db representation for foreign keys ([be98e45](https://github.com/GlobalX/PockyBot.NET/commit/be98e455fb52ca7ef60b1b8d9fbf2f0ca57ad242))
* use ordinal string comparison ([5b36af8](https://github.com/GlobalX/PockyBot.NET/commit/5b36af87a1e09714fa8bbc1c3b0bfd1d6fae5d3f))
* use string ignore case ([2d5bcfc](https://github.com/GlobalX/PockyBot.NET/commit/2d5bcfc74f0fee1c31088eb38c28ca421b815db0))


### Features

* add default trigger ([c8b197c](https://github.com/GlobalX/PockyBot.NET/commit/c8b197cc52a1e79b7ae5f4bacbe98d996eb038af))
* allow trigger to specify whether direct messaging is allowed ([d3f7b57](https://github.com/GlobalX/PockyBot.NET/commit/d3f7b57d64385cfda2830b467199fc2651d90023))
* implement peg ([0859a83](https://github.com/GlobalX/PockyBot.NET/commit/0859a83bd53e07f62342840805df4b57d5ba6f40))
* implement reset ([0443934](https://github.com/GlobalX/PockyBot.NET/commit/0443934524692d434ad1d4db7565047bb2728929))
* implement status trigger ([398fce8](https://github.com/GlobalX/PockyBot.NET/commit/398fce8846eae526e127629f968fe4c33258654f))
* make creating pegs asynchronous ([6905ad4](https://github.com/GlobalX/PockyBot.NET/commit/6905ad45d74acf417767b6ab9f28c4dd014df702))
* scaffold out triggers and implement ping ([0a45b46](https://github.com/GlobalX/PockyBot.NET/commit/0a45b4635086a992f183ca22be1f5036067d2bec))
* send direct messages to peg recipients on Finish ([5173781](https://github.com/GlobalX/PockyBot.NET/commit/5173781040b9dc4d8e64ad14a29c8e6f074e9675))
* send messages async ([984371f](https://github.com/GlobalX/PockyBot.NET/commit/984371fcce8869f291b8ea7be0f903b0abe07b05))
* update peg received text ([eecbedb](https://github.com/GlobalX/PockyBot.NET/commit/eecbedba801d2cae1be6f198f3dba602556439d8))
