# CustomBreachScenarios
![img](https://img.shields.io/github/downloads/Ceglaa/CustomBreachScenarios/total?style=for-the-badge)

Custom breach scenario will be drawed from all scenarios in **EXILED/Configs/CustomBreachScenarios** path

Scenario contains:
- MapEditorReborn map loading integration
- Chance for scenario to occur
- Custom round conditions
- Delayed SCP spawns
- Timed cassies
- Timed door lockdowns
- Timed blackouts
- Opened doors on round start
- Color changes for entire zones

Example scemario:
```yaml
name: example
# Map name for MapEditorReborn plugin
map_name: 
chance: 0
auto_nuke:
# Warhead starts automaticly after delay. If set to 0 it will not start
  delay: 1800
  time: 90
  chance: 100
custom_conditions:
  can_ntf_spawn: true
  can_chi_spawn: true
cassies:
- delay: 20
  is_noisy: true
  announcement: test
delayed_s_c_p_spawns:
- delay: 120
  role: Scp096
  room: Hcz096
door_lockdowns:
- time: 120
  chance: 50
  door_type: GateA
  door_lock_type: SpecialDoorFeature
blackouts:
- delay: 100
  time: 100
  chance: 0
  zones:
  - Entrance
  - LightContainment
opened_doors:
  HeavyContainmentDoor: 50
zone_colors:
  HeavyContainment:
    r: 1
    g: 0
    b: 0
    a: 1
```

Example empty scenario:
```yaml
name: emptyexample
# Map name for MapEditorReborn plugin
map_name: 
chance: 0
auto_nuke:
# Warhead starts automaticly after delay. If set to 0 it will not start
  delay: 0
  time: 90
  chance: 0
custom_conditions:
  can_ntf_spawn: true
  can_chi_spawn: true
cassies: []
delayed_s_c_p_spawns: []
door_lockdowns: []
blackouts: []
opened_doors: {}
zone_colors: {}
```
