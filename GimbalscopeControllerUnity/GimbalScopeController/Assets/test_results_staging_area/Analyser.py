import numpy as np
import json
 
##
##  cueType
# 0       leftTilt,
# 1       rightTilt,
# 2       forwardsTilt,
# 3       backwardsTilt,
# 4       leftTwist,
# 5       rightTwist
##
trialset = []

def GetMotorSaturationIndex(motorSaturations, saturation):
    index = 0
    for motorSaturation in motorSaturations:
        if(motorSaturation == saturation):
            return index
        index += 1
    return -1



# combine all json files in the pwd (without the name mega) into the mega json:


# open the mega JSON file
with open('mega.json') as json_file:
    wrapped_data = json.load(json_file)
 

    data = wrapped_data["completedTrials"]
    motorSaturations = []
    for trial in data:
        if(trial['motorSaturation'] > 5):
            motorSaturations.append(trial['motorSaturation'])

    motorSaturations = list(dict.fromkeys(motorSaturations))
    
    motorSaturations.sort()
    # for each unique motor saturations, add a record
    for saturation in motorSaturations:
        trialset.append([])
    
    print (trialset)
    
    # now sort trial data in to each of the motor saturation
    for trial in data:
        saturation = trial['motorSaturation']
        index = GetMotorSaturationIndex(motorSaturations=motorSaturations, saturation=saturation)
        trialset[index].append(trial)

    print("TRIAL SET")
    i = 0
    # iterate through all trials sorted by motor saturation
    for set in trialset:
        print("=====")
        motorSaturation = motorSaturations[i]
        print(motorSaturation)
        # print hitrate of trial set 

        print("=====")
        print(set)
        i += 1