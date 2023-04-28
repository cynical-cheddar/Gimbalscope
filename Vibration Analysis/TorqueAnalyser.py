import numpy as np
from scipy.fft import fft, rfft
from scipy.fft import fftfreq, rfftfreq
import plotly.graph_objs as go
from plotly.subplots import make_subplots
import matplotlib.pyplot as plt
import math
filename = "l_tilt.txt"




def DisplayTorqueGraph(filename, title, timeMultiplier, uppercut, lowercut):
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






    time_values = []
    i = 0
    for x in xs:
        time_values.append(i * timeMultiplier)
        i += 1
    
    plt.plot( time_values, xs, color = 'red', label = 'x')
    plt.plot( time_values, ys, color = 'green', label = 'y')
    plt.plot( time_values, zs, color = 'blue', label = 'z')
    plt.title(title)
    plt.legend()
    plt.xlabel("Time [ms]")
    plt.ylabel("Rotational Velocity [rads / s]")

    plt.show()


DisplayTorqueGraph("l_tilt.txt", "Left Tilt About Roll Axis", 2, 1000, 250)
DisplayTorqueGraph("r_tilt.txt", "Right Tilt About Roll Axis", 2, 1000, 250)

DisplayTorqueGraph("f_tilt.txt", "Forward Tilt About Pitch Axis", 2, 1000, 250)
DisplayTorqueGraph("b_tilt.txt", "Backward Tilt About Pitch Axis", 2, 1000, 250)

DisplayTorqueGraph("l_twist.txt", "Left Twist About Yaw Axis", 1.8, 1400, 500)
DisplayTorqueGraph("r_twist.txt", "Right Twist About Yaw Axis", 2.5, 1200, 600)

