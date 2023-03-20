import numpy as np
import json
import numpy as np
from scipy.stats import chisquare
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

def CalculateSetHitRate(set):
    hitrate = 0
    i = 0
    hits = 0
    for trial in set:
        i += 1
        if(trial['correct']):
            hits += 1
    hitrate = hits / i
    return hitrate

def SetToHitArray(set):
    hitArray = []
    for trial in set:
        if(trial['correct']):
            hitArray.append(1)
        else:
            hitArray.append(0)
    return hitArray

def GetSuccessRatesFromTrialSet(trial_set):
    rates = []
    for set in trial_set:
        rates.append(CalculateSetHitRate(set))
    return rates

def GetExpectedSuccessRate(trial_set):
    rates = GetSuccessRatesFromTrialSet(trial_set)
    return np.mean(rates)

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
        print("MOTOR SATURATION")
        print(motorSaturation)
        print("SET SIZE")
        print(len(set))
        print("HITRATE")
        # print hitrate of trial set 
        print(CalculateSetHitRate(set))
        
        # https://www.medcalc.org/calc/comparison_of_proportions.php
        print("=====")
        #print(set)
        i += 1

    # chi square over dataset
    successRates = GetSuccessRatesFromTrialSet(trial_set=trialset)
    print(successRates)
    meanRate = GetExpectedSuccessRate(trial_set=trialset)

    
    print(meanRate)
    val, p = chisquare(successRates, meanRate)
    print("CHI Square over dataset")
    print(val, p)

    