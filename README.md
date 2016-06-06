Explain GameDataProcessor Coordinate
#Warning!
From unity view and for GameDataProcessor:
|----------------> x+
|
|
|
Y+


<<<<<<< HEAD
#Game Data Encoding

#Basic


Empty:         0

Player:        1  (Different types can be 1xx)

Enermy:        2  (Different types can be 2xx)

Normal Cube:   3  (Different types can be 3xx)

Wall Cube:     4  (Different types can be 4xx)

Bomb:          5  (Different types can be 5xx)

Fire:          6

Combined:      9xx


#Combined


Player&Enermy:        921

Bomb&Player:          951

Bomb&Enermy:          952

Fire on player:       961

Fire on enermy:       962

Fire on normal Cube:  963

and more...


#Note: no need to change map data. Game data and map data can be implemented independently.
=======
# Bomber-GameDesign



#Loader
Instantiate all global managers (singleton) in its Awake function. Now there is only a RhythmRecorder. Loader is extendable to more 'manegers'.

#How to Use Loader
Add a member variable for each manager. Drag the prefab of that manager onto the varible.


#RhythmRecorder

There are two ways to utilize RhythmRecorder, fixed and non-fixed.

Fixed: Used in cases where automatic call at beats is desirable. For example, the flash of the floor.

How to use RhythmRecorder in fixed way: 

1. implements RhythmObservable interface

2. add self to observed subjects.

3. implements actionOnBeats() method. This will be called every beats.


Non-fixed: Used to check whether some action is on beats. For example, the movement of player.

How to use RhythmRecorder in non-fixed way: 

1. use isOnBeat() to check.

Methods in RhythmRecorder:

setRhythm()

startRhythm()

resetRhythm()

#RhythmList
All rhythm path should be registered in this class as static string. They will act as input to setRhythm() method.

#For more details, please refer to "Test Files" in the project.
>>>>>>> master

