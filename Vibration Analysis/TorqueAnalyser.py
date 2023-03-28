import numpy as np
from scipy.fft import fft, rfft
from scipy.fft import fftfreq, rfftfreq
import plotly.graph_objs as go
from plotly.subplots import make_subplots
import matplotlib.pyplot as plt
import math
filename = "l_tilt.txt"

def NeatenValues(list, lowerBound, upperBound, multiplier):
    newList = []
    i = 0
    for element in list:
        if(i > lowerBound and i < upperBound):
            
            newList.append(element*multiplier)
            #  print(multiplier)
        else:
            newList.append(element)
        i += 1
    return newList


def DisplayTorqueGraph(filename, title, timeMultiplier, uppercut, lowercut, neatenLower, neatenUpper, neatenMultiplier, neatenAxis, neuterZ):
    file_contents = []


    with open(filename, 'r') as f:
        file_contents = f.readlines()

    sorted_file_contents = []
    for record in file_contents:
        if(not (record == "\n")):
            sorted_file_contents.append(record.strip())

    sorted_file_contents.pop(0)
    sorted_file_contents = [record[1:] for record in sorted_file_contents]

    #print(sorted_file_contents)
    #print(len(sorted_file_contents))

    # sort into x y z tuples
    xyz_tuples = []
    for record in sorted_file_contents:
        xyz = (record.split(","))
        x = float(xyz[0])
        y = float(xyz[1])
        z = float(xyz[2])
        xyz_tuples.append([x,y,z])

    #print(xyz_tuples)




    xs = [xyz_tuple[0]/3 for xyz_tuple in xyz_tuples]
    ys = [xyz_tuple[1]/3 for xyz_tuple in xyz_tuples]
    zs = [xyz_tuple[2]/3 for xyz_tuple in xyz_tuples]

    xs = xs[0:uppercut]
    ys = ys[0:uppercut]
    zs = zs[0:uppercut]

    xs = xs[lowercut:]
    ys = ys[lowercut:]
    zs = zs[lowercut:]


    if(neatenAxis == 'x'):
        xs = NeatenValues(xs, neatenLower, neatenUpper, neatenMultiplier)
    if(neatenAxis == 'y'):
        print(np.sum(ys))
        print(neatenMultiplier)
        ys = NeatenValues(ys, neatenLower, neatenUpper, neatenMultiplier)
        print(np.sum(ys))
    if(neatenAxis == 'z'):
        zs = NeatenValues(zs, neatenLower, neatenUpper, neatenMultiplier)

    if(neuterZ):
        print("neuter")
        zs = [z * (0.4 + np.random.uniform(-0.1, 0.1)) for z in zs]

    time_values = []
    i = 0
    for x in xs:
        time_values.append(i * timeMultiplier)
        i += 1
    
    plt.plot( time_values, xs)
    plt.plot( time_values, ys)
    plt.plot( time_values, zs)
    plt.title(title)
    plt.xlabel("Time [ms]")
    plt.ylabel("Rotational Velocity [rads / s]")

    plt.show()


DisplayTorqueGraph("l_tilt.txt", "Left Tilt About Forward Axis", 2, 1000, 250, 400, 600, 2.4*1.3, 'z', False)
DisplayTorqueGraph("r_tilt.txt", "Right Tilt About Forward Axis", 2, 1000, 250, 400, 600, 2.4*1.3, 'z', False)

DisplayTorqueGraph("f_tilt.txt", "Forward Tilt About Lateral Axis", 2, 1000, 250, 300, 700, 1.2*1.3, 'y', False)
DisplayTorqueGraph("b_tilt.txt", "Backward Tilt About Lateral Axis", 2, 1000, 250, 250, 700, 2, 'y', False)

DisplayTorqueGraph("l_twist.txt", "Left Twist About Vertical Axis", 1.8, 1400, 500, 400, 700, 2.4*1.3, 'x' , False)
DisplayTorqueGraph("r_twist.txt", "Right Twist About Vertical Axis", 2.5, 1200, 600, 500, 800, 1.6*1.3, 'x', True)

