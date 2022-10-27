## [1.16.4](https://github.com/GlobalX/PockyBot.NET/compare/v1.16.3...v1.16.4) (2022-10-27)


### Bug Fixes

* allow newlines in peg results ([4c55cd0](https://github.com/GlobalX/PockyBot.NET/commit/4c55cd088e7fe216a6f7e4b41e9bb37bee5572dc))

## [1.16.3](https://github.com/GlobalX/PockyBot.NET/compare/v1.16.2...v1.16.3) (2022-10-27)


### Bug Fixes

* **deps:** bump Scriban from 2.1.2 to 5.5.0 in /src/PockyBot.NET ([5c578f3](https://github.com/GlobalX/PockyBot.NET/commit/5c578f31ad9d56bda7978789d879b2df8f45750f))

## [1.16.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.16.1...v1.16.2) (2022-08-15)


### Bug Fixes

* don't use task.whenall for dbcontext calls ([55c000c](https://github.com/GlobalX/PockyBot.NET/commit/55c000c513447c9189e1f3cd3ea14fd4cb8c15b1))

## [1.16.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.16.0...v1.16.1) (2022-07-11)


### Bug Fixes

* remove code added as a joke ([77a5bd0](https://github.com/GlobalX/PockyBot.NET/commit/77a5bd0c565ae713d4fdc4caba9095b4f2332729))
* reply inline to unpeg messages ([255f79a](https://github.com/GlobalX/PockyBot.NET/commit/255f79aac1361fe302e26b83c80615efc6791ddc))

# [1.16.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.5...v1.16.0) (2022-07-04)


### Features

* add database migrations ([57badfe](https://github.com/GlobalX/PockyBot.NET/commit/57badfec816302074f479e02942aec5952ec25c7))

## [1.15.5](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.4...v1.15.5) (2022-06-28)


### Bug Fixes

* replace double newlines properly ([3557db6](https://github.com/GlobalX/PockyBot.NET/commit/3557db64253f997904fde5f4bcbf898d7b98fa9f))
* replace newlines in status command so that pegs display properly ([f7400f8](https://github.com/GlobalX/PockyBot.NET/commit/f7400f82c006b4e3d7dbba46bfe1e734bc92c22b))
* use correct quotes in template ([77d4cf6](https://github.com/GlobalX/PockyBot.NET/commit/77d4cf6c30c63bf4d593f5109c8341c953b0916e))

## [1.15.4](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.3...v1.15.4) (2022-06-15)


### Bug Fixes

* change order of peg checks so that peg limit is checked before peg validity ([51c979a](https://github.com/GlobalX/PockyBot.NET/commit/51c979a6ab11c2fc32c21864286fe48048c62f4e))
* fix unit tests ([1cb7775](https://github.com/GlobalX/PockyBot.NET/commit/1cb77750cef9951421a0bb748d92f0edb2e5f309))
* update status trigger to not treat linked keyword pegs as penalty pegs ([dfd80ab](https://github.com/GlobalX/PockyBot.NET/commit/dfd80abc99e055360b5726a2c41bddda676a461f))

## [1.15.3](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.2...v1.15.3) (2022-06-09)


### Bug Fixes

* make getuser async so that getpersonasync can be awaited and exception can be caught ([997ee0a](https://github.com/GlobalX/PockyBot.NET/commit/997ee0a9137d15adb6213cc64426d7db9a2bded8))

## [1.15.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.1...v1.15.2) (2022-06-08)


### Bug Fixes

* **username updater:** suppress user not found errors in username updater so that finish doesn't stop if the usernames can't be updated ([f35f332](https://github.com/GlobalX/PockyBot.NET/commit/f35f33274b07574ce5a0e80d0c88112ac13ef98a))
* add error handling for direct message sending and add test ([2518422](https://github.com/GlobalX/PockyBot.NET/commit/2518422536122905895f8f723cce5b77dff84ce8))

## [1.15.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.15.0...v1.15.1) (2022-02-20)


### Bug Fixes

* add unit test for penaltyKeyword ([072fa4c](https://github.com/GlobalX/PockyBot.NET/commit/072fa4c30cdadad012166fd13f68b9db564e9446))
* incorrect peg checking behavior ([6c9ae52](https://github.com/GlobalX/PockyBot.NET/commit/6c9ae522a91271b7729f5265e1fe2fbb9b53eea1))
* separate check for penalty pegs ([eb6a310](https://github.com/GlobalX/PockyBot.NET/commit/eb6a310e1dc2fd0593d5b8619e8f5fbf3047d2ba))
* unit tests ([c676a51](https://github.com/GlobalX/PockyBot.NET/commit/c676a5173be7e75b4eebae64ebf7460acca0e76d))

# [1.15.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.14.0...v1.15.0) (2022-02-11)


### Features

* respond with parentId ([3c1d262](https://github.com/GlobalX/PockyBot.NET/commit/3c1d262103e7a0ae92032dd94f4e560a1ea2cff8))

# [1.14.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.6...v1.14.0) (2022-01-28)


### Features

* add clarification around linked keywords ([b3b9390](https://github.com/GlobalX/PockyBot.NET/commit/b3b9390ce1b1b716777fcb9898e2556fc9cef81e))
* add examples to the stringconfig help ([61f2d3c](https://github.com/GlobalX/PockyBot.NET/commit/61f2d3c3e7beb10a302ef86de455ff6bd8c69983))
* initial linked keywords additions ([9c6cbb7](https://github.com/GlobalX/PockyBot.NET/commit/9c6cbb70543e018d4d597a442f95fbf5f9ebfc13))
* update peg request validator unit tests for addition of linked keywords ([6742de2](https://github.com/GlobalX/PockyBot.NET/commit/6742de29d6b0d1139fede50a4a05f56d7af9ea6d))
* update peg request validator unit tests for addition of linked keywords ([f9fe369](https://github.com/GlobalX/PockyBot.NET/commit/f9fe3698e2b7bd03dabee15bee41e93e0875985b))

## [1.13.6](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.5...v1.13.6) (2021-10-22)


### Bug Fixes

* minor rewording ([60dee52](https://github.com/GlobalX/PockyBot.NET/commit/60dee52841c41bddc12301be9ad1893633104ba4))
* validate whether peg sender is the same as peg receiver ([1b8155f](https://github.com/GlobalX/PockyBot.NET/commit/1b8155f75cd446114fa58c9a5c2661c2bdb42c7b))

## [1.13.5](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.4...v1.13.5) (2021-10-15)


### Bug Fixes

* **database:** update maximum comment length to 2000 chars ([6c1a501](https://github.com/GlobalX/PockyBot.NET/commit/6c1a501e57c9fc58fe259457d919ae83a76c60b1))

## [1.13.4](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.3...v1.13.4) (2021-10-13)


### Bug Fixes

* **generation:** bad code ([85a87d2](https://github.com/GlobalX/PockyBot.NET/commit/85a87d2cc1e93990b4725177c46d2e09f7161136))
* **generation:** sanitise template outputs and display newlines ([92762ab](https://github.com/GlobalX/PockyBot.NET/commit/92762abef00b88c27f0d6562cc09263804d9dcee))
* **generation:** sanitise template outputs and display newlines for all comments ([f6278d0](https://github.com/GlobalX/PockyBot.NET/commit/f6278d0632cf9196ea1c1e48e26ed5c66f30d3e0))

## [1.13.3](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.2...v1.13.3) (2021-10-13)


### Bug Fixes

* Fixing various typos in readme and welcome ([cf126bd](https://github.com/GlobalX/PockyBot.NET/commit/cf126bd89faaa260504a362517e8f2e5e8484d5c))
* typo in welcome message fixed form ouy to out ([8d3e0de](https://github.com/GlobalX/PockyBot.NET/commit/8d3e0de1bf4476ecc9b15951ec08ffa75f8d5e8a))

## [1.13.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.1...v1.13.2) (2020-09-25)


### Bug Fixes

* **deps:** bump GlobalX.ChatBots.Core in /src/PockyBot.NET ([5b0d196](https://github.com/GlobalX/PockyBot.NET/commit/5b0d1968d6a181b8fb54bb3808f14f7563dceb41))

## [1.13.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.13.0...v1.13.1) (2020-08-06)


### Bug Fixes

* make keyword detection case insensitive in finish ([6036b30](https://github.com/GlobalX/PockyBot.NET/commit/6036b309bbed0a14d830678a06c0c89306bd75c6))

# [1.13.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.12.0...v1.13.0) (2020-08-01)


### Features

* implement unpeg trigger ([46f611a](https://github.com/GlobalX/PockyBot.NET/commit/46f611acf5d50271c9d453c660f0827c3037db60))
* move delay into service ([102b3da](https://github.com/GlobalX/PockyBot.NET/commit/102b3da1b5e903a5cae146d368f623612c01e783))

# [1.12.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.11.1...v1.12.0) (2020-05-12)


### Features

* update usernames on finish command ([353d94f](https://github.com/GlobalX/PockyBot.NET/commit/353d94f2d39a9cf820145ffae279e71475116582))

## [1.11.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.11.0...v1.11.1) (2020-04-03)


### Bug Fixes

* use correct logging dependency in constructors ([aa30ffb](https://github.com/GlobalX/PockyBot.NET/commit/aa30ffbb28179ea175ff1977c9f66e11d05f7760))

# [1.11.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.10.1...v1.11.0) (2020-04-02)


### Features

* add in results trigger ([a701667](https://github.com/GlobalX/PockyBot.NET/commit/a701667d8c26e45719c38e12af028d4600f47c25))
* add Results and Numberconfig triggers to the help message ([da41b42](https://github.com/GlobalX/PockyBot.NET/commit/da41b42727c127beb9b6a2e5087b7e1c4f1be078))

## [1.10.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.10.0...v1.10.1) (2020-03-31)


### Bug Fixes

* update enum mapping ([59ed4f7](https://github.com/GlobalX/PockyBot.NET/commit/59ed4f76365a1e42ceaaa1c5c127de0ed4120867))
* use enums for role ([44e56f9](https://github.com/GlobalX/PockyBot.NET/commit/44e56f9bc904ea7edf1d3384c922bad7ccad91c2))

# [1.10.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.9.1...v1.10.0) (2020-03-30)


### Features

* add numberconfig trigger ([a331c4b](https://github.com/GlobalX/PockyBot.NET/commit/a331c4bac122b2e4a3e337fbe1fa6916e997e109))

## [1.9.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.9.0...v1.9.1) (2020-03-30)


### Bug Fixes

* **user-location-getter:** handle nulls gracefully ([80718bc](https://github.com/GlobalX/PockyBot.NET/commit/80718bc78919fa8c7420d9fe8ca59b9f9a1a468e))

# [1.9.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.8.3...v1.9.0) (2020-03-24)


### Bug Fixes

* update async code ([ed51ebc](https://github.com/GlobalX/PockyBot.NET/commit/ed51ebccbded6ec91dd415d95edae0917027ed78))
* use AppendLine ([c4d4bc3](https://github.com/GlobalX/PockyBot.NET/commit/c4d4bc320394df55965cf65c1b1b747080a35cdb))


### Features

* add roleconfig to help message ([6728d75](https://github.com/GlobalX/PockyBot.NET/commit/6728d75a391daf1ca357d234a2be5808757181a4))
* implement roleconfig trigger ([6c71ea9](https://github.com/GlobalX/PockyBot.NET/commit/6c71ea9744a369563ae3e5672906510f2f8f3279))

## [1.8.3](https://github.com/GlobalX/PockyBot.NET/compare/v1.8.2...v1.8.3) (2020-03-13)


### Bug Fixes

* **user-location:** handle case where new user has location set ([228fe41](https://github.com/GlobalX/PockyBot.NET/commit/228fe41eb89f21d0ce90b0d61e3e18f21fa79d65))

## [1.8.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.8.1...v1.8.2) (2020-03-12)


### Bug Fixes

* make helper classes' constructors public ([edec725](https://github.com/GlobalX/PockyBot.NET/commit/edec725731ee881a90add721f1ec3c95ed66d561))

## [1.8.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.8.0...v1.8.1) (2020-03-11)


### Bug Fixes

* inject missing dependencies ([f7a37b6](https://github.com/GlobalX/PockyBot.NET/commit/f7a37b61116f46c895ffe8cec7f249b72c8c757a))

# [1.8.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.7.0...v1.8.0) (2020-03-11)


### Features

* add location config trigger to startup code ([b46029e](https://github.com/GlobalX/PockyBot.NET/commit/b46029e1d5bcd52c080f575742c8abfdcbb2fb2f))
* add location weight trigger ([c42acbd](https://github.com/GlobalX/PockyBot.NET/commit/c42acbdbdc8fe37f362545ff0729e55d7cb8d1b9))
* add locationconfig to help trigger ([23b6d7d](https://github.com/GlobalX/PockyBot.NET/commit/23b6d7d4a8e6a7649d2980b33cac5b97b0691672))
* add locationconfig trigger ([989a8da](https://github.com/GlobalX/PockyBot.NET/commit/989a8da3a9376f2c89b6d36113774c7ecd58d5f3))
* add locationweight command to help message ([74a1c7d](https://github.com/GlobalX/PockyBot.NET/commit/74a1c7dd3c90f3459a0b261a472bb0cadb8b603d))
* rename 'set' to 'add' ([39ca4da](https://github.com/GlobalX/PockyBot.NET/commit/39ca4da6fd8a4050864ad4517c921267b76c1d80))

# [1.7.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.6.2...v1.7.0) (2020-03-03)


### Features

* add keywords trigger ([b9e6bbd](https://github.com/GlobalX/PockyBot.NET/commit/b9e6bbd3117ca594a3cd52fc67555164ff9e5e9e))

## [1.6.2](https://github.com/GlobalX/PockyBot.NET/compare/v1.6.1...v1.6.2) (2020-02-25)


### Bug Fixes

* fix db representation so that users can be accessed and removed properly ([4d2f153](https://github.com/GlobalX/PockyBot.NET/commit/4d2f15370591dc37805a0a361a2890d3d6ad95f1))

## [1.6.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.6.0...v1.6.1) (2020-02-21)


### Bug Fixes

* add removeuser to help trigger ([42a5970](https://github.com/GlobalX/PockyBot.NET/commit/42a5970d08a237857b23904f40a3ce0694b41676))
* fix removeuser null reference and add logging ([3e6519c](https://github.com/GlobalX/PockyBot.NET/commit/3e6519c67c9dc5f3ad6259f5788e30eea6d1fcbb))

# [1.6.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.5.1...v1.6.0) (2020-02-17)


### Bug Fixes

* code style issues - make test objects readonly ([1334a30](https://github.com/GlobalX/PockyBot.NET/commit/1334a3058d5f11695073867ccd7d96bcb31dcbaa))


### Features

* add removeuser trigger ([07d9213](https://github.com/GlobalX/PockyBot.NET/commit/07d9213fcf0cbb9f4cb1b6628d9d31e1de79eb46))

## [1.5.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.5.0...v1.5.1) (2020-02-10)


### Bug Fixes

* update stringconfig help message ([8b9e635](https://github.com/GlobalX/PockyBot.NET/commit/8b9e63510d59bedde750cb2d750a240c54a9d893))

# [1.5.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.4.0...v1.5.0) (2020-02-10)


### Bug Fixes

* code style fixes ([a77086b](https://github.com/GlobalX/PockyBot.NET/commit/a77086b3b237871aa29614bb0c2741aa5278e708))
* PR comments for string config ([20ef1cb](https://github.com/GlobalX/PockyBot.NET/commit/20ef1cb314f1ce453614655899082a530d55c6eb))


### Features

* add stringconfig to factory and startup extensions, and add unit tests ([e29cbfb](https://github.com/GlobalX/PockyBot.NET/commit/e29cbfbeaa13ddb282c5c19d029e1516f62ad86e))
* add stringconfig to help and update unit tests ([8bff45f](https://github.com/GlobalX/PockyBot.NET/commit/8bff45f7a005bc3751532b117389e7ee6ef9dcea))
* implement stringconfig add and delete ([d045921](https://github.com/GlobalX/PockyBot.NET/commit/d0459215a12137bfaa363e20744c40bf23029548))
* implement stringconfig get ([e5bb292](https://github.com/GlobalX/PockyBot.NET/commit/e5bb2923463ea28e6df676c76da00c2c58e4232b))
* update stringconfig to display configuration in a list rather than a table ([f772211](https://github.com/GlobalX/PockyBot.NET/commit/f772211fbf66a671018e9c467e42c3692522eb63))

# [1.4.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.3.1...v1.4.0) (2020-01-31)


### Features

* implement rotation trigger and add to help messages ([518f507](https://github.com/GlobalX/PockyBot.NET/commit/518f5073d81df5ffb5868b71e318d1f62051b681))

## [1.3.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.3.0...v1.3.1) (2020-01-31)


### Bug Fixes

* fix default trigger markdown ([02efa83](https://github.com/GlobalX/PockyBot.NET/commit/02efa83373cb8a094736c1ec4c1b09f658309b79))
* update welcome trigger to allow args and add it to the help trigger ([57ed60b](https://github.com/GlobalX/PockyBot.NET/commit/57ed60be82245e3a69abce975b7df38c88d8142c))

# [1.3.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.2.0...v1.3.0) (2020-01-30)


### Features

* add welcome trigger ([6e2df3a](https://github.com/GlobalX/PockyBot.NET/commit/6e2df3af2de28bf0ed299f3f70804731b7cb0912))

# [1.2.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.1.1...v1.2.0) (2020-01-29)


### Bug Fixes

* fix up stripping out command logic ([f1c7fa9](https://github.com/GlobalX/PockyBot.NET/commit/f1c7fa9302ab8defc34212502d85c0b40d708b70))


### Features

* finish implementing help trigger ([90a7a6e](https://github.com/GlobalX/PockyBot.NET/commit/90a7a6e06b1ac11ab5f540cabfa76bf3f233ceac))
* start implementing help trigger ([91ab450](https://github.com/GlobalX/PockyBot.NET/commit/91ab450f55e19d451266efe733ec89b138e06133))

## [1.1.1](https://github.com/GlobalX/PockyBot.NET/compare/v1.1.0...v1.1.1) (2020-01-29)


### Bug Fixes

* make sure objects are logged as json ([382fef7](https://github.com/GlobalX/PockyBot.NET/commit/382fef73ff4f4afcf8c063280bc23fdd2476c5b2))

# [1.1.0](https://github.com/GlobalX/PockyBot.NET/compare/v1.0.4...v1.1.0) (2020-01-29)


### Bug Fixes

* do it properly ([1191caf](https://github.com/GlobalX/PockyBot.NET/commit/1191cafa0806939bb7602164c5cd136ca339ae25))


### Features

* add some fairly high level logging ([a49c349](https://github.com/GlobalX/PockyBot.NET/commit/a49c34945329be044c3eaf8a80a5f94841fb2069))
* rearrange peg log so that comment comes at the end ([8c89e3b](https://github.com/GlobalX/PockyBot.NET/commit/8c89e3b28424c3fa715fe45657b44a925b7ba6a3))

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
