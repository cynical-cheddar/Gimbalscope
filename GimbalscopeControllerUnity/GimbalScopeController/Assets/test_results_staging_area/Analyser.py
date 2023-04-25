import numpy as np
import json
import numpy as np
from scipy.stats import chisquare
from glob import glob
import pandas as pd
from scipy.stats import chi2_contingency
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

def CalculateMeanGuesses(set):
    meanGuesses = 0
    i = 0
    guesses = 0
    for trial in set:
        i += 1
        if "requests" in trial:
            guesses +=trial['requests']
    meanGuesses = guesses / i
    return meanGuesses

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


def GetCueRequestsFromCueType(cuetype, data):
    # loop through completed trials
    # if cueType == cuetype, add to array
    results = []
    for trial in data:
        if(trial['cueType'] == cuetype):
            results.append(trial['requests'])
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

def GetBinaryOutcomeFromTrialSaturation(trial_set, saturation):
    rates = []
    for set in trial_set:
        for trial in set:
            if(trial['motorSaturation'] == saturation):
                if(trial['correct']):
                    rates.append(1)
                else:
                    rates.append(0)
    return rates

def GetCuesRequestedFromTrialSaturation(trial_set, saturation):
    cues = []
    for set in trial_set:
        for trial in set:
            if(trial['motorSaturation'] == saturation):
                cues.append(trial['requests'])
    return cues


def DoChiSquareForThresholdOfDiscriminationTest(trialset, data):
    print("CHI Square over dataset")
    
    list1 = GetBinaryOutcomeFromTrialSaturation(trialset, 5)
    list2 = GetBinaryOutcomeFromTrialSaturation(trialset, 10)
    list3 = GetBinaryOutcomeFromTrialSaturation(trialset, 15)
    list4 = GetBinaryOutcomeFromTrialSaturation(trialset, 20)

    # create a contingency table
    cont_table = np.array([[list1.count(1), list2.count(1), list3.count(1), list4.count(1)],
                        [list1.count(0), list2.count(0), list3.count(0), list4.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    # print the results
    print(f"Chi-squared test statistic for 5, 10, 15, 20: {chi2}")
    print(f"P-value: {pval}")

    # create a contingency table for everything but list 1
    cont_table = np.array([[list2.count(1), list3.count(1), list4.count(1)],
                        [list2.count(0), list3.count(0), list4.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    # print the results
    print(f"Chi-squared test statistic for 10, 15, 20: {chi2}")
    print(f"P-value: {pval}")

    # create a contingency table for everything but list 1
    cont_table = np.array([[list3.count(1), list4.count(1)],
                        [list3.count(0), list4.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    # print the results
    print(f"Chi-squared test statistic for 15, 20: {chi2}")
    print(f"P-value: {pval}")


    # now perform chi squared test across each of the haptic cues
    leftTiltResults = GetHitResultsFromCueType(0, data)
    rightTiltResults = GetHitResultsFromCueType(1, data)
    forwardsTiltResults = GetHitResultsFromCueType(2, data)
    backwardsTiltResults = GetHitResultsFromCueType(3, data)
    leftTwistResults = GetHitResultsFromCueType(4, data)
    rightTwistResults = GetHitResultsFromCueType(5, data)  

     # create a contingency table
    cont_table = np.array([[leftTiltResults.count(1), rightTiltResults.count(1), forwardsTiltResults.count(1), backwardsTiltResults.count(1), leftTwistResults.count(1), rightTwistResults.count(1)],
                        [leftTiltResults.count(0), rightTiltResults.count(0), forwardsTiltResults.count(0), backwardsTiltResults.count(0), leftTwistResults.count(0), rightTwistResults.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    print(f"Chi-squared test statistic for directions {chi2}")
    print(f"P-value: {pval}")

    



    #========== cues

    list1 = GetCuesRequestedFromTrialSaturation(trialset, 5)
    list2 = GetCuesRequestedFromTrialSaturation(trialset, 10)
    list3 = GetCuesRequestedFromTrialSaturation(trialset, 15)
    list4 = GetCuesRequestedFromTrialSaturation(trialset, 20)
    import math
    list1 = [math.ceil(x) for x in list1]
    print(list2)
    print(list3)
    print(list4)


    data_to_plot = [list1, list2, list3, list4]
    means = [np.mean(list1), np.mean(list2), np.mean(list3), np.mean(list4)]
    medians = [np.median(list1), np.median(list2), np.median(list3), np.median(list4)]
    import matplotlib.pyplot as plt


    # Create the boxplot
    plt.violinplot(data_to_plot)
    plt.scatter(1, means[0], c='blue', s = 50, label = "Means")
    plt.scatter(2, means[1], c='blue', s = 50)
    plt.scatter(3, means[2], c='blue', s = 50)
    plt.scatter(4, means[3], c='blue', s = 50)
    plt.scatter(1, medians[0], c='red', marker = '1', s = 300, label = "Medians")
    plt.scatter(2, medians[1], c='red', marker = '1', s = 300)
    plt.scatter(3, medians[2], c='red', marker = '1', s = 300)
    plt.scatter(4, medians[3], c='red', marker = '1', s = 300)
    plt.legend()
    plt.xlabel("Flywheel Saturation / [%]")
    plt.ylabel("Cues Requested")
    plt.show()



    # perform the kruskal-wallace test for statistical independnce
    from scipy.stats import kruskal

    stat, p_value = kruskal(list1, list2, list3, list4)

    # Print results
    print("Kruskal-Wallis test statistic:", stat)
    print("p-value:", p_value)



def DoChiSquareForTorquePerceptionTest(trialset, data):
    print("CHI Square over dataset")
    
    list1 = GetBinaryOutcomeFromTrialSaturation(trialset, 20)
    list2 = GetBinaryOutcomeFromTrialSaturation(trialset, 30)
    list3 = GetBinaryOutcomeFromTrialSaturation(trialset, 40)

    # create a contingency table
    cont_table = np.array([[list1.count(1), list2.count(1), list3.count(1)],
                        [list1.count(0), list2.count(0), list3.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    # print the results
    print(f"Chi-squared test statistic for 20, 30, 40: {chi2}")
    print(f"P-value: {pval}")

    # create a contingency table 
    cont_table = np.array([[list1.count(1), list2.count(1), list3.count(1)],
                        [list1.count(0), list2.count(0), list3.count(0)]])


    # now perform chi squared test across each of the haptic cues
    leftTiltResults = GetHitResultsFromCueType(0, data)
    rightTiltResults = GetHitResultsFromCueType(1, data)
    forwardsTiltResults = GetHitResultsFromCueType(2, data)
    backwardsTiltResults = GetHitResultsFromCueType(3, data)
    leftTwistResults = GetHitResultsFromCueType(4, data)
    rightTwistResults = GetHitResultsFromCueType(5, data)  

     # create a contingency table
    cont_table = np.array([[leftTiltResults.count(1), rightTiltResults.count(1), forwardsTiltResults.count(1), backwardsTiltResults.count(1), leftTwistResults.count(1), rightTwistResults.count(1)],
                        [leftTiltResults.count(0), rightTiltResults.count(0), forwardsTiltResults.count(0), backwardsTiltResults.count(0), leftTwistResults.count(0), rightTwistResults.count(0)]])

    # perform the chi-squared test
    chi2, pval, dof, expected = chi2_contingency(cont_table)

    print(f"Chi-squared test statistic for directions {chi2}")
    print(f"P-value: {pval}")







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
        if(trial['motorSaturation'] > 2):
            motorSaturations.append(trial['motorSaturation'])

    motorSaturations = list(dict.fromkeys(motorSaturations))
    
    motorSaturations.sort()
    # for each unique motor saturations, add a record
    for saturation in motorSaturations:
        trialset.append([])
    
   # print (trialset)
    
    # now sort trial data in to each of the motor saturation
    for trial in data:
        saturation = trial['motorSaturation']
        index = GetMotorSaturationIndex(motorSaturations=motorSaturations, saturation=saturation)
        trialset[index].append(trial)

    print("TRIAL SET")
    i = 0
    # iterate through all trials sorted by motor saturation
    allTrialsCount = 0
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
        print("MEAN GUESSES")
        print(CalculateMeanGuesses(set))
        # https://www.medcalc.org/calc/comparison_of_proportions.php
        print("-")

        outcomes = GetBinaryOutcomeFromTrialSaturation(trialset, motorSaturation)
        print(outcomes)
        print("=====")
        #print(set)
        i += 1
        allTrialsCount += len(set)

    print("TOTAL TRIALS")
    print(allTrialsCount)
    # chi square over dataset
    print("Expected correct guess chances recap:")
    successRates = GetSuccessRatesFromTrialSet(trial_set=trialset)
    print(successRates)
    meanRate = GetExpectedSuccessRate(trial_set=trialset)

    print("Overall mean")
    print(meanRate)


    

    DoChiSquareForThresholdOfDiscriminationTest(trialset, data)
    #DoChiSquareForTorquePerceptionTest(trialset, data)
    





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

