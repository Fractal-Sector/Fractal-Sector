### Examine

gas-turbine-examine-stator-null = Похоже, отсутствует статор.
gas-turbine-examine-stator = Статор на месте.

gas-turbine-examine-blade-null = Похоже, отсутствует лопасть турбины.
gas-turbine-examine-blade = Лопасть турбины на месте.

turbine-spinning-0 = Лопасти не вращаются.
turbine-spinning-1 = Лопасти медленно вращаются.
turbine-spinning-2 = Лопасти вращаются.
turbine-spinning-3 = Лопасти вращаются быстро.
turbine-spinning-4 = [color=red]Лопасти вращаются с неконтролируемой скоростью![/color]

turbine-damaged-0 = Похоже, турбина в хорошем состоянии.
turbine-damaged-1 = Турбина выглядит немного поцарапанной.
turbine-damaged-2 = [color=yellow]Турбина выглядит сильно поврежденной.[/color]
turbine-damaged-3 = [color=orange]Она критически повреждена![/color]

turbine-ruined = [color=red]Она полностью сломана![/color]

### Popups

# Shown when an event occurs
turbine-overheat = {$owner} активирует аварийный клапан сброса перегрева!
turbine-explode = {$owner} разрывает себя на части!

# Shown when damage occurs
turbine-spark = {$owner} начинает искрить!
turbine-spark-stop = {$owner} перестала искрить.
turbine-smoke = {$owner} начинает дымиться!
turbine-smoke-stop = {$owner} перестала дымиться.

# Shown during repairs
gas-turbine-repair-fail-blade = Вам необходимо заменить лопасть турбины, прежде чем можно будет произвести ремонт.
gas-turbine-repair-fail-stator = Вам необходимо заменить статор, прежде чем можно будет произвести ремонт.
turbine-repair-ruined = Вы чините корпус {$target} с помощью {$tool}.
turbine-repair = Вы устраняете часть повреждений {$target} с помощью {$tool}.
turbine-no-damage = {$target} не имеет повреждений, которые можно устранить с помощью {$tool}.
turbine-show-damage = Прочность лопасти: {$health}/{$healthMax}.

# Anchoring warnings
turbine-unanchor-warning = Вы не можете отсоединить газовую турбину, пока она вращается!
turbine-anchor-warning = Недопустимая позиция для закрепления.

gas-turbine-eject-fail-speed = Вы не можете извлечь детали турбины, пока она вращается!
gas-turbine-insert-fail-speed = Вы не можете установить детали турбины, пока она вращается!

### UI

# Shown when using the UI
comp-turbine-ui-tab-main = Управление
comp-turbine-ui-tab-parts = Детали

comp-turbine-ui-rpm = RPM

comp-turbine-ui-overspeed = ПРЕВЫШЕНИЕ СКОРОСТИ
comp-turbine-ui-overtemp = ПЕРЕГРЕВ
comp-turbine-ui-stalling = ПОМПАЖ (СТАЛЛИНГ)
comp-turbine-ui-undertemp = НЕДОСТАТОЧНАЯ ТЕМПЕРАТУРА

comp-turbine-ui-flow-rate = Скорость потока
comp-turbine-ui-stator-load = Нагрузка на статор

comp-turbine-ui-blade = Лопасть турбины
comp-turbine-ui-blade-integrity = Целостность
comp-turbine-ui-blade-stress = Напряжение

comp-turbine-ui-stator = Статор турбины
comp-turbine-ui-stator-potential = Потенциал
comp-turbine-ui-stator-supply = Питание

comp-turbine-ui-power = { POWERWATTS($power) }

comp-turbine-ui-locked-message = Управление заблокировано.
comp-turbine-ui-footer-left = Опасно: быстродвижущиеся механизмы.
comp-turbine-ui-footer-right = 2.0 REV 1