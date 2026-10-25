THEME: OLD TECH

Design Doc: https://docs.google.com/document/d/1CNTr4www0_wEoLcrbZV5NYowFTrmt4szc_PByVHaFmk/edit?tab=t.0

level ideas:
    - vertical level where you are dodging sparks while falling
To Do:
- Thumb Boss
- Squash bugs
- More levels
- Level based goals
- Level transition
- Pistons should squish player? + Probably not that easy to do
- A more clear down state for buttons
- Tutorial signage
- player death animation - Quinn
- menu/level select
- audio level controls
- puzzles w/ actual mechanics
    - toggle piston puzzle
        - multiple pistons linked to shared buttons, and then they all have to be down or sum shit
    - Timed parkour 
    - Circuit completion?
    - turn on all lights
- fast pistons boost you?
- Add wait time and stagger to buttons and pistons - Emma
- improve piston button pushed when it currently moving - Emma
- more button colours/wires
- Solve FMOD command buffer issues - Julian:
    - cull out of distance instances in FMOD
    - or have them load in only when within distance
- indicators of where to go/how to solve levels (like with jumps and shit) - Art Quinn
    - signs that point in the direction, or suggest what move should be taken
- Implement skinny wires(can't stand on them) and more coloured wires
- Timer as a visual signal that something will take a long time to activate
- Slopes -Kent

- portals! (toggleable)
- Decrease flash intensity: 


- Time delay buttons
    - (or redstone repeater?)
- Connection switcher (switching what's connected to what)
    - Button and fan are connected, but you can hit a switch on the wire to connect it to piston instead
- multiplier (effects the power/speed of pistons/fans)


- (somewhat) historically accurate mechanics


emma to-do:
    - improve checkpoint sprite code



done:
    - Art for skinny wires and standing wires
    - Dash Trail
    - Better spark effect
    - fan trigger only on player
    - add fan sprite 
        - auto-tile palette from new floor tilesheet
    - animate fan
    - add wire platforms
    - allow for "built-in" fans which can also be stood on
        - auto-tile wires: 
        - totally safe
        - danger ends
        - totally unsafe (ish)
    - fan animation stop/start
    - add button for fans
    - button switches fan state
    - update button sprite 
    - fan blows player up (ty kent)
    - button turn on/off fan
    - piston movement (initial)
    - fix upside piston movement
    - piston button connection
    - link sparks to buttons
    - add background
    - add timer to sparks
    - update button sprite on button push (ty julian)
    - game design
        - add big red button
        - add secondary area
        - add win condition
