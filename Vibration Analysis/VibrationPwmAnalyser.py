import numpy as np
from scipy.fft import fft, rfft
from scipy.fft import fftfreq, rfftfreq
import plotly.graph_objs as go
from plotly.subplots import make_subplots
import matplotlib.pyplot as plt
import math

mult = 5
mini_mult = 1
def GetVibrationsFromFile(filename):
    file_contents = []
    with open(filename, 'r') as f:
        file_contents = f.readlines()

    sorted_file_contents = []
    for record in file_contents:
        if(not (record == "\n")):
            sorted_file_contents.append(record.strip())

    sorted_file_contents = sorted_file_contents[-1000:]
  #  print(sorted_file_contents)
    def Extract_Var_and_Time(vibrations_list, var_label):
        timestamp_var_pairs = []
        for record in vibrations_list:
            items = record.split(",")
            pair = []
            for item in items:
                
                if(item[0] == var_label or item[0] == 't'):
                    var_and_value = item.split(":")
                    var = var_and_value[1]
                    pair.append(var)
            if(len(pair) == 2):
                timestamp_var_pairs.append(pair)
        return timestamp_var_pairs
                
                

    sample_rate = 900
    # now extract only the Xs
    x_and_time = Extract_Var_and_Time(sorted_file_contents, 'x')
    y_and_time = Extract_Var_and_Time(sorted_file_contents, 'y')
    z_and_time = Extract_Var_and_Time(sorted_file_contents, 'z')

    x_values = [(float(x[0]) * 9.81 *mult) / 4096 for x in x_and_time]
    y_values = [(float(x[0]) * 9.81*mult) / 4096 for x in y_and_time]
    z_values = [(float(x[0]) * 9.81*mult) / 4096 for x in z_and_time]

    return(x_values, y_values, z_values, z_and_time)


def GetMeanVibrationsFromFile(filename):
    file_contents = []
    with open(filename, 'r') as f:
        file_contents = f.readlines()

    sorted_file_contents = []
    for record in file_contents:
        if(not (record == "\n")):
            sorted_file_contents.append(record.strip())

    sorted_file_contents = sorted_file_contents[len(sorted_file_contents)//2:]

    def Extract_Var_and_Time(vibrations_list, var_label):
        timestamp_var_pairs = []
        for record in vibrations_list:
            items = record.split(",")
            pair = []
            for item in items:
                
                if(item[0] == var_label or item[0] == 't'):
                    var_and_value = item.split(":")
                    var = var_and_value[1]
                    pair.append(var)
            if(len(pair) == 2):
                timestamp_var_pairs.append(pair)
        return timestamp_var_pairs
                
                

    sample_rate = 900
    # now extract only the Xs
    x_and_time = Extract_Var_and_Time(sorted_file_contents, 'x')
    y_and_time = Extract_Var_and_Time(sorted_file_contents, 'y')
    z_and_time = Extract_Var_and_Time(sorted_file_contents, 'z')
    
    x_values = [(float(x[0]) * 9.81*mult ) / 4096 for x in x_and_time]
    y_values = [(float(x[0]) * 9.81*mult) / 4096 for x in y_and_time]
    z_values = [(float(x[0]) * 9.81*mult) / 4096 for x in z_and_time]

    # get abs average values
    x_values_abs = [abs(x_value) for x_value in x_values]
    y_values_abs = [abs(y_value) for y_value in y_values]
    z_values_abs = [abs(z_value) for z_value in z_values]
    
    x_mean = np.mean(x_values_abs)
    y_mean = np.mean(y_values_abs)
    z_mean = np.mean(z_values_abs)
    print(x_mean)
    print(y_mean)
    print(z_mean)

    words = filename.split(".")
    name = words[0]
    name = float(name)

    saturation = name * 1
    print(saturation)

    return ([x_mean, y_mean, z_mean],saturation)

fileNameRangeBottom = 0

fileNameRangeTop = 100

fileNameRangeIncrement = 2
i = fileNameRangeBottom

filenames = []

while (i < fileNameRangeTop +fileNameRangeIncrement ):
    name = str(i) + ".txt"
    filenames.append(name)
    i += fileNameRangeIncrement
print(filenames)

#filenames = ["0.txt", "2.txt", "4.txt", "6.txt", "8.txt", "10.txt", "12.txt", "14.txt", "16.txt", "18.txt", "20.txt"]



mean_xyz_list = []
saturations = []

xyz_sets = []

tick_locs = []
tick_sats = []


for filename in filenames:
    xyz_sets.append(GetVibrationsFromFile(filename))
    means, saturation = GetMeanVibrationsFromFile(filename)
    saturations.append(saturation)
    mean_xyz_list.append(means)
# remove saturation duplicates
saturations = list(dict.fromkeys(saturations))
print(saturations)
i = 0
every_n = 5
for sat in saturations:
    if(i % every_n == 0):
        tick_locs.append(i * 1000)
        tick_sats.append(sat)
    i += 1

    



def flatten(l):
    return [item for sublist in l for item in sublist]
# xyz_sets
# xs 
xs = []
ys = []
zs = []
j = 0
for xyz_set in xyz_sets:
    xs.append(xyz_set[0])
    ys.append(xyz_set[1])
    zs.append(xyz_set[2])
    j += 2
    print(j)
print("len xs")

xs = flatten(xs)
print (len(xs))
print("len ys")

ys = flatten(ys)
print (len(ys))
print("len zs")

zs = flatten(zs)
print (len(zs))



plt.xticks(tick_locs, tick_sats)
plt.plot(xs, linewidth = 0.05, alpha =1, color = 'red', label = 'x')
plt.plot(ys, linewidth = 0.05 , alpha = 0.5, color = 'green', label = 'y')
plt.plot(zs, linewidth = 0.05 , alpha = 0.3, color = 'blue', label = 'z')

plt.ylabel('Acceleration in axis [m/s^2]')
plt.xlabel('Brushless Motor Saturation [%]')
plt.title("Acceleration as a Measure of Vibration Sampled Discretely over Full Motor Saturation Range")
plt.legend()
plt.show()

# now we have this, calculate the mean absolute acceleration over saturation with error bars of max and min
# there are 1000 samples per 2% saturation


# convert set to absolute values
# for each saturation
for i in range(len(xyz_sets)):
    
    # this is a set of  [x_values, y_values, z_values, z_and_time]
    #(xyz_sets[i])

    # for each x value
    for j in range(len(xyz_sets[i][0])):

        xyz_sets[i][0][j] = abs(xyz_sets[i][0][j]) * mini_mult
    # for each y value
    for j in range(len(xyz_sets[i][1])):

        xyz_sets[i][1][j] = abs(xyz_sets[i][1][j]) * mini_mult
    # for each z value
    for j in range(len(xyz_sets[i][2])):

        xyz_sets[i][2][j] = abs(xyz_sets[i][2][j]) * mini_mult

xs = []
ys = []
zs = []

for xyz_set in xyz_sets:
    xs.append(xyz_set[0] )
    ys.append(xyz_set[1] )
    zs.append(xyz_set[2] )

mean_absolute_vibrations_x = []
min_absolute_vibrations_x = []
max_absolute_vibrations_x = []

mean_absolute_vibrations_y = []
min_absolute_vibrations_y = []
max_absolute_vibrations_y = []

mean_absolute_vibrations_z = []
min_absolute_vibrations_z = []
max_absolute_vibrations_z = []


for x_set in xs:
    mean_absolute_vibrations_x.append(np.mean(x_set))
    min_absolute_vibrations_x.append(np.percentile(x_set, 25))
    max_absolute_vibrations_x.append(np.percentile(x_set, 75))

for y_set in ys:
    mean_absolute_vibrations_y.append(np.mean(y_set))
    min_absolute_vibrations_y.append(np.percentile(y_set, 25))
    max_absolute_vibrations_y.append(np.percentile(y_set, 75))

for z_set in zs:
    mean_absolute_vibrations_z.append(np.mean(z_set))
    min_absolute_vibrations_z.append(np.percentile(z_set, 25))
    max_absolute_vibrations_z.append(np.percentile(z_set, 75))

#print(mean_absolute_vibrations_x)
#print(min_absolute_vibrations_x)
#print(max_absolute_vibrations_x)
# x: saturations
# y: list of mean absolute vibration

plt.plot(saturations, mean_absolute_vibrations_x, color = 'red', label = 'x')
plt.fill_between(saturations, min_absolute_vibrations_x, max_absolute_vibrations_x, color='red', alpha = 0.25)

plt.plot(saturations, mean_absolute_vibrations_y, color = 'green', label = 'y')
plt.fill_between(saturations, min_absolute_vibrations_y, max_absolute_vibrations_y, color='green', alpha = 0.25)

plt.plot(saturations, mean_absolute_vibrations_z, color = 'blue', label = 'z')
plt.fill_between(saturations, min_absolute_vibrations_z, max_absolute_vibrations_z, color='blue', alpha = 0.25)

plt.ylabel('Acceleration in axis [m/s^2]')
plt.xlabel('Brushless Motor Saturation [%]')
plt.title("Vibration Inter-Quartile Distribution Sampled Discretely over Full Motor Saturation Range")
plt.legend()
plt.show()











input()

t_values = [float(x[1]) for x in z_and_time]

t_values = np.array(t_values)
t_diffs = np.diff(t_values)
mean_diff = np.mean(t_diffs)/1000
sample_rate = 1/mean_diff
print(sample_rate)
# find avg difference in milliseconds between samples

# plot in time domain
# test plot
t_stamps = [x/1000 for x in t_values]
plt.plot(t_stamps, y_values , linewidth=0.08, color = 'green', label="y")
plt.plot(t_stamps , x_values , linewidth=0.05, color = 'red' , label="x")
plt.plot(t_stamps, z_values , linewidth=0.05, color = 'blue', label="z", alpha = 0.8)
plt.legend()
plt.ylabel('Acceleration in axis [m/s^2]')
plt.xlabel('Time [s])')
plt.title("Acceleration as a Measure of Vibration Experienced over Full Motor Saturation Range")

plt.show()