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
