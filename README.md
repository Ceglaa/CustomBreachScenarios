# **CustomBreachScenarios**
[![Downloads](https://img.shields.io/github/downloads/Ceglaa/CustomBreachScenarios/total?style=for-the-badge)](https://github.com/Ceglaa/CustomBreachScenarios/releases)
![Last Commit](https://img.shields.io/github/last-commit/Ceglaa/CustomBreachScenarios?style=for-the-badge)
[![Last Release](https://img.shields.io/github/v/release/Ceglaa/CustomBreachScenarios?style=for-the-badge)](https://github.com/Ceglaa/CustomBreachScenarios/releases)<br>
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Exiled-Team/Exiled?label=EXILED&style=for-the-badge)](https://github.com/Exiled-Team/EXILED/releases)

### Custom breach scenario will be drawed from all scenarios in **EXILED/Configs/CustomBreachScenarios** path

#### Scenario contains:
- Commands executed at the start of the scenario
- Chance for scenario to occur
- Custom round conditions
- Delayed SCP spawns
- Timed cassies
- Timed door lockdowns
- Timed blackouts
- Opened doors on round start
- Color changes for entire zones

### 🔧 Plans for future: 🔧
- [ ] Custom Team Respawn Queue
- [ ] Delayed commands
- [ ] Scenario drawing optimizations

### Examples:

<details>
  <summary>Example scenario</summary>
  
```yaml
name: example
chance: 0
commands:
- '/mp l mapname'
- '/bc 10 test boradcast'
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
zone_colors:
- delay: 10
  time: 70
  zone_type: LightContainment
  r: 1
  g: 0
  b: 0
  a: 0
opened_doors:
  HeavyContainmentDoor: 50
```
</details>

<details>
  <summary>Example empty scenario</summary>
  
```yaml
name: emptyexample
chance: 0
commands: []
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
</details>
