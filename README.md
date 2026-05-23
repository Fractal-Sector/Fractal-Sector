<div class="header" align="center">
<img alt="Wayfarer Station" height="300" src="Resources/Textures/_Orehum/Logo/logo.png?raw=true" />
</div>

**Orehum Sector** это репозиторий англоязычного фронтира [Frontier Station](https://github.com/new-frontiers-14/frontier-station-14) и оригинального билда [Space Station 14](https://github.com/space-wizards/space-station-14), которые работают на движке [Robust Toolbox](https://github.com/space-wizards/RobustToolbox) от Space Wizards написанном на C#.


Это основной репозиторий проекта Orehum Sector

Если вы хотите создавать или размещать контент для Orehum Sector, вам нужен именно этот репозиторий. Он содержит как RobustToolbox, так и набор контента для разработки нового контента.

## Ссылки

#### Orehum Sector
<div class="header" align="center">

[Github](https://github.com/Orehum-Project/Orehum-Sector/) | [Discord](https://discord.gg/ZC94VrbFNY)
</div>


#### Frontier Station
<div class="header" align="center">

[Discord](https://discord.com/invite/frontier) | [Patreon](https://www.patreon.com/frontierstation14) | [Wiki](https://frontierstation.wiki.gg/)

</div>

#### Space Station 14
<div class="header" align="center">

[Website](https://spacestation14.io/) | [Discord](https://discord.ss14.io/) | [Forum](https://forum.spacestation14.io/) | [Steam](https://store.steampowered.com/app/1255460/Space_Station_14/) | [Standalone Download](https://spacestation14.io/about/nightlies/)

</div>

## Вклад

Мы рады любой помощи и вкладу в проект от каждого желающего. Если вы хотите помочь, заходите в наш [Discord-сервер](https://discord.gg/ZC94VrbFNY).
Хотя соблюдение [правил контрибьюта Space Station 14](https://docs.spacestation14.com/en/general-development/codebase-info/pull-request-guidelines.html) не является строго обязательным для Orehum Sector, мы рекомендуем ознакомиться с ними, чтобы придерживаться лучших практик разработки.

## Сборка

Следуйте [гайду от Space Wizards](https://docs.spacestation14.com/en/general-development/setup/setting-up-a-development-environment.html) по настройке рабочей среды, но учитывайте, что наши репозитории отличаются и некоторые вещи могут отличаться.
Мы предлагаем несколько скриптов, показанных ниже, чтобы облегчить работу.

### Необходимые зависимости

> - Git
> - .NET SDK 10.0.101

### Windows

> 1. Склонируйте данный репозиторий
> 2. Запустите `git submodule update --init --recursive` в командной строке, чтобы скачать движок игры
> 3. Запускайте `Scripts/bat/buildAllDebug.bat` после любых изменений в коде проекта
> 4. Запустите `Scripts/bat/runQuickAll.bat`, чтобы запустить клиент и сервер
> 5. Подключитесь к локальному серверу и играйте

### Linux

> 1. Склонируйте данный репозиторий.
> 2. Запустите `git submodule update --init --recursive` в командной строке, чтобы скачать движок игры
> 3. Запускайте `Scripts/sh/buildAllDebug.sh` после любых изменений в коде проекта
> 4. Запустите `Scripts/sh/runQuickAll.sh`, чтобы запустить клиент и сервер
> 5. Подключитесь к локальному серверу и играйте

### MacOS

> Предположительно, также, как и на Линуксе.

## Лицензия
Для получения подробной информации о лицензировании внимательно прочтите файл [LEGAL.md](docs/LEGAL.md)


