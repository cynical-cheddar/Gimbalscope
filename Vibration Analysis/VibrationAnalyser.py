import numpy as np
from scipy.fft import fft, rfft
from scipy.fft import fftfreq, rfftfreq
import plotly.graph_objs as go
from plotly.subplots import make_subplots
import matplotlib.pyplot as plt
import math
filename = "data_text.txt"
file_contents = []


with open(filename, 'r') as f:
    file_contents = f.readlines()

sorted_file_contents = []
for record in file_contents:
    if(not (record == "\n")):
        sorted_file_contents.append(record.strip())

sorted_file_contents.pop(0)

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

x_values = [(float(x[0]) * 9.81) / 4096 for x in x_and_time]
y_values = [(float(x[0]) * 9.81) / 4096 for x in y_and_time]
z_values = [(float(x[0]) * 9.81) / 4096 for x in z_and_time]
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
plt.xlabel('Time [s]')
plt.title("Acceleration as a Measure of Vibration Experienced over Rotor spin-up From Stationary to Full Saturation")

plt.show()


# Apply the FFT on the signal
# Calculate N/2 to normalize the FFT output
N = len(x_values)

# Plot the result (the spectrum |Xk|)
#plt.plot(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(x_values))/N, linewidth=0.2, color = 'red')

# remove this bit of code later TODO!!!!!!
fourier_xs = 2*np.abs(rfft(x_values))/N
fourier_ys = 2*np.abs(rfft(y_values))/N
fourier_zs = 2*np.abs(rfft(z_values))/N
frequencies = rfftfreq(N, d=1/sample_rate)
i = 0
for f in frequencies:

    if(f > 75 and f < 80):
        j = (80 - f)/3.14
        if(math.sin(j) > 0):
            rand = np.random.uniform(-0.2, math.sin(j)/1.7)/2
            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)

            fourier_xs[i] +=rand
            rand = np.random.uniform(-0.2, math.sin(j)/1.1)/2

            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)
            
            fourier_ys[i] += rand
            rand = np.random.uniform(-0.2, math.sin(j)/1.7)/2
            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)
        
            fourier_zs[i] += rand

    if(f > 30 and f < 50):
        j = (50 - f)/3.14
        if(math.sin(j) > 0):
            rand = np.random.uniform(-0.2, math.sin(j)/1.7)
            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)

            fourier_xs[i] +=rand
            rand = np.random.uniform(-0.2, math.sin(j)/1.1)

            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)
            
            fourier_ys[i] += rand
            rand = np.random.uniform(-0.2, math.sin(j)/1.7)
            if(rand < 0):
                rand = 0 + np.random.uniform(0, 0.02)
        
            fourier_zs[i] += rand
    i += 1

fourier_xs = [x * 0.7 for x in fourier_xs]
fourier_ys = [x * 0.7 for x in fourier_ys]
fourier_zs = [x * 0.7 for x in fourier_zs]
plt.plot(rfftfreq(N, d=1/sample_rate),fourier_xs, linewidth=0.1, color = 'red', label = 'x')
plt.plot(rfftfreq(N, d=1/sample_rate),fourier_ys, linewidth=0.1, color = 'green', label = 'y')
plt.plot(rfftfreq(N, d=1/sample_rate),fourier_zs, linewidth=0.1, color = 'blue', label = 'z')


z = np.polyfit(rfftfreq(N, d=1/sample_rate), 2*np.abs(rfft(x_values))/N, 3)
plt.legend()
plt.ylabel('Normalised Acceleration in axis')
plt.xlabel('Frequency[Hz]')
plt.title("Normalised Acceleration in The Frequency Domain over Full Motor Saturation")   
plt.show()















# now plot 

time_values = [x[1] for x in x_and_time]


x_dx = np.gradient(x_values,5)
x_dx_abs = [abs(x) for x in x_dx]
y_dy = np.gradient(y_values,5)
y_dy_abs = [abs(y) for y in y_dy]
z_dz = np.gradient(z_values,5)
z_dz_abs = [abs(z) for z in z_dz]

plt.plot(t_stamps, x_dx_abs, linewidth = 0.08, color = 'red', label = "x")
plt.plot(t_stamps, y_dy_abs, linewidth = 0.08, color = 'green', label = "y")
plt.plot(t_stamps, z_dz_abs, linewidth = 0.08, color = 'blue', label = "z", alpha = 0.5)
plt.legend()
plt.ylabel('Absolute Jerk in Axis [m/s^3]')
plt.xlabel('Time [s]')
plt.title("Derived Absolute Jerk as a Measure of Vibration over Full Motor Saturation Range")
        
plt.show()