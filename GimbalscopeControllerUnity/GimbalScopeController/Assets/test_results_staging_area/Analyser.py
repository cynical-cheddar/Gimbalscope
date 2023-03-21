import numpy as np
import json
import numpy as np
from scipy.stats import chisquare
from glob import glob
import pandas as pd
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

def GetHitResultsFromCueType(cuetype, data):
    # loop through completed trials
    # if cueType == cuetype, add to array
    results = []
    for trial in data:
        if(trial['cueType'] == cuetype):
            if(trial['correct'] == True):
                results.append(1)
            else:
                results.append(0)
    return results

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
def FlattenList(l):
    flat_list = []
    for sublist in l:
        for item in sublist:
            flat_list.append(item)
    return flat_list
def merge_JsonFiles_to_mega(filenames):
    # get completed trials from each
    contents = ['{"completedTrials":']
    for filename in filenames:
        with open(filename) as json_file:
            wrapped_data = json.load(json_file)
            data = wrapped_data["completedTrials"]
            contents.append(data)
    contents.append('}')
    joined_contents = "".join([str(item) for item in contents])
    joined_contents = joined_contents.replace("'", '"' )
    joined_contents = joined_contents.replace("True", 'true' )
    joined_contents = joined_contents.replace("False", 'false' )
    joined_contents = joined_contents.replace(" ", '' )
    joined_contents = joined_contents.replace("][", "," )
    print(joined_contents)
    f = open("mega.json", "w")
    f.write(joined_contents)
    f.close()


# combine all json files in the pwd (without the name mega) into the mega json:
f_names = []
for f_name in glob('*.json'):
    if not (f_name == "mega.json"):
        f_names.append(f_name)

merge_JsonFiles_to_mega(f_names)

# open the mega JSON file
with open('mega.json') as json_file:
    wrapped_data = json.load(json_file)
    print(wrapped_data)
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

    # now get the success rates for each of the cue types
                           ##  cueType
    # 0       leftTilt,
    leftTiltResults = GetHitResultsFromCueType(0, data)
    print("leftTiltResults")
    print(leftTiltResults)
    print(np.mean(leftTiltResults))
    # 1       rightTilt,
    print("rightTiltResults")
    rightTiltResults = GetHitResultsFromCueType(1, data)
    print(rightTiltResults)
    print(np.mean(rightTiltResults))
    # 2       forwardsTilt,
    print("forwardsTiltResults")
    forwardsTiltResults = GetHitResultsFromCueType(2, data)
    print(forwardsTiltResults)
    print(np.mean(forwardsTiltResults))
    # 3       backwardsTilt,
    print("backwardsTiltResults")
    backwardsTiltResults = GetHitResultsFromCueType(3, data)
    print(backwardsTiltResults)
    print(np.mean(backwardsTiltResults))
    # 4       leftTwist,
    print("leftTwistResults")
    leftTwistResults = GetHitResultsFromCueType(4, data)
    print(leftTwistResults)
    print(np.mean(leftTwistResults))
    # 5       rightTwist    
    print("rightTwistResults")
    rightTwistResults = GetHitResultsFromCueType(5, data)
    print(rightTwistResults)
    print(np.mean(rightTwistResults))                   

